namespace Game;
partial class DiveSpender : SafeStrings.IScene<DiveSpender>
{
    public static DiveSpender Instantiate()
    {
        var val = SafeStrings.Res.game.load_abilities.dive_spender.dive_spender_tscn.Value.Instantiate<DiveSpender>();
        ((SafeStrings.IScene<DiveSpender>)val).OnInstanced();
        return val;
    }
    public static class Scene
    {
        public static partial class CollisionShape2D
        {
            public static SafeStrings.SceneNodePath<Godot.CollisionShape2D> Path = "./CollisionShape2D";
            public static Godot.CollisionShape2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.CollisionShape2D GetCached(Godot.Node root) => Path.GetCached(root);
        }
        public static partial class CanvasGroup
        {
            public static SafeStrings.SceneNodePath<Godot.CanvasGroup> Path = "./CanvasGroup";
            public static Godot.CanvasGroup Get(Godot.Node root) => Path.Get(root);
            public static Godot.CanvasGroup GetCached(Godot.Node root) => Path.GetCached(root);
        }
        partial class CanvasGroup { public static partial class CPUParticles2D
        {
            public static SafeStrings.SceneNodePath<Godot.CPUParticles2D> Path = "./CanvasGroup/CPUParticles2D";
            public static Godot.CPUParticles2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.CPUParticles2D GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        public static partial class DisabledTimer
        {
            public static SafeStrings.SceneNodePath<Godot.Timer> Path = "./DisabledTimer";
            public static Godot.Timer Get(Godot.Node root) => Path.Get(root);
            public static Godot.Timer GetCached(Godot.Node root) => Path.GetCached(root);
        }
        
        public static class Unique
        {
            
        }
    }
}
