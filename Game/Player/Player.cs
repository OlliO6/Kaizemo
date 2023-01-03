namespace Game.PlayerBehaviour;

using Godot;
using System;
using StateMachines;

public partial class Player : CharacterBody2D, ILoadAbilityObtainer, IKillable
{
    public enum MainStateId { Idle, Run, Fall, Jump, CancelJump, Dead, Dive, UpwardsDive }

    public enum MainStateTag { OnGround, InAir, AirHorizontalMovement }

    public enum LoadAbilityStateId { Nothing, Dive }

    public enum LoadAbilityStateTag { LooseOnGround }

    private const int AnimationTreeGroundedStateIdle = 0;
    private const int AnimationTreeGroundedStateRun = 1;

    #region Exports 

    [ExportGroup("Movement")]
    [Export] public float fallGravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    [ExportSubgroup("Jump", "jump")]
    [Export] public float jumpVelocity;
    [Export] public float jumpGravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    [Export] public float jumpCancelGravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    [Export(PropertyHint.Range, "0,1")] public float jumpCancelStrenght;

    [ExportSubgroup("OnGround", "ground")]
    [Export] public float groundAcceleration;
    [Export(PropertyHint.Range, "0,1")] public float groundDamping, groundStopDamping;

    [ExportSubgroup("InAir", "air")]
    [Export] public float airAcceleration;
    [Export(PropertyHint.Range, "0,1")] public float airDamping, airStopDamping;

    [ExportSubgroup("Dive", "dive")]
    [Export] public float diveDuration;
    [Export] public float diveGravity;
    [Export(PropertyHint.Range, "0,1")] public Vector2 diveVelocityRemain;
    [Export] public Vector2 diveUpVelocity;
    [Export] public Vector2 diveDownVelocity;
    [Export] public Vector2 diveLeftVelocity;
    [Export] public Vector2 diveRightVelocity;

    [ExportGroup("LoadAbilityColors")]
    [Export] public Color diveOutlineColor;

    #endregion Exports

    private bool _faceLeft;

    public event Action LoadedAbilityChanged;

    public StateMachine MainStateMachine => Scene.MainStateMachine.GetCached(this);
    public StateMachine LoadAbilityStateMachine => Scene.LoadAbilityStateMachine.GetCached(this);
    public Timer GroundRememberTimer => Scene.GroundRememberTimer.GetCached(this);
    public Timer DiveTimer => Scene.DiveTimer.GetCached(this);
    public PlayerAnimationTree AnimTree => Scene.AnimationTree.GetCached(this);
    public Sprite2D Sprite => Scene.Sprite.GetCached(this);

    public bool FaceLeft
    {
        get => _faceLeft;
        set
        {
            _faceLeft = value;
            if (value)
            {
                Scale = new(1, -1);
                RotationDegrees = 180;
                return;
            }
            Scale = new(1, 1);
            Rotation = 0;
        }
    }

    public override void _EnterTree()
    {
        InputManager.ActionPressed += ActionPressedCallback;
    }

