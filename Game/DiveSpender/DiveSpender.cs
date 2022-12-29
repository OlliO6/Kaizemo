namespace Game;

using Godot;
using System;

public partial class DiveSpender : Area2D
{
    public override void _Ready()
    {
        BodyEntered += (body) =>
        {

        };
    }
}
