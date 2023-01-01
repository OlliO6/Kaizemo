namespace Game;

using Godot;
using System;

public abstract partial class LoadAbilitySpenderPoint : Area2D
{
    [Export] public float disabledTime;

    public Timer DisabledTimer { get; private set; }

    public bool Disabled { get; private set; }

    public ILoadAbilityObtainer LatestObtainerInArea { get; private set; }

    protected abstract bool HandleObtainer(ILoadAbilityObtainer obtainer);

    protected abstract void GiveAbility(ILoadAbilityObtainer obtainer);

    protected abstract void Disable();

    protected abstract void Enable();

    public override void _Ready()
    {
        DisabledTimer = new()
        {
            OneShot = true,
        };

        AddChild(DisabledTimer);

        BodyEntered += (body) =>
        {
            if (body is not ILoadAbilityObtainer obtainer)
                return;

            if (LatestObtainerInArea != null)
                LatestObtainerInArea.LoadedAbilityChanged -= InAreaAbilityChangedCallback;

            LatestObtainerInArea = obtainer;

            if (Disabled)
                return;

            UseIfObtainerIsHandeled(obtainer);

            LatestObtainerInArea.LoadedAbilityChanged += InAreaAbilityChangedCallback;
        };

        BodyExited += (body) =>
        {
            if (body == LatestObtainerInArea)
            {
                LatestObtainerInArea.LoadedAbilityChanged -= InAreaAbilityChangedCallback;
                LatestObtainerInArea = null;
            }
        };

        DisabledTimer.Timeout += () =>
        {
            Enable();
            Disabled = false;

            if (LatestObtainerInArea != null)
            {
                UseIfObtainerIsHandeled(LatestObtainerInArea);
            }
        };
    }

    private void UseIfObtainerIsHandeled(ILoadAbilityObtainer obtainer)
    {
        if (HandleObtainer(obtainer))
        {
            Disable();
            Disabled = true;
            DisabledTimer.Start(disabledTime);
            GiveAbility(obtainer);
        }
    }

    private void InAreaAbilityChangedCallback()
    {
        if (Disabled)
            return;

        UseIfObtainerIsHandeled(LatestObtainerInArea);
    }
}
