namespace Game.PlayerBehaviour;

using Godot;
using System;

public partial class PlayerAnimationTree : AnimationTree
{
    public AnimationNodeStateMachinePlayback AlivePlayback => (AnimationNodeStateMachinePlayback)Get("parameters/Alive/playback").Obj;
    public AnimationNodeStateMachinePlayback OnGroundPlayback => (AnimationNodeStateMachinePlayback)Get("parameters/Alive/OnGround/StateMachine/playback").Obj;
    public AnimationNodeStateMachinePlayback InAirPlayback => (AnimationNodeStateMachinePlayback)Get("parameters/Alive/InAir/StateMachine/playback").Obj;

    public bool IsDead
    {
        get => Get("parameters/conditions/IsDead").AsBool();
        set => Set("parameters/conditions/IsDead", value);
    }

    public bool LandActive
    {
        get => Get("parameters/Alive/OnGround/OneShot/active").AsBool();
        set => Set("parameters/Alive/OnGround/OneShot/active", value);
    }
}
