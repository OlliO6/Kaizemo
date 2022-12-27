namespace PlayerBehaviour;

using Godot;
using System;
using Additions;
using StateMachines;

public partial class Player : CharacterBody2D
{
    public enum StateId { Idle, Run, Fall, Jump, CancelJump }

    public enum StateTag { OnGround, InAir, AirHorizontalMovement }

    private const int AnimationTreeGroundedStateIdle = 0;
    private const int AnimationTreeGroundedStateRun = 1;

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

    private bool _faceLeft;

    public StateMachine StateMachine => this.GetSceneNodeCached(Scene.StateMachine.Path);
    public Timer GroundRememberTimr => this.GetSceneNodeCached(Scene.GroundRememberTimer.Path);
    public AnimationTree AnimationTree => this.GetSceneNodeCached(Scene.AnimationTree.Path);

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

    public override void _Ready()
    {
        StateMachine.AddState(StateId.Idle)
            .WithTag(StateTag.OnGround)
            .WithEnterCallback(() => AnimationTree.Set("parameters/GroundedState/current", AnimationTreeGroundedStateIdle), true)
            .WithPhysicsProcessCallback((_) => StateMachine.SwitchStateIf(StateId.Fall, !IsOnFloor()), true)
            .WithPhysicsProcessCallback((delta) =>
            {
                var horizontalInput = InputManager.GetPlayerHorizontalInput();
                ApplyHorizontalMovement(delta, horizontalInput, groundAcceleration, groundDamping, groundStopDamping);
                if (horizontalInput != 0)
                    StateMachine.SwitchToState(StateId.Run);
            });

        StateMachine.AddState(StateId.Run)
            .WithTag(StateTag.OnGround)
            .WithEnterCallback(() => this.GetSceneNodeCached(Scene.DustParticles.RunParticles.Path).Emitting = true)
            .WithEnterCallback(() => AnimationTree.Set("parameters/GroundedState/current", AnimationTreeGroundedStateRun), true)
            .WithPhysicsProcessCallback((_) => StateMachine.SwitchStateIf(StateId.Fall, !IsOnFloor()), true)
            .WithPhysicsProcessCallback((delta) =>
            {
                var horizontalInput = InputManager.GetPlayerHorizontalInput();
                ApplyHorizontalMovement(delta, horizontalInput, groundAcceleration, groundDamping, groundStopDamping);
                if (horizontalInput == 0)
                    StateMachine.SwitchToState(StateId.Idle);
            })
            .WithExitCallback(() => this.GetSceneNodeCached(Scene.DustParticles.RunParticles.Path).Emitting = false);


        StateMachine.AddState(StateId.Fall)
            .WithTag(StateTag.InAir)
            .WithTag(StateTag.AirHorizontalMovement)
            .WithEnterCallback(() =>
            {
                if (StateMachine.PreviousState.HasTag(StateTag.OnGround))
                    GroundRememberTimr.Start();
            })
            .WithPhysicsProcessCallback((delta) =>
            {
                ApplyGravity(delta, fallGravity);
                if (GroundRememberTimr.TimeLeft > 0 && InputManager.IsJumpBuffered)
                    Jump();
            })
            .WithExitCallback(GroundRememberTimr.Stop);

        StateMachine.AddState(StateId.Jump)
            .WithTag(StateTag.InAir)
            .WithTag(StateTag.AirHorizontalMovement)
            .WithPhysicsProcessCallback((delta) => ApplyGravity(delta, jumpGravity))
            .WithPhysicsProcessCallback((_) => StateMachine.SwitchStateIf(StateId.CancelJump, !InputManager.IsHoldingJump))
            .WithPhysicsProcessCallback((_) => StateMachine.SwitchStateIf(StateId.Fall, Velocity.y > 0));

        StateMachine.AddState(StateId.CancelJump)
            .WithTag(StateTag.InAir)
            .WithTag(StateTag.AirHorizontalMovement)
            .WithEnterCallback(() => this.SetVelocityY(Velocity.y * (1 - jumpCancelStrenght)))
            .WithPhysicsProcessCallback((delta) => ApplyGravity(delta, jumpCancelGravity))
            .WithPhysicsProcessCallback((_) => StateMachine.SwitchStateIf(StateId.Fall, Velocity.y > 0));

        StateMachine.WithPhysicsProcessTagCallback(StateTag.OnGround, (_) =>
        {
            this.SetVelocityY(1);

            if (InputManager.IsJumpBuffered)
                Jump();
        });

        StateMachine.WithPhysicsProcessTagCallback(StateTag.InAir, (_) =>
        {
            AnimationTree.Set("parameters/Falling/blend_position", Velocity.y);

            if (IsOnFloor())
                StateMachine.SwitchToState(StateId.Idle);
        }, true);

        StateMachine.WithPhysicsProcessTagCallback(StateTag.AirHorizontalMovement,
            (delta) => ApplyHorizontalMovement(delta, InputManager.GetPlayerHorizontalInput(), airAcceleration, airDamping, airStopDamping));

        StateMachine.WithEnterTagCallback(StateTag.OnGround, () =>
        {
            AnimationTree.Set("parameters/Grounded/current", 1);

            if (StateMachine.PreviousState?.Id.Equals(StateId.Fall) ?? false)
            {
                AnimationTree.Set("parameters/Landed/active", true);
                this.GetSceneNodeCached(Scene.DustParticles.LandParticles.Path).Restart();
            }
        });

        StateMachine.WithEnterTagCallback(StateTag.InAir, () =>
        {
            AnimationTree.Set("parameters/Grounded/current", 0);
        });

        StateMachine.PhysicsProcessApplied += (delta) =>
        {
            MoveAndSlide();
        };

        StateMachine.SwitchToState(StateId.Idle);
    }

    private void ApplyGravity(double delta, float gravity) => this.SetVelocityY(Velocity.y + gravity * (float)delta);

    private void ApplyHorizontalMovement(double delta, float horizontalInput, float acceleration, float damping, float stopDamping)
    {
        var velocity = Velocity;
        velocity.x += horizontalInput * acceleration * (float)delta;
        velocity.x *= Mathf.Pow(1f - (horizontalInput == 0 ? stopDamping : damping), (float)delta * 10f);
        Velocity = velocity;

        if (horizontalInput > 0)
        {
            FaceLeft = false;
            return;
        }
        if (horizontalInput < 0)
            FaceLeft = true;
    }

    private void Jump()
    {
        this.SetVelocityY(jumpVelocity);
        StateMachine.SwitchToState(StateId.Jump);
    }

    public override void _Process(double delta)
    {
        Scene.StateLabel.GetCached(this).Text = StateMachine.GetCurrentStateAsString(true);
    }
}
