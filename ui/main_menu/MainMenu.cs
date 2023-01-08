namespace UI;

using Godot;
using System;

public partial class MainMenu : Control
{
    public override void _Ready()
    {
        Scene.Unique.PlayButton.Get(this).Pressed += () => Transitions.StartTransition(TransitionType.BlackFade, () =>
        {
            Transitions.EndTransition(TransitionType.BlackFade);
            GetTree().ChangeSceneToFile(Res.Game.Playground_tscn);
        });
    }
}
