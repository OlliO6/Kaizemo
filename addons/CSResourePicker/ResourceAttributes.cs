namespace Godot;

using System;
using System.Diagnostics;
using Godot;

[Conditional("TOOLS")]
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
sealed class ResourceAttribute : Attribute { }

[Conditional("TOOLS")]
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
sealed class ResourceScriptPathAttribute : Attribute
{
    public readonly string path;
    public ResourceScriptPathAttribute(string path) => this.path = path;
}
