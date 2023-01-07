namespace Game;

using PlayerBehaviour;
using Godot;
using System;

public partial class Ball : CharacterBody2D, ILoadAbilityObtainer, IThrowableItem
{
    [Export] public float playerJumpVelocity;

    [ExportGroup("Physics")]
    [Export] public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
    [Export(PropertyHint.Range, "0,1")] public float groundDamping;
    [Export(PropertyHint.Range, "0,1")] public float airDamping;

    [ExportGroup("Throw")]
    [Export] public Vector2 holderVelocityRemain;
    [Export] public Vector2 throwVelocityUp;
    [Export] public Vector2 throwVelocityDown;
    [Export] public Vector2 throwVelocityLeft;
    [Export] public Vector2 throwVelocityRight;

    public event Action LoadedAbilityChanged;

    public IThrowableItemHolder holder;
    public ILoadAbilityObtainer bound;

    public override void _Ready()
    {
        Scene.PickUpArea.Get(this).BodyEntered += PickupAreaBodyEnteredCallback;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        velocity.y += gravity * (float)delta;
        velocity.x = velocity.x.Damped(IsOnFloor() ? groundDamping : airDamping, delta);
        velocity.y = velocity.y.Damped(IsOnFloor() ? groundDamping : airDamping, delta);

        Velocity = velocity;
        MoveAndSlide();
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

        Velocity = holder.Velocity * holderVelocityRemain + direction switch
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

        Velocity = Vector2.Zero;

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
