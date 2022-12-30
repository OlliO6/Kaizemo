namespace Game;

using Godot;
using System;

public partial class DiveSpender : Area2D
{
    public override void _Ready()
    {
        BodyEntered += (body) =>
        {
            if (body is not ILoadAbilityObtainer obtainer)
                return;

            obtainer.ObtainLoadAbility(LoadAbility.Dive);

            Scene.CollisionShape2D.GetCached(this).SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
            Scene.CanvasGroup.GetCached(this).Material.Set("shader_parameter/line_scale", 0);
            Modulate = Colors.Gray;
            Scene.DisabledTimer.GetCached(this).Start();
        };

        Scene.DisabledTimer.GetCached(this).Timeout += () =>
        {
            Scene.CollisionShape2D.GetCached(this).SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
            Scene.CanvasGroup.GetCached(this).Material.Set("shader_parameter/line_scale", 1);
            Modulate = Colors.White;
        };
    }
}
