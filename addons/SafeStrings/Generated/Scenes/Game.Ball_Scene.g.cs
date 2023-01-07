namespace Game;
partial class Ball : SafeStrings.IScene<Ball>
{
    public static Ball Instantiate()
    {
        var val = SafeStrings.Res.game.ball.ball_tscn.Value.Instantiate<Ball>();
        ((SafeStrings.IScene<Ball>)val).OnInstanced();
        return val;
    }
    public static class Scene
    {
        public static partial class CollisionPolygon2D
        {
            public static SafeStrings.SceneNodePath<Godot.CollisionPolygon2D> Path = "./CollisionPolygon2D";
            public static Godot.CollisionPolygon2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.CollisionPolygon2D GetCached(Godot.Node root) => Path.GetCached(root);
        }
        public static partial class Ball
        {
            public static SafeStrings.SceneNodePath<Godot.Sprite2D> Path = "./Ball";
            public static Godot.Sprite2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.Sprite2D GetCached(Godot.Node root) => Path.GetCached(root);
        }
        public static partial class PickUpArea
        {
            public static SafeStrings.SceneNodePath<Godot.Area2D> Path = "./PickUpArea";
            public static Godot.Area2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.Area2D GetCached(Godot.Node root) => Path.GetCached(root);
        }
        partial class PickUpArea { public static partial class CollisionPolygon2D
        {
            public static SafeStrings.SceneNodePath<Godot.CollisionPolygon2D> Path = "./PickUpArea/CollisionPolygon2D";
            public static Godot.CollisionPolygon2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.CollisionPolygon2D GetCached(Godot.Node root) => Path.GetCached(root);
        }}
        public static partial class PlayerJumpSound
        {
            public static SafeStrings.SceneNodePath<Godot.AudioStreamPlayer2D> Path = "./PlayerJumpSound";
            public static Godot.AudioStreamPlayer2D Get(Godot.Node root) => Path.Get(root);
            public static Godot.AudioStreamPlayer2D GetCached(Godot.Node root) => Path.GetCached(root);
        }
        
        public static class Unique
        {
            
        }
    }
}
