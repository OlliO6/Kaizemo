#if TOOLS
namespace BetterInspector.Editor;

using System;
using System.Reflection;
using BetterInspector.Utilities;
using Godot;

public class TypedPathsInspectorPlugin : EditorInspectorPlugin
{
    public override bool CanHandle(Godot.Object @object) => true;

    public override bool ParseProperty(Godot.Object @object, int typeArg, string path, int hint, string hintText, int usage)
    {
        string propName = path.GetPropName();

        FieldInfo field = @object.GetInEditorTypeCached()?.GetField(propName, Utilities.InstancePubAndNonPubBindingFlags);

        if (field == null || field.FieldType != typeof(NodePath)) return false;

        var attribute = field.GetCustomAttribute<TypedPathAttribute>();

        if (attribute == null) return false;

        // Make TypedPathPropertyEditor instance with correct type
        AddPropertyEditor(
            property: path,
            editor: new TypedPathPropertyEditor(attribute.type));

        return true;
    }
}

#endif