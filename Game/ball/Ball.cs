namespace Game;

using PlayerBehaviour;
using Godot;
using System;

public partial class Ball : RigidBody2D, ILoadAbilityObtainer, IThrowableItem
{
    [Export] private float playerJumpVelocity;

    [ExportGroup("Throw")]
    [Export] private float holderVelocityRemain;
    [Export] private Vector2 throwVelocityUp;
    [Export] private Vector2 throwVelocityDown;
    [Export] private Vector2 throwVelocityLeft;
    [Export] private Vector2 throwVelocityRight;

    public event Action LoadedAbilityChanged;

    public IThrowableItemHolder holder;
    public ILoadAbilityObtainer bound;

    public override void _Ready()
    {
        Scene.PickUpArea.Get(this).BodyEntered += PickupAreaBodyEnteredCallback;
    }

    public LoadAbility? GetLoadedAbility() => bound?.GetLoadedAbility();

    public void ObtainLoadAbility(LoadAbility loadAbility) => bound?.ObtainLoadAbility(loadAbility);

    public void PickupAreaBodyEnteredCallback(Node2D body)
    {
        if (body is IThrowableItemHolder holder)
        {
            if (holder.IsHoldingItem())
                return;

            this.holder = holder;
            holder.CollectThrowableItem(this);
            PickUp(holder);
        }
    }

    public async void Throw(InputManager.ActionDirection direction)
    {
        ProcessMode = Node.ProcessModeEnum.Inherit;

        LinearVelocity = holder.Velocity * holderVelocityRemain + direction switch
        {
            InputManager.ActionDirection.Up => throwVelocityUp,
            InputManager.ActionDirection.Down => throwVelocityDown,
            InputManager.ActionDirection.Left => throwVelocityLeft,
            _ => throwVelocityRight
        };

        holder = null;

        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

        Scene.PickUpArea.Get(this).BodyEntered += PickupAreaBodyEnteredCallback;
    }

    public void PickUp(IThrowableItemHolder holder)
    {
        ProcessMode = Node.ProcessModeEnum.Disabled;
        Scene.PickUpArea.Get(this).BodyEntered -= PickupAreaBodyEnteredCallback;

        bound = holder as ILoadAbilityObtainer;

        if (holder is Player player && InputManager.IsHoldingJump && player.MainStateMachine.CurrentState.HasTag(Player.MainStateTag.InAir))
        {
            player.SetVelocityY(playerJumpVelocity);
            player.MainStateMachine.SwitchToState(Player.MainStateId.Jump);
            Scene.PlayerJumpSound.Get(this).Play();
        }
    }

    public IThrowableItemHolder GetHolder() => holder;
}
