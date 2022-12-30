namespace SafeStrings;

public static class Res
{
    public static readonly ResourcePath<Godot.CSharpScript> LevelMap_cs = "res://LevelMap.cs";
    public static readonly ResourcePath<Godot.CompressedTexture2D> Platform_png = "res://Platform.png";
    public static readonly ResourcePath<Godot.PackedScene> Playground_tscn = "res://Playground.tscn";
    public static class Game
    {
        public static class Common
        {
            public static class Components
            {
                public static class StateMachine
                {
                    public static readonly ResourcePath<Godot.CSharpScript> IState_cs = "res://Game/Common/Components/StateMachine/IState.cs";
                    public static readonly ResourcePath<Godot.CSharpScript> ProcessCallbackDelegate_cs = "res://Game/Common/Components/StateMachine/ProcessCallbackDelegate.cs";
                    public static readonly ResourcePath<Godot.CSharpScript> State_cs = "res://Game/Common/Components/StateMachine/State.cs";
                    public static readonly ResourcePath<Godot.CSharpScript> StateMachine_cs = "res://Game/Common/Components/StateMachine/StateMachine.cs";
                    public static readonly ResourcePath<Godot.PackedScene> StateMachine_tscn = "res://Game/Common/Components/StateMachine/StateMachine.tscn";
                }
            }
            public static class Interfaces
            {
            }
        }
        public static class DiveSpender
        {
            public static readonly ResourcePath<Godot.CSharpScript> DiveSpender_cs = "res://Game/DiveSpender/DiveSpender.cs";
            public static readonly ResourcePath<Godot.PackedScene> DiveSpender_tscn = "res://Game/DiveSpender/DiveSpender.tscn";
        }
        public static class LoadAbilities
        {
            public static readonly ResourcePath<Godot.CSharpScript> ILoadAbilityObtainer_cs = "res://Game/LoadAbilities/ILoadAbilityObtainer.cs";
            public static readonly ResourcePath<Godot.CSharpScript> LoadAbility_cs = "res://Game/LoadAbilities/LoadAbility.cs";
        }
        public static class Player
        {
            public static readonly ResourcePath<Godot.CSharpScript> Player_cs = "res://Game/Player/Player.cs";
            public static readonly ResourcePath<Godot.PackedScene> Player_tscn = "res://Game/Player/Player.tscn";
            public static readonly ResourcePath<Godot.CSharpScript> PlayerAnimationTree_cs = "res://Game/Player/PlayerAnimationTree.cs";
            public static readonly ResourcePath<Godot.CSharpScript> PlayerOld_cs = "res://Game/Player/PlayerOld.cs";
            public static readonly ResourcePath<Godot.CSharpScript> PlayerWeird_cs = "res://Game/Player/PlayerWeird.cs";
            public static readonly ResourcePath<Godot.CompressedTexture2D> player_png = "res://Game/Player/player.png";
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
            public static readonly ResourcePath<Godot.Shader> color_replace_gdshader = "res://Shaders/Common/color_replace.gdshader";
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