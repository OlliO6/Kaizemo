namespace UI;
partial class MainMenu : SafeStrings.IScene<MainMenu>
{
    public static MainMenu Instantiate()
    {
        var val = SafeStrings.Res.Ui.MainMenu.MainMenu_tscn.Value.Instantiate<MainMenu>();
        ((SafeStrings.IScene<MainMenu>)val).OnInstanced();
        return val;
    }
    public static class Scene
    {
        public static partial class Title
        {
            public static SafeStrings.SceneNodePath<Godot.Label> Path = "./Title";
            public static Godot.Label Get(Godot.Node root) => Path.Get(root);
            public static Godot.Label GetCached(Godot.Node root) => Path.GetCached(root);
        }
        public static partial class VBoxContainer
        {
            public static SafeStrings.SceneNodePath<Godot.VBoxContainer> Path = "./VBoxContainer";
            public static Godot.VBoxContainer Get(Godot.Node root) => Path.Get(root);
            public static Godot.VBoxContainer GetCached(Godot.Node root) => Path.GetCached(root);
        }
        partial class VBoxContainer { public static partial class PlayButton
        {
            public static SafeStrings.SceneNodePath<Godot.Button> Path = "./VBoxContainer/PlayButton";
            public static Godot.Button Get(Godot.Node root) => Path.Get(root);
            public static Godot.Button GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        partial class VBoxContainer { public static partial class OptionsButton
        {
            public static SafeStrings.SceneNodePath<Godot.Button> Path = "./VBoxContainer/OptionsButton";
            public static Godot.Button Get(Godot.Node root) => Path.Get(root);
            public static Godot.Button GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        public static partial class FullscreenExplanation
        {
            public static SafeStrings.SceneNodePath<Godot.Label> Path = "./FullscreenExplanation";
            public static Godot.Label Get(Godot.Node root) => Path.Get(root);
            public static Godot.Label GetCached(Godot.Node root) => Path.GetCached(root);
        }
        
        public static class Unique
        {
            public static class PlayButton
            {
                public static SafeStrings.SceneNodePath<Godot.Button> Path = VBoxContainer.PlayButton.Path;
                public static Godot.Button Get(Godot.Node root) => Path.Get(root);
                public static Godot.Button GetCached(Godot.Node root) => Path.GetCached(root);
            }
            public static class OptionsButton
            {
                public static SafeStrings.SceneNodePath<Godot.Button> Path = VBoxContainer.OptionsButton.Path;
                public static Godot.Button Get(Godot.Node root) => Path.Get(root);
                public static Godot.Button GetCached(Godot.Node root) => Path.GetCached(root);
            }
            
        }
    }
}
