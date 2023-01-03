namespace Game.PlayerBehaviour;

using Godot;
using System;

public partial class PlayerCam : Camera2D
{
    [Export] public float playerVelocityInfluence;

    private Player _player;

    public void Init(Player player)
    {
        _player = player;
    }

    public override void _PhysicsProcess(double delta)
    {
        Position = _player?.Position + _player?.Velocity * playerVelocityInfluence ?? Vector2.Zero;
    }
}
