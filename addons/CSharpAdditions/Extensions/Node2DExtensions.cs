namespace Additions;

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class Node2DExtensions
{
    public static void ReparentKeepPosition(this Node2D from, Node newParent)
    {
        Vector2 globPos = from.GlobalPosition;

        from.GetParent()?.RemoveChild(from);
        newParent.AddChild(from);

        from.GlobalPosition = globPos;
    }

    public static void ReparentKeepPositionDeffered(this Node2D from, Node newParent)
    {
        Callable.From(() => from.ReparentKeepPosition(newParent)).CallDeferred();
    }

    public static void SetTopLevelKeepPosition(this Node2D from, bool enable)
    {
        var globPos = from.GlobalPosition;
        from.TopLevel = enable;
        from.GlobalPosition = globPos;
    }
}
