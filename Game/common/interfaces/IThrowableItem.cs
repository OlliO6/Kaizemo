namespace Game;

using Godot;

public interface IThrowableItem
{
    void Throw(InputManager.ActionDirection direction);

    void PickUp(IThrowableItemHolder holder);

    IThrowableItemHolder GetHolder();

    Node2D AsNode2D() => this as Node2D;
}