    public override void _Ready()
    {
        #region MainStateMachine 

        MainStateMachine.AddState(MainStateId.Idle)
            .WithTag(MainStateTag.OnGround)
            .WithEnterCallback(() => AnimTree.OnGroundPlayback.Travel("Idle"))
            .WithPhysicsProcessCallback((_) => MainStateMachine.SwitchStateIf(MainStateId.Fall, !IsOnFloor()), true)
            .WithPhysicsProcessCallback((delta) =>
            {
                var horizontalInput = InputManager.GetPlayerHorizontalInput();
                ApplyHorizontalMovement(delta, horizontalInput, groundAcceleration, groundDamping, groundStopDamping);
                if (horizontalInput != 0)
                    MainStateMachine.SwitchToState(MainStateId.Run);
            });

        MainStateMachine.AddState(MainStateId.Run)
            .WithTag(MainStateTag.OnGround)
            .WithEnterCallback(() => AnimTree.OnGroundPlayback.Travel("Run"))
            .WithEnterCallback(() => Scene.DustParticles.RunParticles.GetCached(this).Emitting = true)
            .WithPhysicsProcessCallback((_) => MainStateMachine.SwitchStateIf(MainStateId.Fall, !IsOnFloor()), true)
            .WithPhysicsProcessCallback((delta) =>
            {
                var horizontalInput = InputManager.GetPlayerHorizontalInput();
                ApplyHorizontalMovement(delta, horizontalInput, groundAcceleration, groundDamping, groundStopDamping);
                if (horizontalInput == 0)
                    MainStateMachine.SwitchToState(MainStateId.Idle);
            })
            .WithExitCallback(() => Scene.DustParticles.RunParticles.GetCached(this).Emitting = false);

        MainStateMachine.AddState(MainStateId.Dead)
            .WithEnterCallback(() => AnimTree.IsDead = true);

        MainStateMachine.AddState(MainStateId.Fall)
            .WithTag(MainStateTag.InAir)
            .WithTag(MainStateTag.AirHorizontalMovement)
            .WithEnterCallback(() => AnimTree.InAirPlayback.Travel("Fall"))
            .WithEnterCallback(() =>
            {
                if (MainStateMachine.PreviousState.HasTag(MainStateTag.OnGround))
                    GroundRememberTimer.Start();
            })
            .WithPhysicsProcessCallback((delta) =>
            {
                ApplyGravity(delta, fallGravity);
                if (!GroundRememberTimer.IsStopped() && InputManager.IsJumpBuffered)
                    Jump();
            })
            .WithPhysicsProcessCallback((_) => MainStateMachine.SwitchStateIf(MainStateId.Idle, IsOnFloor()))
            .WithExitCallback(GroundRememberTimer.Stop);

        MainStateMachine.AddState(MainStateId.Jump)
            .WithTag(MainStateTag.InAir)
            .WithTag(MainStateTag.AirHorizontalMovement)
            .WithEnterCallback(() => Scene.DustParticles.JumpParticles.GetCached(this).Restart())
            .WithEnterCallback(() => AnimTree.InAirPlayback.Travel("Jump"))
            .WithPhysicsProcessCallback((delta) => ApplyGravity(delta, jumpGravity))
            .WithPhysicsProcessCallback((_) => MainStateMachine.SwitchStateIf(MainStateId.CancelJump, !InputManager.IsHoldingJump))
            .WithPhysicsProcessCallback((_) => MainStateMachine.SwitchStateIf(MainStateId.Fall, Velocity.y > 0));

        MainStateMachine.AddState(MainStateId.CancelJump)
            .WithTag(MainStateTag.InAir)
            .WithTag(MainStateTag.AirHorizontalMovement)
            .WithEnterCallback(() => AnimTree.InAirPlayback.Travel("CancelJump"))
            .WithEnterCallback(() => this.SetVelocityY(Velocity.y * (1 - jumpCancelStrenght)))
            .WithPhysicsProcessCallback((delta) => ApplyGravity(delta, jumpCancelGravity))
            .WithPhysicsProcessCallback((_) => MainStateMachine.SwitchStateIf(MainStateId.Fall, Velocity.y > 0));

        MainStateMachine.AddState(MainStateId.Dive)
            .WithTag(MainStateTag.InAir)
            .WithTag(MainStateTag.AirHorizontalMovement)
            .WithEnterCallback(() => Scene.DustParticles.DiveParticles.GetCached(this).Emitting = true)
            .WithEnterCallback(() => AnimTree.InAirPlayback.Travel("Dive"))
            .WithEnterCallback(() => DiveTimer.Start(diveDuration))
            .WithPhysicsProcessCallback((delta) => ApplyGravity(delta, diveGravity))
            .WithExitCallback(() => Scene.DustParticles.DiveParticles.GetCached(this).Emitting = false)
            .WithExitCallback(() => DiveTimer.Stop());

        MainStateMachine.AddState(MainStateId.UpwardsDive)
            .WithTag(MainStateTag.InAir)
            .WithTag(MainStateTag.AirHorizontalMovement)
            .WithEnterCallback(() => Scene.DustParticles.DiveParticles.GetCached(this).Emitting = true)
            .WithEnterCallback(() => AnimTree.InAirPlayback.Travel("Dive"))
            .WithEnterCallback(() => DiveTimer.Start(diveDuration))
            .WithPhysicsProcessCallback((delta) => ApplyGravity(delta, diveGravity))
            .WithExitCallback(() => Scene.DustParticles.DiveParticles.GetCached(this).Emitting = false)
            .WithExitCallback(() => DiveTimer.Stop());

        DiveTimer.Timeout += () => MainStateMachine.SwitchToState(MainStateId.Fall);

        MainStateMachine.WithPhysicsProcessTagCallback(MainStateTag.OnGround, (_) =>
        {
            this.SetVelocityY(1);

            if (InputManager.IsJumpBuffered)
                Jump();
        });

        MainStateMachine.WithPhysicsProcessTagCallback(MainStateTag.AirHorizontalMovement,
            (delta) => ApplyHorizontalMovement(delta, InputManager.GetPlayerHorizontalInput(), airAcceleration, airDamping, airStopDamping));

        MainStateMachine.WithEnterTagCallback(MainStateTag.OnGround, () =>
        {
            AnimTree.AlivePlayback.Travel("OnGround");

            if (MainStateMachine.PreviousState?.Id.Equals(MainStateId.Fall) ?? false)
            {
                AnimTree.LandActive = true;
                Scene.DustParticles.LandParticles.GetCached(this).Restart();
            }
        });

        MainStateMachine.WithExitTagCallback(MainStateTag.OnGround, () =>
        {
            AnimTree.LandActive = false;
        });

        MainStateMachine.WithEnterTagCallback(MainStateTag.InAir, () =>
        {
            AnimTree.AlivePlayback.Travel("InAir");
        });

        MainStateMachine.SwitchToState(MainStateId.Idle);

        MainStateMachine.PhysicsProcessApplied += (delta) =>
        {
            if (MainStateMachine.CurrentState.HasTag(MainStateTag.InAir))
                ApplyCornerPushing(delta);

            MoveAndSlide();
        };

        #endregion MainStateMachine

        #region LoadAbilityStateMachine

        LoadAbilityStateMachine.StateChanged += () => LoadedAbilityChanged?.Invoke();

        LoadAbilityStateMachine.AddState(LoadAbilityStateId.Nothing)
            .WithEnterCallback(() => Sprite.Material.Set("shader_parameter/apply", false))
            .WithExitCallback(() => Sprite.Material.Set("shader_parameter/apply", true));

        LoadAbilityStateMachine.AddState(LoadAbilityStateId.Dive)
            .WithTag(LoadAbilityStateTag.LooseOnGround)
            .WithEnterCallback(() =>
            {
                Sprite.Material.Set("shader_parameter/newColor", diveOutlineColor);
                Scene.Sounds.GainDive.GetCached(this).Play();
            });

        LoadAbilityStateMachine.WithPhysicsProcessTagCallback(LoadAbilityStateTag.LooseOnGround, (_) =>
        {
            LoadAbilityStateMachine.SwitchStateIf(LoadAbilityStateId.Nothing, IsOnFloor());
        }, true);

        LoadAbilityStateMachine.SwitchToState(LoadAbilityStateId.Nothing);

        #endregion LoadAbilityStateMachine

        Scene.PlayerCam.Get(this).Init(this);
    }

