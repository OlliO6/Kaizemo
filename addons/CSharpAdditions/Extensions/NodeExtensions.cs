namespace Additions;

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class NodeExtensions
{
    public static async Task ProcessFrame(this Node from)
    {
        await from.ToSignal(from.GetTree(), SceneTree.SignalName.ProcessFrame);
    }

    public static async Task PhysicsFrame(this Node from)
    {
        await from.ToSignal(from.GetTree(), SceneTree.SignalName.PhysicsFrame);
    }

    /// <summary>
    /// Gets the children from this object with an specific type
    /// </summary>
    public static IEnumerable<T> GetChildren<T>(this Node from) where T : class
    {
        return from.GetChildren().OfType<T>();
    }

    public static T GetFirstChildOfType<T>(this Node from) where T : class
    {
        for (int i = 0; i < from.GetChildCount(); i++)
        {
            if (from.GetChild(i) is T result)
                return result;
        }
        return null;
    }

    public static List<Node> GetAllChildrenRecursive(this Node from)
    {
        List<Node> result = new();

        for (int i = 0; i < from.GetChildCount(); i++)
        {
            Node child = from.GetChild(i);
            result.Add(child);
            result.AddRange(child.GetAllChildrenRecursive());
        }

        return result;
    }

    public static async Task ToTime(this Node from, float time)
    {
        var timer = new Timer();
        from.AddChild(timer);
        timer.Start(time);
        await timer.ToTimeout();
        from.RemoveChild(timer);
        timer.QueueFree();
    }
}
