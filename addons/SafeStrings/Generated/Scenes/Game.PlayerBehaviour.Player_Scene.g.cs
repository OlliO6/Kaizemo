namespace Game.PlayerBehaviour;
partial class Player : SafeStrings.IScene<Player>
{
    public static Player Instantiate()
    {
        var val = SafeStrings.Res.game.player.player_tscn.Value.Instantiate<Player>();
        ((SafeStrings.IScene<Player>)val).OnInstanced();
        return val;
    }
    public static class Scene
    {
        public static partial class MainStateMachine
        {
            public static SafeStrings.SceneNodePath<global::StateMachines.StateMachine> Path = "./MainStateMachine";
            public static global::StateMachines.StateMachine Get(Godot.Node root) => Path.Get(root);
            public static global::StateMachines.StateMachine GetCached(Godot.Node root) => Path.GetCached(root);
        }
        public static partial class LoadAbilityStateMachine
        {
            public static SafeStrings.SceneNodePath<global::StateMachines.StateMachine> Path = "./LoadAbilityStateMachine";
            public static global::StateMachines.StateMachine Get(Godot.Node root) => Path.Get(root);
            public static global::StateMachines.StateMachine GetCached(Godot.Node root) => Path.GetCached(root);
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
            public static SafeStrings.SceneNodePath<global::Game.PlayerBehaviour.PlayerAnimationTree> Path = "./AnimationTree";
            public static global::Game.PlayerBehaviour.PlayerAnimationTree Get(Godot.Node root) => Path.Get(root);
            public static global::Game.PlayerBehaviour.PlayerAnimationTree GetCached(Godot.Node root) => Path.GetCached(root);
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
        public static partial class PlayerCam
        {
            public static SafeStrings.SceneNodePath<global::Game.PlayerBehaviour.PlayerCam> Path = "./PlayerCam";
            public static global::Game.PlayerBehaviour.PlayerCam Get(Godot.Node root) => Path.Get(root);
            public static global::Game.PlayerBehaviour.PlayerCam GetCached(Godot.Node root) => Path.GetCached(root);
        }
        public static partial class Sounds
        {
            public static SafeStrings.SceneNodePath<Godot.Node2D> Path = "./Sounds";
            public static Godot.Node2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.Node2D GetCached(Godot.Node root) => Path.GetCached(root);
        }
        partial class Sounds { public static partial class Jump
        {
            public static SafeStrings.SceneNodePath<Godot.AudioStreamPlayer2D> Path = "./Sounds/Jump";
            public static Godot.AudioStreamPlayer2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.AudioStreamPlayer2D GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        partial class Sounds { public static partial class Dive
        {
            public static SafeStrings.SceneNodePath<Godot.AudioStreamPlayer2D> Path = "./Sounds/Dive";
            public static Godot.AudioStreamPlayer2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.AudioStreamPlayer2D GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        partial class Sounds { public static partial class GainDive
        {
            public static SafeStrings.SceneNodePath<Godot.AudioStreamPlayer2D> Path = "./Sounds/GainDive";
            public static Godot.AudioStreamPlayer2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.AudioStreamPlayer2D GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        partial class Sounds { public static partial class Die
        {
            public static SafeStrings.SceneNodePath<Godot.AudioStreamPlayer> Path = "./Sounds/Die";
            public static Godot.AudioStreamPlayer Get(Godot.Node root) => Path.Get(root);
            public static Godot.AudioStreamPlayer GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        public static partial class GroundRememberTimer
        {
            public static SafeStrings.SceneNodePath<Godot.Timer> Path = "./GroundRememberTimer";
            public static Godot.Timer Get(Godot.Node root) => Path.Get(root);
            public static Godot.Timer GetCached(Godot.Node root) => Path.GetCached(root);
        }
        public static partial class DiveTimer
        {
            public static SafeStrings.SceneNodePath<Godot.Timer> Path = "./DiveTimer";
            public static Godot.Timer Get(Godot.Node root) => Path.Get(root);
            public static Godot.Timer GetCached(Godot.Node root) => Path.GetCached(root);
        }
        public static partial class UpwardsCornerPushRaycasts
        {
            public static SafeStrings.SceneNodePath<Godot.Node2D> Path = "./UpwardsCornerPushRaycasts";
            public static Godot.Node2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.Node2D GetCached(Godot.Node root) => Path.GetCached(root);
        }
        partial class UpwardsCornerPushRaycasts { public static partial class Left
        {
            public static SafeStrings.SceneNodePath<Godot.RayCast2D> Path = "./UpwardsCornerPushRaycasts/Left";
            public static Godot.RayCast2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.RayCast2D GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        partial class UpwardsCornerPushRaycasts { public static partial class MiddleLeft
        {
            public static SafeStrings.SceneNodePath<Godot.RayCast2D> Path = "./UpwardsCornerPushRaycasts/MiddleLeft";
            public static Godot.RayCast2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.RayCast2D GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        partial class UpwardsCornerPushRaycasts { public static partial class MiddleRight
        {
            public static SafeStrings.SceneNodePath<Godot.RayCast2D> Path = "./UpwardsCornerPushRaycasts/MiddleRight";
            public static Godot.RayCast2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.RayCast2D GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        partial class UpwardsCornerPushRaycasts { public static partial class Right
        {
            public static SafeStrings.SceneNodePath<Godot.RayCast2D> Path = "./UpwardsCornerPushRaycasts/Right";
            public static Godot.RayCast2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.RayCast2D GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        public static partial class SidewardsCornerPushRaycasts
        {
            public static SafeStrings.SceneNodePath<Godot.Node2D> Path = "./SidewardsCornerPushRaycasts";
            public static Godot.Node2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.Node2D GetCached(Godot.Node root) => Path.GetCached(root);
        }
        partial class SidewardsCornerPushRaycasts { public static partial class Bottom
        {
            public static SafeStrings.SceneNodePath<Godot.RayCast2D> Path = "./SidewardsCornerPushRaycasts/Bottom";
            public static Godot.RayCast2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.RayCast2D GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        partial class SidewardsCornerPushRaycasts { public static partial class Middle
        {
            public static SafeStrings.SceneNodePath<Godot.RayCast2D> Path = "./SidewardsCornerPushRaycasts/Middle";
            public static Godot.RayCast2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.RayCast2D GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        
        public static class Unique
        {
            
        }
    }
}
