namespace Additions;

using Godot;
using System;

public static class GDObjectExtensions
{
    public static bool IsValid(this Godot.Object what) => Godot.Object.IsInstanceValid(what);
}
