#if TOOLS
namespace ResPicker;

using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;

[Tool]
public partial class Plugin : EditorPlugin
{
    private const string RegisterSettingsPath = "ResPicker/do_register_resources";

    public static Plugin Instance { get; private set; }
    public static bool HasInstance => IsInstanceValid(Instance);


    public static bool RegistryEnabled => ProjectSettings.GetSetting(RegisterSettingsPath).AsBool();

    private Manager manager;

    private List<string> registeredResourceTypes = new();

    public override void _EnterTree()
    {
        Instance = this;

        if (!ProjectSettings.HasSetting(RegisterSettingsPath))
        {
            ProjectSettings.SetSetting(RegisterSettingsPath, false);
            ProjectSettings.SetInitialValue(RegisterSettingsPath, false);
        }

        Reset();
    }

    public override void _ExitTree()
    {
        Instance = null;

        ClearResourceTypes();
    }

    public static Texture2D GetIcon(string name)
    {
        if (!HasInstance ||
            !Instance.GetEditorInterface().GetBaseControl().Theme.HasIcon(name, "EditorIcons")) return null;

        return Instance.GetEditorInterface().GetBaseControl().Theme.GetIcon(name, "EditorIcons");
    }

    public override void _Process(double delta)
    {
        if (!HasInstance)
        {
            Instance = this;

            Reset();
        }
    }

    private async void Reset()
    {
        manager?.QueueFree();
        AddChild(manager = new(this));

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        if (RegistryEnabled)
            ResetResourceTypes();

        void ResetResourceTypes()
        {
            ClearResourceTypes();
            AddResourceTypes();
        }
    }

    private void AddResourceTypes()
    {
        foreach (var type in GetType().Assembly.DefinedTypes)
        {
            ResourceScriptPathAttribute resourcePath = type.GetCustomAttribute<ResourceScriptPathAttribute>();

            if (resourcePath == null) continue;

            registeredResourceTypes.Add(type.Name);
            AddCustomType(type.Name, GetBaseType(type), GD.Load<Script>(resourcePath.path), null);
        }

        string GetBaseType(Type type)
        {
            Type baseType = type.BaseType;

            if (ClassDB.ClassExists(baseType.Name))
                return baseType.Name;

            return GetBaseType(baseType);
        }
    }

    private void ClearResourceTypes()
    {
        foreach (var resourceType in new List<string>(registeredResourceTypes))
        {
            registeredResourceTypes.Remove(resourceType);
            RemoveCustomType(resourceType);
        }
    }
}

#endif