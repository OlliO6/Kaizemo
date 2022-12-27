// namespace PlayerBehaviour;

// using System;
// using System.Reflection;
// using Additions;
// using Godot;

// public partial class PlayerOld : CharacterBody2D
// {
//     [Signal] public delegate void JumpedEventHandler();

//     public const float JumpLenienceTime = 0.15f;

//     [ExportGroup("Movement")]
//     [Export] public float baseGravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

//     [ExportSubgroup("Jumping", "jump")]
//     [Export] public float jumpVelocity;
//     [Export] public float jumpGravityScale = 1, jumpCancelGravityScale = 1;
//     [Export(PropertyHint.Range, "0,1")] public float jumpCancelStrenght;

//     [ExportSubgroup("In Air", "air")]
//     [Export] public float airAcceleration;
//     [Export(PropertyHint.Range, "0,1")] public float airDamping;

//     [ExportSubgroup("On Ground", "ground")]
//     [Export] public float groundAcceleration;
//     [Export(PropertyHint.Range, "0,1")] public float groundDamping;
//     [Export(PropertyHint.Range, "0,1")] public float groundStopDamping;

//     [ExportSubgroup("Dive", "dive")]
//     [Export] public Vector2 diveUpVelocity;
//     [Export] public Vector2 diveHorizontalVelocity;

//     public bool isGrounded, isJumping;

//     private Timer _groundRememberTimer;
//     private bool _faceLeft, _isCancelingJump;

//     public AnimationTree AnimTree => Scene.AnimationTree.GetCached(this);
//     public GPUParticles2D RunParticles => Scene.DustParticles.RunParticles.GetCached(this);
//     public GPUParticles2D JumpParticles => Scene.DustParticles.JumpParticles.GetCached(this);
//     public GPUParticles2D LandParticles => Scene.DustParticles.LandParticles.GetCached(this);
//     public GPUParticles2D DiveParticles => Scene.DustParticles.DiveParticles.GetCached(this);

//     public bool FaceLeft
//     {
//         get => _faceLeft;
//         set
//         {
//             _faceLeft = value;

//             if (value)
//             {
//                 Scale = new(1, -1);
//                 Rotation = Mathf.DegToRad(180);
//                 return;
//             }

//             Scale = new(1, 1);
//             Rotation = 0;
//         }
//     }

//     public override void _EnterTree()
//     {
//         InputManager.JumpReleased += OnJumpReleased;
//     }

//     public override void _ExitTree()
//     {
//         InputManager.JumpReleased -= OnJumpReleased;
//     }

//     public override void _Ready()
//     {
//         AddChild(
//             _groundRememberTimer = new()
//             {
//                 WaitTime = JumpLenienceTime,
//                 OneShot = true
//             });
//     }

//     public override void _PhysicsProcess(double delta)
//     {
//         float deltaf = (float)delta;

//         Vector2 velocity = Velocity;
//         float horizontalInput = InputManager.GetPlayerHorizontalInput();

//         ApplyGravity();
//         MoveHorizontal();
//         HandleJumping();
//         Move();
//         UpdateIsGrounded();
//         Animate();

//         void Move()
//         {
//             Velocity = velocity;

//             if (MoveAndSlide())
//                 HandleCollisions();
//         }

//         void ApplyGravity()
//         {
//             if (isGrounded)
//                 return;

//             if (isJumping)
//             {
//                 velocity.y += baseGravity * jumpGravityScale * deltaf;
//                 return;
//             }

//             if (_isCancelingJump)
//             {
//                 velocity.y += baseGravity * jumpCancelGravityScale * deltaf;
//                 return;
//             }

//             velocity.y += baseGravity * deltaf;
//         }

//         void MoveHorizontal()
//         {
//             if (horizontalInput != 0)
//                 FaceLeft = horizontalInput < 0;

//             if (isGrounded)
//             {
//                 velocity.x += horizontalInput * groundAcceleration * deltaf;
//                 velocity.x *= Mathf.Pow(1f - (horizontalInput == 0 ? groundStopDamping : groundDamping), deltaf * 10f);
//                 return;
//             }

//             velocity.x += InputManager.GetPlayerHorizontalInput() * airAcceleration * deltaf;
//             velocity.x *= Mathf.Pow(1f - airDamping, deltaf * 10f);
//         }

//         void HandleJumping()
//         {
//             if (InputManager.IsJumpBuffered && (isGrounded || (_groundRememberTimer.TimeLeft != 0 && !_groundRememberTimer.IsStopped())))
//             {
//                 Jump(jumpVelocity, ref velocity);
//                 return;
//             }

//             if (isGrounded)
//             {
//                 velocity.y = 1;
//                 isJumping = false;
//                 _isCancelingJump = false;
//                 return;
//             }

//             if (Velocity.y > 0)
//             {
//                 isJumping = false;
//                 _isCancelingJump = false;
//             }
//         }

//         void HandleCollisions()
//         {
//             for (int i = 0; i < GetSlideCollisionCount(); i++)
//             {
//                 var collision = GetSlideCollision(i);

//                 if (collision.GetCollider() is LevelMap map)
//                     map.ProcessCollision(this, collision);
//             }
//         }

//         void UpdateIsGrounded()
//         {
//             if (IsOnFloor() != isGrounded)
//             {
//                 isGrounded = IsOnFloor();

//                 if (isGrounded) Land();
//                 else LeaveGround();
//             }
//         }

//         void Animate()
//         {
//             if (!isGrounded)
//             {
//                 AnimTree.Set("Grounded/current", 0);
//                 AnimTree.Set("FallSpeed/blend_position", Velocity.y);
//                 return;
//             }

//             AnimTree.Set("Grounded/current", 1);

//             if (horizontalInput != 0)
//             {
//                 AnimTree.Set("parameters/GroundedState/currentt", (int)GroundedAnimationState.Run);
//                 AnimTree.Set("RunSpeed/scale", horizontalInput.Abs());
//                 RunParticles.Emitting = true;
//                 return;
//             }

//             AnimTree.Set("parameters/GroundedState/current", (int)GroundedAnimationState.Idle);
//             RunParticles.Emitting = false;
//         }
//     }

//     private void OnJumpReleased()
//     {
//         if (!isJumping)
//             return;

//         Vector2 velocity = Velocity;
//         CancelJump(ref velocity);
//         Velocity = velocity;
//     }

//     private void Jump(float jumpVelocity, ref Vector2 velocity)
//     {
//         GetEmitter<SignalEmitter.Jumped>().Emit();

//         isJumping = true;
//         velocity.y = jumpVelocity;

//         InputManager.UseJumpBuffer();

//         if (!InputManager.IsHoldingJump)
//             CancelJump(ref velocity);

//         JumpParticles.Restart();
//     }

//     private void CancelJump(ref Vector2 velocity)
//     {
//         isJumping = false;
//         _isCancelingJump = true;
//         velocity.y *= 1f - jumpCancelStrenght;
//     }

//     private void LeaveGround()
//     {
//         if (!isJumping)
//             _groundRememberTimer.Start();

//         AnimTree.Set("parameters/Landed/active", false);
//     }

//     private void Land()
//     {
//         AnimTree.Set("parameters/Landed/active", true);
//         LandParticles.Restart();
//     }

//     public void Die()
//     {
//         SetProcess(false);
//         SetPhysicsProcess(false);

//         AnimTree.Set("parameters/Dead/current", 1);

//         GD.Print("Player died.");
//     }

//     public enum ActionDirection
//     {
//         Up,
//         Down,
//         Left,
//         Right
//     }

//     private enum GroundedAnimationState
//     {
//         Idle,
//         Run
//     }
// }