    private void ApplyCornerPushing(double delta)
    {
        const float MinYVelocityToDoSidewardsPushing = -20;

        if (Velocity.y < 0)
        {
            ApplyUpwardsCornerPushing(delta);
        }

        if (Velocity.y > MinYVelocityToDoSidewardsPushing && (FaceLeft ? -Velocity.x : Velocity.x) > 0)
        {
            ApplySidewardsCornerPushing(delta);
        }
    }

    private void ApplyGravity(double delta, float gravity) => this.SetVelocityY(Velocity.y + gravity * (float)delta);

    private void ApplyHorizontalMovement(double delta, float horizontalInput, float acceleration, float damping, float stopDamping)
    {
        var velocity = Velocity;
        velocity.x += horizontalInput * acceleration * (float)delta;
        velocity.x *= Mathf.Pow(1f - (horizontalInput == 0 ? stopDamping : damping), (float)delta * 10f);
        Velocity = velocity;

        if (horizontalInput != 0)
            FaceLeft = horizontalInput < 0;
    }

    private void Jump()
    {
        InputManager.UseJumpBuffer();
        this.SetVelocityY(jumpVelocity);
        MainStateMachine.SwitchToState(MainStateId.Jump);
        Scene.Sounds.Jump.GetCached(this).Play();
    }

    private void Dive(InputManager.ActionDirection direction)
    {
        Velocity = Velocity * diveVelocityRemain;

        Velocity += direction switch
        {
            InputManager.ActionDirection.Up => diveUpVelocity,
            InputManager.ActionDirection.Down => diveDownVelocity,
            InputManager.ActionDirection.Left => diveLeftVelocity,
            InputManager.ActionDirection.Right => diveRightVelocity,
            _ => Vector2.Zero
        };

        if (direction is InputManager.ActionDirection.Left)
            FaceLeft = true;
        else if (direction is InputManager.ActionDirection.Right)
            FaceLeft = false;

        LoadAbilityStateMachine.SwitchToState(LoadAbilityStateId.Nothing);
        MainStateMachine.SwitchToState(direction is InputManager.ActionDirection.Up ? MainStateId.UpwardsDive : MainStateId.Dive);
        Scene.Sounds.Dive.GetCached(this).Play();
    }

