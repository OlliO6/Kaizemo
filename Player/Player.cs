using System;
using Godot;

public partial class Player : CharacterBody2D
{
    [Export] private NodePath _killArea;

    [Export, ExportGroup("Movement")] public float speed = 300;
    [Export] public float jumpVelocity = -400.0f;
    [Export] public float gravityScale = 1;

    public float gravity;

    public Area2D killArea;

    public override void _Ready()
    {
        killArea = GetNode<Area2D>(_killArea);

        killArea.BodyEntered += (body) =>
        {
            GD.Print("Colllll");
        };

        killArea.BodyShapeEntered += (RID bodyRid, Node2D body, long bodyShapeIndex, long localShapeIndex) =>
        {
            GD.Print("Collllliii");
        };

        // Get the gravity from the project settings to be synced with RigidBody nodes.
        gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle() * gravityScale;
        GD.Print(GetClass());
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
            velocity.y += gravity * (float)delta;

        // Handle Jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
            velocity.y = jumpVelocity;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (direction != Vector2.Zero)
        {
            velocity.x = direction.x * speed;
        }
        else
        {
            velocity.x = Mathf.MoveToward(Velocity.x, 0, speed);
        }

        Velocity = velocity;

        if (MoveAndSlide())
            HandleCollisions();


        void HandleCollisions()
        {
            for (int i = 0; i < GetSlideCollisionCount(); i++)
            {
                var collision = GetSlideCollision(i);

                if (collision.GetCollider() is LevelMap map)
                    map.ProcessCollision(this, collision);
            }
        }
    }

    public void Die()
    {
        GD.Print("Player died.");
    }
}
