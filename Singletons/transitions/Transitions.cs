using Godot;
using System;

public partial class Transitions : CanvasLayer
{
    public static Transitions Instance { get; private set; }

    public override void _EnterTree()
    {
        Instance = this;
    }

    public static void StartTransition(TransitionType transitionType, Action finished)
    {
        Scene.AnimationPlayer.GetCached(Instance).Play("Start" + transitionType.ToString());

        Instance.ToSignal(Scene.AnimationPlayer.GetCached(Instance), AnimationPlayer.SignalName.AnimationFinished)
            .OnCompleted(finished);
    }

    public static void EndTransition(TransitionType transitionType, Action finished = null)
    {
        Scene.AnimationPlayer.GetCached(Instance).Play("End" + transitionType.ToString());

        if (finished != null) Instance.ToSignal(Scene.AnimationPlayer.GetCached(Instance), AnimationPlayer.SignalName.AnimationFinished)
            .OnCompleted(finished);
    }
}

public enum TransitionType { BlackFade }
