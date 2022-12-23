#if TOOLS
namespace ResPicker;

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Godot;

public static class Utils
{
    public static Type GetInEditorType(this Godot.Object obj)
    {
        Type type = obj.GetType();

        if (type.GetCustomAttribute(typeof(ToolAttribute)) != null)
            return type;

        CSharpScript script = obj.GetScript().Obj as CSharpScript;

        if (script is null) return type;

        return script.GetTypeFromScript();
    }

    public static string GetCsFullNameFromScript(this CSharpScript script)
    {
        GetCsTypeFromScript(script, out string @namespace, out string @class);

        if (@namespace == "")
            return @class;

        return $"{@namespace}.{@class}";
    }

    public static void GetCsTypeFromScript(this CSharpScript script, out string @namespace, out string @class)
    {
        const string Namespace = "namespace ";
        const string ClassBegin = "class ";
        const string Class = " class ";

        StreamReader sourceReader = new(ProjectSettings.GlobalizePath(script.ResourcePath));

        @namespace = "";
        @class = "";

        while (true)
        {
            string line = sourceReader.ReadLine();

            if (line == null)
                break;

            if (line.StartsWith("//"))
                continue;

            if (line.StartsWith(Namespace))
            {
                @namespace = GetNamespace(line);
                continue;
            }

            if (line.StartsWith(ClassBegin))
            {
                @class = GetClass(line, fromBegin: true);
                break;
            }

            if (line.Contains(" class "))
            {
                @class = GetClass(line, fromBegin: false);
                break;
            }
        }

        sourceReader.Close();

        static string GetNamespace(string line)
        {
            string @namespace = line.Substring(Namespace.Length);

            if (@namespace.Contains(' ') || @namespace.Contains(';'))
            {
                int lenght = @namespace.IndexOfAny(new char[] { ' ', ';' });

                @namespace = @namespace.Substring(0, lenght);
            }

            return @namespace;
        }

        static string GetClass(string line, bool fromBegin)
        {
            string @class;

            if (fromBegin)
                @class = line.Substring(ClassBegin.Length);
            else
                @class = line.Substring(line.IndexOf(Class) + Class.Length);

            if (@class.Contains(':'))
                @class = @class.Substring(0, @class.IndexOf(':'));

            return @class.StripEdges();
        }
    }

    public static Type GetTypeFromScript(this CSharpScript script) => Type.GetType(GetCsFullNameFromScript(script));

    public static FieldInfo GetFieldInherited(this Type type, string name)
    {
        FieldInfo field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (field != null)
            return field;

        if (type.BaseType == null)
            return null;

        return type.BaseType.GetField(name);
    }

    public static PropertyInfo GetPropertyInherited(this Type type, string name)
    {
        PropertyInfo field = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (field != null)
            return field;

        if (type.BaseType == null)
            return null;

        return type.BaseType.GetPropertyInherited(name);
    }
}

#endif