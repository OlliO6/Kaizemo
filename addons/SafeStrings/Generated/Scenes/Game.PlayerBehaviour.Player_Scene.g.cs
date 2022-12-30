namespace Game.PlayerBehaviour;
partial class Player : SafeStrings.IScene<Player>
{
    public static Player Instantiate()
    {
        var val = SafeStrings.Res.Game.Player.Player_tscn.Value.Instantiate<Player>();
        ((SafeStrings.IScene<Player>)val).OnInstanced();
        return val;
    }
    public static class Scene
    {
        public static partial class MainStateMachine
        {
            public static SafeStrings.SceneNodePath<StateMachines.StateMachine> Path = "./MainStateMachine";
            public static StateMachines.StateMachine Get(Godot.Node root) => Path.Get(root);
            public static StateMachines.StateMachine GetCached(Godot.Node root) => Path.GetCached(root);
        }
        public static partial class LoadAbilityStateMachine
        {
            public static SafeStrings.SceneNodePath<StateMachines.StateMachine> Path = "./LoadAbilityStateMachine";
            public static StateMachines.StateMachine Get(Godot.Node root) => Path.Get(root);
            public static StateMachines.StateMachine GetCached(Godot.Node root) => Path.GetCached(root);
        }
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
            public static SafeStrings.SceneNodePath<Game.PlayerBehaviour.PlayerAnimationTree> Path = "./AnimationTree";
            public static Game.PlayerBehaviour.PlayerAnimationTree Get(Godot.Node root) => Path.Get(root);
            public static Game.PlayerBehaviour.PlayerAnimationTree GetCached(Godot.Node root) => Path.GetCached(root);
        }
        public static partial class DustParticles
        {
            public static SafeStrings.SceneNodePath<Godot.CanvasGroup> Path = "./DustParticles";
            public static Godot.CanvasGroup Get(Godot.Node root) => Path.Get(root);
            public static Godot.CanvasGroup GetCached(Godot.Node root) => Path.GetCached(root);
        }
        partial class DustParticles { public static partial class RunParticles
        {
            public static SafeStrings.SceneNodePath<Godot.CPUParticles2D> Path = "./DustParticles/RunParticles";
            public static Godot.CPUParticles2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.CPUParticles2D GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        partial class DustParticles { public static partial class JumpParticles
        {
            public static SafeStrings.SceneNodePath<Godot.CPUParticles2D> Path = "./DustParticles/JumpParticles";
            public static Godot.CPUParticles2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.CPUParticles2D GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        partial class DustParticles { public static partial class LandParticles
        {
            public static SafeStrings.SceneNodePath<Godot.CPUParticles2D> Path = "./DustParticles/LandParticles";
            public static Godot.CPUParticles2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.CPUParticles2D GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        partial class DustParticles { public static partial class DiveParticles
        {
            public static SafeStrings.SceneNodePath<Godot.CPUParticles2D> Path = "./DustParticles/DiveParticles";
            public static Godot.CPUParticles2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.CPUParticles2D GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        public static partial class Camera2D
        {
            public static SafeStrings.SceneNodePath<Godot.Camera2D> Path = "./Camera2D";
            public static Godot.Camera2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.Camera2D GetCached(Godot.Node root) => Path.GetCached(root);
        }
        public static partial class StateLabel
        {
            public static SafeStrings.SceneNodePath<Godot.Label> Path = "./StateLabel";
            public static Godot.Label Get(Godot.Node root) => Path.Get(root);
            public static Godot.Label GetCached(Godot.Node root) => Path.GetCached(root);
        }
        public static partial class GroundRememberTimer
        {
            public static SafeStrings.SceneNodePath<Godot.Timer> Path = "./GroundRememberTimer";
            public static Godot.Timer Get(Godot.Node root) => Path.Get(root);
            public static Godot.Timer GetCached(Godot.Node root) => Path.GetCached(root);
        }
        
        public static class Unique
        {
            
        }
    }
}
