namespace Additions;

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class SceneTreeExtensions
{
    public static async Task ToProcessFrame(this SceneTree from)
    {
        await from.ToSignal(from, SceneTree.SignalName.ProcessFrame);
    }

    public static async Task ToPhysicsFrame(this SceneTree from)
    {
        await from.ToSignal(from, SceneTree.SignalName.PhysicsFrame);
    }
}
