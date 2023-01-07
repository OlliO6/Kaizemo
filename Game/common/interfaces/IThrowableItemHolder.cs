using Godot;

namespace Game;

public interface IThrowableItemHolder
{
    Vector2 Velocity { get; set; }

    void CollectThrowableItem(IThrowableItem item);

    void ThrowHeldItem(InputManager.ActionDirection direction);

    bool IsHoldingItem();

    IThrowableItem GetHeldItem();
}
