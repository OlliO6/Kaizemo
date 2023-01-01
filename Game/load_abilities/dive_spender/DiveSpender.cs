namespace Game;

using Godot;
using System;

public partial class DiveSpender : LoadAbilitySpenderPoint
{
    protected override bool HandleObtainer(ILoadAbilityObtainer obtainer) => obtainer.GetLoadedAbility() is not LoadAbility.Dive;

    protected override void GiveAbility(ILoadAbilityObtainer obtainer) => obtainer.ObtainLoadAbility(LoadAbility.Dive);

    protected override void Disable()
    {
        Modulate = new Color(Colors.Gray, 0.5f);
        Scene.CanvasGroup.Get(this).Material.Set("shader_parameter/line_scale", 0f);
    }

    protected override void Enable()
    {
        Modulate = Colors.White;
        Scene.CanvasGroup.Get(this).Material.Set("shader_parameter/line_scale", 1f);
    }
}
