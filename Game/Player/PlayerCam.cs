namespace Game.PlayerBehaviour;

using Godot;
using System;

public partial class PlayerCam : Camera2D
{
    [Export] public Vector2 playerVelocityInfluence;
    [Export] public Vector2 playerVelocitySmoothing;

    private Vector2 _smoothedPlayerVelocity = Vector2.Zero;

    private Player _player;

    public void Init(Player player)
    {
        _player = player;
        this.SetTopLevelKeepPosition(true);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_player == null)
            return;

        SmoothVelocity();

        GlobalPosition = _player.GlobalPosition + _smoothedPlayerVelocity * playerVelocityInfluence;

        void SmoothVelocity()
        {
            _smoothedPlayerVelocity = _smoothedPlayerVelocity.Lerp(_player.Velocity, ((float)delta * (Vector2.One / playerVelocitySmoothing)).Clamp(Vector2.Zero, Vector2.One));
        }
    }
}
