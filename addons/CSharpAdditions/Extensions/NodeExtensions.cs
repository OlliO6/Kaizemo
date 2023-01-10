namespace Additions;

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class NodeExtensions
{
    public static void Reparent(this Node from, Node newParent)
    {
        from.GetParent()?.RemoveChild(from);
        newParent.AddChild(from);
    }

    public static void ReparentDeffered(this Node from, Node newParent)
    {
        Callable.From(() => from.Reparent(newParent)).CallDeferred();
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

    /// <summary>
    /// Uses timer node
    /// </summary>
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
