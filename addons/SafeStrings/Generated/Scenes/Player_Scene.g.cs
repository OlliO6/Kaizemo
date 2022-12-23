partial class Player : SafeStrings.IScene<Player>
{
    public static Player Instantiate()
    {
        var val = SafeStrings.Res.Game.Player.player_tscn.Value.Instantiate<Player>();
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
        public static partial class AnimationPlayer
        {
            public static SafeStrings.SceneNodePath<Godot.AnimationPlayer> Path = "./AnimationPlayer";
            public static Godot.AnimationPlayer Get(Godot.Node root) => Path.Get(root);
            public static Godot.AnimationPlayer GetCached(Godot.Node root) => Path.GetCached(root);
        }
        public static partial class AnimationTree
        {
            public static SafeStrings.SceneNodePath<Godot.AnimationTree> Path = "./AnimationTree";
            public static Godot.AnimationTree Get(Godot.Node root) => Path.Get(root);
            public static Godot.AnimationTree GetCached(Godot.Node root) => Path.GetCached(root);
        }
        public static partial class DustParticles
        {
            public static SafeStrings.SceneNodePath<Godot.Node2D> Path = "./DustParticles";
            public static Godot.Node2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.Node2D GetCached(Godot.Node root) => Path.GetCached(root);
        }
        partial class DustParticles { public static partial class RunParticles
        {
            public static SafeStrings.SceneNodePath<Godot.GPUParticles2D> Path = "./DustParticles/RunParticles";
            public static Godot.GPUParticles2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.GPUParticles2D GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        partial class DustParticles { public static partial class JumpParticles
        {
            public static SafeStrings.SceneNodePath<Godot.GPUParticles2D> Path = "./DustParticles/JumpParticles";
            public static Godot.GPUParticles2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.GPUParticles2D GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        partial class DustParticles { public static partial class LandParticles
        {
            public static SafeStrings.SceneNodePath<Godot.GPUParticles2D> Path = "./DustParticles/LandParticles";
            public static Godot.GPUParticles2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.GPUParticles2D GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        partial class DustParticles { public static partial class DiveParticles
        {
            public static SafeStrings.SceneNodePath<Godot.GPUParticles2D> Path = "./DustParticles/DiveParticles";
            public static Godot.GPUParticles2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.GPUParticles2D GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        public static partial class Camera2D
        {
            public static SafeStrings.SceneNodePath<Godot.Camera2D> Path = "./Camera2D";
            public static Godot.Camera2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.Camera2D GetCached(Godot.Node root) => Path.GetCached(root);
        }
        
        public static class Unique
        {
            
        }
    }
}
