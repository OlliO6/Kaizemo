namespace Additions;

using Godot;

public static class CharacterBodyExtensions
{
    public static void SetVelocityY(this CharacterBody2D from, float yVelocity)
    {
        Vector2 velocity = from.Velocity;
        velocity.y = yVelocity;
        from.Velocity = velocity;
    }

    public static void SetVelocityX(this CharacterBody2D from, float xVelocity)
    {
        Vector2 velocity = from.Velocity;
        velocity.x = xVelocity;
        from.Velocity = velocity;
    }
}
