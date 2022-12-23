partial class Player : SafeStrings.IScene<Player>
{
    public static Player Instantiate()
    {
        var val = SafeStrings.Res.Player.player_tscn.Value.Instantiate<Player>();
        ((SafeStrings.IScene<Player>)val).OnInstanced();
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
        public static partial class Sprite
        {
            public static SafeStrings.SceneNodePath<Godot.Sprite2D> Path = "./Sprite";
            public static Godot.Sprite2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.Sprite2D GetCached(Godot.Node root) => Path.GetCached(root);
        }
        public static partial class Camera2D
        {
            public static SafeStrings.SceneNodePath<Godot.Camera2D> Path = "./Camera2D";
            public static Godot.Camera2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.Camera2D GetCached(Godot.Node root) => Path.GetCached(root);
        }
        public static partial class HurtArea
        {
            public static SafeStrings.SceneNodePath<Godot.Area2D> Path = "./HurtArea";
            public static Godot.Area2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.Area2D GetCached(Godot.Node root) => Path.GetCached(root);
        }
        partial class HurtArea { public static partial class CollisionShape2D
        {
            public static SafeStrings.SceneNodePath<Godot.CollisionShape2D> Path = "./HurtArea/CollisionShape2D";
            public static Godot.CollisionShape2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.CollisionShape2D GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        public static partial class AnimationPlayer
        {
            public static SafeStrings.SceneNodePath<Godot.AnimationPlayer> Path = "./AnimationPlayer";
            public static Godot.AnimationPlayer Get(Godot.Node root) => Path.Get(root);
            public static Godot.AnimationPlayer GetCached(Godot.Node root) => Path.GetCached(root);
        }
        public static partial class Sprite2D4
        {
            public static SafeStrings.SceneNodePath<Godot.Sprite2D> Path = "./Sprite2D4";
            public static Godot.Sprite2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.Sprite2D GetCached(Godot.Node root) => Path.GetCached(root);
        }
        public static partial class CanvasGroup
        {
            public static SafeStrings.SceneNodePath<Godot.CanvasGroup> Path = "./CanvasGroup";
            public static Godot.CanvasGroup Get(Godot.Node root) => Path.Get(root);
            public static Godot.CanvasGroup GetCached(Godot.Node root) => Path.GetCached(root);
        }
        partial class CanvasGroup { public static partial class GPUParticles2D
        {
            public static SafeStrings.SceneNodePath<Godot.GPUParticles2D> Path = "./CanvasGroup/GPUParticles2D";
            public static Godot.GPUParticles2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.GPUParticles2D GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        
        public static class Unique
        {
            
        }
    }
}
