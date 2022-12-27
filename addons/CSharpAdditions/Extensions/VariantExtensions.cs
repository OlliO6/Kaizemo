namespace Additions;

using Godot;
using System;

public static class VariantExtensions
{
    public static bool IsNull(this Variant from) => from.Obj == null;
}