    public void Die()
    {
        MainStateMachine.SwitchToState(MainStateId.Dead);
        Scene.Sounds.Die.GetCached(this).Play();
    }

    private void ActionPressedCallback(InputManager.ActionDirection direction)
    {
        // if (holding) ThrowHeldItem; return;

        if (LoadAbilityStateMachine.CurrentState.Id is LoadAbilityStateId.Dive)
        {
            Dive(direction);
            return;
        }
    }

    private void ApplyUpwardsCornerPushing(double delta)
    {
        const float MinPushLenght = 0.1f;
        const int MaxPushSteps = 50;

        Vector2 rayTargetPos = new Vector2(0, Velocity.y * (float)delta);

        var rayHolder = Scene.UpwardsCornerPushRaycasts.GetCached(this);
        var leftRay = Scene.UpwardsCornerPushRaycasts.Left.GetCached(this);
        var midLeftRay = Scene.UpwardsCornerPushRaycasts.MiddleLeft.GetCached(this);

        rayHolder.GlobalPosition = GlobalPosition;

        leftRay.TargetPosition = rayTargetPos;
        leftRay.ForceRaycastUpdate();
        midLeftRay.TargetPosition = rayTargetPos;
        midLeftRay.ForceRaycastUpdate();

        if (leftRay.IsColliding() && !midLeftRay.IsColliding())
        {
            PushRight();
            return;
        }

        var rightRay = Scene.UpwardsCornerPushRaycasts.Right.GetCached(this);
        var midRightRay = Scene.UpwardsCornerPushRaycasts.MiddleRight.GetCached(this);

        rightRay.TargetPosition = rayTargetPos;
        rightRay.ForceRaycastUpdate();
        midRightRay.TargetPosition = rayTargetPos;
        midRightRay.ForceRaycastUpdate();

        if (rightRay.IsColliding() && !midRightRay.IsColliding())
        {
            PushLeft();
            return;
        }

        void PushRight()
        {
            for (int i = 0; i < MaxPushSteps; i++)
            {
                GlobalPosition = new Vector2(GlobalPosition.x + MinPushLenght, GlobalPosition.y);
                rayHolder.GlobalPosition = GlobalPosition;
                leftRay.ForceRaycastUpdate();

                if (!leftRay.IsColliding())
                    break;
            }
        }

        void PushLeft()
        {
            for (int i = 0; i < MaxPushSteps; i++)
            {
                GlobalPosition = new Vector2(GlobalPosition.x - MinPushLenght, GlobalPosition.y);
                rayHolder.GlobalPosition = GlobalPosition;
                rightRay.ForceRaycastUpdate();

                if (!rightRay.IsColliding())
                    break;
            }
        }
    }

    private void ApplySidewardsCornerPushing(double delta)
    {
        const float MinPushLenght = 0.1f;
        const int MaxPushSteps = 50;

        Vector2 rayTargetPos = new Vector2(Velocity.x.Abs() * (float)delta, 0);

        var bottomRay = Scene.SidewardsCornerPushRaycasts.Bottom.GetCached(this);
        var middleRay = Scene.SidewardsCornerPushRaycasts.Middle.GetCached(this);

        bottomRay.TargetPosition = rayTargetPos;
        bottomRay.ForceRaycastUpdate();
        middleRay.TargetPosition = rayTargetPos;
        middleRay.ForceRaycastUpdate();

        if (bottomRay.IsColliding() && !middleRay.IsColliding())
        {
            PushUp();
        }

        void PushUp()
        {
            for (int i = 0; i < MaxPushSteps; i++)
            {
                GlobalPosition = new Vector2(GlobalPosition.x, GlobalPosition.y - MinPushLenght);
                bottomRay.ForceRaycastUpdate();

                if (!bottomRay.IsColliding())
                    break;
            }

            MainStateMachine.SwitchToState(MainStateId.Fall);
            this.SetVelocityY(0);
        }
    }

    public void ObtainLoadAbility(LoadAbility loadAbility)
    {
        switch (loadAbility)
        {
            case LoadAbility.Dive:
                LoadAbilityStateMachine.SwitchToState(LoadAbilityStateId.Dive);
                break;
        }
    }

    public LoadAbility? GetLoadedAbility()
    {
        return LoadAbilityStateMachine.CurrentState.Id switch
        {
            LoadAbilityStateId.Dive => LoadAbility.Dive,
            _ or LoadAbilityStateId.Nothing => null
        };
    }
}
