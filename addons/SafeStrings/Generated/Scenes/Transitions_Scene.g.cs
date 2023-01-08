partial class Transitions : SafeStrings.IScene<Transitions>
{
    public static Transitions Instantiate()
    {
        var val = SafeStrings.Res.Singletons.Transitions.Transitions_tscn.Value.Instantiate<Transitions>();
        ((SafeStrings.IScene<Transitions>)val).OnInstanced();
        return val;
    }
    public static class Scene
    {
        public static partial class AnimationPlayer
        {
            public static SafeStrings.SceneNodePath<Godot.AnimationPlayer> Path = "./AnimationPlayer";
            public static Godot.AnimationPlayer Get(Godot.Node root) => Path.Get(root);
            public static Godot.AnimationPlayer GetCached(Godot.Node root) => Path.GetCached(root);
        }
        public static partial class BlackScreen
        {
            public static SafeStrings.SceneNodePath<Godot.ColorRect> Path = "./BlackScreen";
            public static Godot.ColorRect Get(Godot.Node root) => Path.Get(root);
            public static Godot.ColorRect GetCached(Godot.Node root) => Path.GetCached(root);
        }
        
        public static class Unique
        {
            
        }
    }
}
