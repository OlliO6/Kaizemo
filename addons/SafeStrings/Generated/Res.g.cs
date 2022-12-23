namespace SafeStrings;

public static class Res
{
    public static readonly ResourcePath<Godot.CSharpScript> LevelMap_cs = "res://LevelMap.cs";
    public static readonly ResourcePath<Godot.CompressedTexture2D> Platform_png = "res://Platform.png";
    public static readonly ResourcePath<Godot.PackedScene> Playground_tscn = "res://Playground.tscn";
    public static class Game
    {
        public static class DiveSpender
        {
        }
        public static class Player
        {
            public static readonly ResourcePath<Godot.CSharpScript> Player_cs = "res://Game/Player/Player.cs";
            public static readonly ResourcePath<Godot.CompressedTexture2D> Player_png = "res://Game/Player/Player.png";
            public static readonly ResourcePath<Godot.PackedScene> player_tscn = "res://Game/Player/player.tscn";
        }
    }
    public static class Particles
    {
        public static class Dust
        {
            public static readonly ResourcePath<Godot.CompressedTexture2D> DustParticle_png = "res://Particles/Dust/DustParticle.png";
            public static readonly ResourcePath<Godot.CanvasItemMaterial> DustParticles_material = "res://Particles/Dust/DustParticles.material";
        }
    }
    public static class Shaders
    {
        public static class Common
        {
            public static readonly ResourcePath<Godot.Shader> canvas_group_outline_4_gdshader = "res://Shaders/Common/canvas_group_outline_4.gdshader";
            public static readonly ResourcePath<Godot.Shader> canvas_group_outline_8_gdshader = "res://Shaders/Common/canvas_group_outline_8.gdshader";
        }
        public static class Includes
        {
            public static readonly ResourcePath<Godot.ShaderInclude> constants_gdshaderinc = "res://Shaders/Includes/constants.gdshaderinc";
        }
    }
    public static class Singletons
    {
        public static readonly ResourcePath<Godot.CSharpScript> InputManager_cs = "res://Singletons/InputManager.cs";
    }
}