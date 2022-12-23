#if TOOLS
namespace ResPicker;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;

[Tool]
public partial class Manager : Node
{
    public Plugin plugin;

    public Manager(Plugin plugin)
    {
        this.plugin = plugin;
    }
    public Manager() { }

    public override void _Ready()
    {
        GetTree().NodeAdded += NodeAdded;
    }

    private void NodeAdded(Node node)
    {
        if (node is not PopupMenu popup)
            return;

        if (node.GetParent() is not EditorResourcePicker picker)
            return;

        var propertyEditor = picker.GetParent() as EditorProperty;

        if (propertyEditor == null || propertyEditor.GetClass() != "EditorPropertyResource")
            return;

        popup.AboutToPopup += () => PickerAboutToPopup(propertyEditor, picker, popup);
    }

    private void PickerAboutToPopup(EditorProperty propertyEditor, EditorResourcePicker picker, PopupMenu menu)
    {
        string propName = ((string)propertyEditor.GetEditedProperty()).GetFile();

        Type desiredType = propertyEditor.GetEditedObject().GetInEditorType().GetFieldInherited(propName)?.FieldType;

        if (desiredType == null)
            desiredType = propertyEditor.GetEditedObject().GetInEditorType().GetPropertyInherited(propName)?.PropertyType;

        if (desiredType == null) return;

        RecreateMenu(propertyEditor, menu, desiredType);

        // Give it the right position and size
        Vector2i rightTop = menu.Position + Vector2i.Right * menu.Size.x;
        menu.Size = new(0, 0);
        menu.Position = rightTop + Vector2i.Left * menu.Size.x;

        menu.IndexPressed += (idx) => OnResPickerItemPressed((int)idx, menu, propertyEditor);

        Texture2D GetIconFrom(Type type)
        {
            Texture2D icon = null;

            while (icon == null && type != null)
            {
                icon = Plugin.GetIcon(type.Name);
                type = type.BaseType;
            }

            return icon;
        }
        Texture2D GetIconFromClassDB(string @class)
        {
            Texture2D icon = null;

            while (icon == null && ClassDB.ClassExists(@class))
            {
                icon = Plugin.GetIcon(@class);
                @class = ClassDB.GetParentClass(@class);
            }

            return icon;
        }

        void RecreateMenu(EditorProperty propertyEditor, PopupMenu menu, Type desiredType)
        {
            bool copyShown = menu.GetItemIndex(6) != -1;
            bool pasteShown = menu.GetItemIndex(7) != -1;
            bool showInFileSystemShown = menu.GetItemIndex(9) != -1;

            menu.Clear();
            AddNewItems(propertyEditor, menu, desiredType);

            menu.AddItem("");
            menu.SetItemAsSeparator(menu.ItemCount - 1, true);

            menu.AddIconItem(Plugin.GetIcon("Load"), "Quick Load", 1);
            menu.AddIconItem(Plugin.GetIcon("Load"), "Load", 0);

            if (propertyEditor.GetEditedObject().Get(propertyEditor.GetEditedProperty()).Obj != null)
            {
                menu.AddIconItem(Plugin.GetIcon("Edit"), "Edit", 2);
                menu.AddIconItem(Plugin.GetIcon("Clear"), "Clear", 3);
                menu.AddIconItem(Plugin.GetIcon("Duplicate"), "Make Unique", 4);
                menu.AddIconItem(Plugin.GetIcon("Save"), "Save", 6);

                if (showInFileSystemShown)
                {
                    menu.AddItem("");
                    menu.SetItemAsSeparator(menu.ItemCount - 1, true);
                    menu.AddIconItem(Plugin.GetIcon("ListSelect"), "Show in Filesystem", 9);
                }
            }
            if (copyShown || pasteShown)
            {
                menu.AddItem("");
                menu.SetItemAsSeparator(menu.ItemCount - 1, true);
            }
            if (copyShown) menu.AddIconItem(Plugin.GetIcon("ActionCopy"), "Copy", 7);
            if (pasteShown) menu.AddIconItem(Plugin.GetIcon("ActionPaste"), "Paste", 8);

            void AddNewItems(EditorProperty propertyEditor, PopupMenu menu, Type desiredType)
            {
                foreach (var type in GetType().Assembly.DefinedTypes
                                    .Where(type => desiredType.IsAssignableFrom(type) && !type.IsAbstract))
                {
                    var pathAttribute = type.GetCustomAttribute<ResourceScriptPathAttribute>();

                    if (pathAttribute == null) continue;

                    propertyEditor.SetMeta("i" + menu.ItemCount.ToString(), pathAttribute.path);
                    menu.AddIconItem(GetIconFrom(type), "New " + type.Name, 100);
                }


                if (ClassDB.ClassExists(desiredType.Name))
                {
                    string className = desiredType.Name;

                    if (ClassDB.CanInstantiate(className))
                    {
                        propertyEditor.SetMeta("i" + menu.ItemCount.ToString(), "#ClassDB");
                        menu.AddIconItem(GetIconFromClassDB(className), "New " + className, 100);
                    }

                    foreach (string @class in ClassDB.GetInheritersFromClass(className)
                            .Where(@class => ClassDB.CanInstantiate(@class)))
                    {
                        propertyEditor.SetMeta("i" + menu.ItemCount.ToString(), "#ClassDB");
                        menu.AddIconItem(GetIconFromClassDB(@class), "New " + @class, 100);
                    }
                }
            }
        }
    }

    private void OnResPickerItemPressed(int idx, PopupMenu menu, EditorProperty property)
    {
        if (menu.GetItemId(idx) != 100)
            return;

        string typeName = menu.GetItemText(idx).TrimPrefix("New ");
        string metaString = (string)property.GetMeta("i" + idx.ToString());

        if (metaString == "#ClassDB")
        {
            property.EmitChanged(property.GetEditedProperty(),
                ClassDB.Instantiate(typeName));
            return;
        }

        if (!metaString.IsAbsolutePath())
            return;

        Resource res = new Resource();
        res.SetScript(GD.Load<Script>(ProjectSettings.LocalizePath(metaString)));
        res.ResourceName = typeName;
        property.EmitChanged(property.GetEditedProperty(), res);
    }
}

#endif