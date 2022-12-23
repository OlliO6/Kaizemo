using System;
using System.Reflection;
using Godot;

public partial class Player : CharacterBody2D
{
    [Signal] public delegate void JumpedEventHandler(Player sender, bool highJump, Test test);
    [Signal] public delegate void LandedEventHandler(Player sender);

    public const float JumpLenienceTime = 0.1f;

    [Export, ExportGroup("Movement")] public float jumpVelocity;
    [Export] public float gravityScale = 1, jumpingGravityScale;
    [Export(PropertyHint.Range, "0,1")] public float jumpCancelStrenght;
    [Export] public float groundedAcceleration, airAcceleration;
    [Export(PropertyHint.Range, "0,1")] public float groundDamping;
    [Export(PropertyHint.Range, "0,1")] public float groundedStopDamping;
    [Export(PropertyHint.Range, "0,1")] public float airDamping;
    [Export] public Vector2 diveUpVelocity;
    [Export] public Vector2 diveHorizontalVelocity;

    public float baseGravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    public LevelMap map;

    private Timer groundRememberTimer;
    private bool isGrounded, isJumping;
    private bool faceLeft;

    public AnimationPlayer Anim => Scene.AnimationPlayer.GetCached(this);

    public bool FaceLeft
    {
        get => faceLeft;
        set
        {
            faceLeft = value;

            if (value)
            {
                Scale = new(1, -1);
                Rotation = Mathf.DegToRad(180);
                return;
            }

            Scale = new(1, 1);
            Rotation = 0;
        }
    }

    public override void _EnterTree()
    {
        InputManager.JumpReleased += OnJumpReleased;
    }

    public override void _ExitTree()
    {
        InputManager.JumpReleased -= OnJumpReleased;
    }

    public override void _Ready()
    {
        AddChild(
            groundRememberTimer = new()
            {
                WaitTime = JumpLenienceTime,
                OneShot = true
            });
    }

    public override void _PhysicsProcess(double delta)
    {
        float deltaf = (float)delta;

        Vector2 velocity = Velocity;
        float horizontalInput = InputManager.GetPlayerHorizontalInput();

        ApplyGravity();
        MoveHorizontal();
        HandleJumping();
        Move();
        UpdateIsGrounded();

        void Move()
        {
            Velocity = velocity;

            if (MoveAndSlide())
                HandleCollisions();
        }

        void ApplyGravity()
        {
            if (isGrounded)
                return;

            if (isJumping)
            {
                velocity.y += baseGravity * jumpingGravityScale * deltaf;
                return;
            }

            velocity.y += baseGravity * gravityScale * deltaf;
        }

        void MoveHorizontal()
        {
            if (horizontalInput != 0)
                FaceLeft = horizontalInput < 0;

            if (isGrounded)
            {
                velocity.x += horizontalInput * groundedAcceleration * deltaf;
                velocity.x *= Mathf.Pow(1f - (horizontalInput == 0 ? groundedStopDamping : groundDamping), deltaf * 10f);
                return;
            }

            velocity.x += InputManager.GetPlayerHorizontalInput() * airAcceleration * deltaf;
            velocity.x *= Mathf.Pow(1f - airDamping, deltaf * 10f);
        }

        void HandleJumping()
        {
            if (InputManager.IsJumpBuffered && (isGrounded || (groundRememberTimer.TimeLeft != 0 && !groundRememberTimer.IsStopped())))
            {
                Jump(jumpVelocity, ref velocity);
                return;
            }

            if (isGrounded)
            {
                velocity.y = 1;
                isJumping = false;
                return;
            }

            if (Velocity.y > 0) isJumping = false;
        }

        void HandleCollisions()
        {
            for (int i = 0; i < GetSlideCollisionCount(); i++)
            {
                var collision = GetSlideCollision(i);

                if (collision.GetCollider() is LevelMap map)
                    map.ProcessCollision(this, collision);
            }
        }

        void UpdateIsGrounded()
        {
            if (IsOnFloor() != isGrounded)
            {
                isGrounded = IsOnFloor();

                if (isGrounded) Land();
                else LeaveGround();
            }
        }
    }

    private void OnJumpReleased()
    {
        if (!isJumping)
            return;

        Vector2 velocity = Velocity;
        CancelJump(ref velocity);
        Velocity = velocity;
    }

    private void Jump(float jumpVelocity, ref Vector2 velocity)
    {
        // GetEmitter<SignalEmitter.Jumped>().Emit(this, false);

        isJumping = true;
        velocity.y = jumpVelocity;

        InputManager.UseJumpBuffer();
        groundRememberTimer.Stop();

        if (!InputManager.IsHoldingJump)
            CancelJump(ref velocity);
    }

    private void CancelJump(ref Vector2 velocity)
    {
        isJumping = false;
        velocity.y *= 1f - jumpCancelStrenght;
    }

    private void LeaveGround()
    {
        groundRememberTimer.Start();
    }

    private void Land()
    {
        // Emit.Landed(this);
    }

    public void Die()
    {
        GD.Print("Player died.");
    }
}

public enum Test { guzg, uhgik }
