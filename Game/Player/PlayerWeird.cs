// namespace PlayerBehaviour;

// using System;
// using System.Reflection;
// using Additions;
// using Godot;
// using StateMachines;

// public partial class Player : CharacterBody2D
// {
//     public enum StateId { Idle, Run, Jump, CancelJump, Fall }
//     public enum StateTag { InAir, OnGround }

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

//     private bool _faceLeft;

//     public StateMachine StateMachine => this.GetSceneNodeCached(Scene.StateMachine.Path);
//     public AnimationTree AnimTree => this.GetSceneNodeCached(Scene.AnimationTree.Path);
//     public GPUParticles2D RunParticles => this.GetSceneNodeCached(Scene.DustParticles.RunParticles.Path);
//     public GPUParticles2D JumpParticles => this.GetSceneNodeCached(Scene.DustParticles.JumpParticles.Path);
//     public GPUParticles2D LandParticles => this.GetSceneNodeCached(Scene.DustParticles.LandParticles.Path);
//     public GPUParticles2D DiveParticles => this.GetSceneNodeCached(Scene.DustParticles.DiveParticles.Path);
//     public Timer GroundRemberTimer => this.GetSceneNodeCached(Scene.GroundRememberTimer.Path);

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

//     public override void _Ready()
//     {
//         StateMachine.AddState(StateId.Idle)
//             .WithTag(StateTag.OnGround)
//             .WithPhysicsProcessCallback((_) =>
//             {

//             });

//         StateMachine.AddState(StateId.Run)
//             .WithTag(StateTag.OnGround);
//         // .WithPhysicsProcessCallback(IdleSwitch)

//         StateMachine.AddState(StateId.Jump)
//             .WithTag(StateTag.InAir)
//             .WithPhysicsProcessCallback(
//                 (_) => StateMachine.SwitchStateIf(StateId.CancelJump, !InputManager.IsHoldingJump));

//         StateMachine.AddState(StateId.CancelJump)
//             .WithTag(StateTag.InAir);

//         StateMachine.AddState(StateId.Fall)
//             .WithTag(StateTag.InAir)
//             .WithEnterCallback(() =>
//             {
//                 if (StateMachine.PreviousState.HasTag(StateTag.OnGround))
//                     GroundRemberTimer.Start();
//             })
//             .WithPhysicsProcessCallback((_) =>
//             {
//                 if (GroundRemberTimer.TimeLeft > 0 && InputManager.IsJumpBuffered)
//                     Jump();
//             });



//         StateMachine.WithPhysicsProcessTagCallback(StateTag.OnGround, (_) =>
//         {
//             if (InputManager.IsJumpBuffered)
//                 Jump();
//         });

//         StateMachine.WithPhysicsProcessTagCallback(StateTag.InAir, (_) =>
//         {
//             if (IsOnFloor())
//                 Land();
//         }, true);

//         StateMachine.WithPhysicsProcessTagCallback(StateTag.InAir, (_) =>
//         {

//         });

//         StateMachine.SwitchToState(StateId.Idle);
//     }

//     public void Jump()
//     {
//         this.SetVelocityY(jumpVelocity);
//         StateMachine.SwitchToState(StateId.Jump);
//     }

//     public void Land()
//     {
//         StateMachine.SwitchToState(StateId.Idle);
//     }

//     private void FallSwitch(double delta)
//     {
//         if (Velocity.y > 0)
//             StateMachine.SwitchToState(StateId.Fall);
//     }
// }
