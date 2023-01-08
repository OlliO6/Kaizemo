namespace SafeStrings;

public static class Res
{
    public static readonly ResourcePath<Godot.AudioBusLayout> DefaultBusLayout_tres = "res://default_bus_layout.tres";
    public static class Game
    {
        public static readonly ResourcePath<Godot.PackedScene> Playground_tscn = "res://game/playground.tscn";
        public static class Ball
        {
            public static readonly ResourcePath<Godot.CSharpScript> Ball_cs = "res://game/ball/Ball.cs";
            public static readonly ResourcePath<Godot.CompressedTexture2D> Ball_png = "res://game/ball/ball.png";
            public static readonly ResourcePath<Godot.PackedScene> Ball_tscn = "res://game/ball/ball.tscn";
        }
        public static class Common
        {
            public static readonly ResourcePath<Godot.CSharpScript> LevelMap_cs = "res://game/common/LevelMap.cs";
            public static class Interfaces
            {
                public static readonly ResourcePath<Godot.CSharpScript> IKillable_cs = "res://game/common/interfaces/IKillable.cs";
                public static readonly ResourcePath<Godot.CSharpScript> ILoadAbilityObtainer_cs = "res://game/common/interfaces/ILoadAbilityObtainer.cs";
                public static readonly ResourcePath<Godot.CSharpScript> IThrowableItem_cs = "res://game/common/interfaces/IThrowableItem.cs";
                public static readonly ResourcePath<Godot.CSharpScript> IThrowableItemHolder_cs = "res://game/common/interfaces/IThrowableItemHolder.cs";
            }
            public static class StateMachine
            {
                public static readonly ResourcePath<Godot.CSharpScript> IState_cs = "res://game/common/state_machine/IState.cs";
                public static readonly ResourcePath<Godot.CSharpScript> PhysicsUtils_cs = "res://game/common/state_machine/PhysicsUtils.cs";
                public static readonly ResourcePath<Godot.CSharpScript> ProcessCallbackDelegate_cs = "res://game/common/state_machine/ProcessCallbackDelegate.cs";
                public static readonly ResourcePath<Godot.CSharpScript> State_cs = "res://game/common/state_machine/State.cs";
                public static readonly ResourcePath<Godot.CSharpScript> StateMachine_cs = "res://game/common/state_machine/StateMachine.cs";
                public static readonly ResourcePath<Godot.PackedScene> StateMachine_tscn = "res://game/common/state_machine/state_machine.tscn";
            }
        }
        public static class LoadAbilities
        {
            public static readonly ResourcePath<Godot.CSharpScript> LoadAbility_cs = "res://game/load_abilities/LoadAbility.cs";
            public static readonly ResourcePath<Godot.CSharpScript> LoadAbilitySpenderPoint_cs = "res://game/load_abilities/LoadAbilitySpenderPoint.cs";
            public static class DiveSpender
            {
                public static readonly ResourcePath<Godot.CSharpScript> DiveSpender_cs = "res://game/load_abilities/dive_spender/DiveSpender.cs";
                public static readonly ResourcePath<Godot.PackedScene> DiveSpender_tscn = "res://game/load_abilities/dive_spender/dive_spender.tscn";
            }
        }
        public static class Player
        {
            public static readonly ResourcePath<Godot.CSharpScript> Player_cs = "res://game/player/Player.cs";
            public static readonly ResourcePath<Godot.CSharpScript> PlayerAnimationTree_cs = "res://game/player/PlayerAnimationTree.cs";
            public static readonly ResourcePath<Godot.CSharpScript> PlayerCam_cs = "res://game/player/PlayerCam.cs";
            public static readonly ResourcePath<Godot.CompressedTexture2D> Player_png = "res://game/player/player.png";
            public static readonly ResourcePath<Godot.PackedScene> Player_tscn = "res://game/player/player.tscn";
            public static class Sounds
            {
                public static readonly ResourcePath<Godot.AudioStreamOggVorbis> Die_ogg = "res://game/player/sounds/die.ogg";
                public static readonly ResourcePath<Godot.AudioStreamOggVorbis> Dive_ogg = "res://game/player/sounds/dive.ogg";
                public static readonly ResourcePath<Godot.AudioStreamOggVorbis> GainDive_ogg = "res://game/player/sounds/gain_dive.ogg";
                public static readonly ResourcePath<Godot.AudioStreamOggVorbis> Jump_ogg = "res://game/player/sounds/jump.ogg";
            }
        }
        public static class Tilesets
        {
            public static readonly ResourcePath<Godot.CompressedTexture2D> GrassPlatform_png = "res://game/tilesets/grass_platform.png";
        }
    }
    public static class Musics
    {
        public static readonly ResourcePath<Godot.AudioStreamOggVorbis> MysticalSong_ogg = "res://musics/mystical_song.ogg";
    }
    public static class Particles
    {
        public static class Dust
        {
            public static readonly ResourcePath<Godot.CompressedTexture2D> DustParticle_png = "res://particles/dust/dust_particle.png";
            public static readonly ResourcePath<Godot.CanvasItemMaterial> DustParticles_material = "res://particles/dust/dust_particles.material";
        }
    }
    public static class Shaders
    {
        public static class Common
        {
            public static readonly ResourcePath<Godot.Shader> CanvasGroupOutline4_gdshader = "res://shaders/common/canvas_group_outline_4.gdshader";
            public static readonly ResourcePath<Godot.Shader> CanvasGroupOutline8_gdshader = "res://shaders/common/canvas_group_outline_8.gdshader";
            public static readonly ResourcePath<Godot.Shader> ColorReplace_gdshader = "res://shaders/common/color_replace.gdshader";
        }
        public static class Includes
        {
            public static readonly ResourcePath<Godot.ShaderInclude> Constants_gdshaderinc = "res://shaders/includes/constants.gdshaderinc";
        }
    }
    public static class Singletons
    {
        public static readonly ResourcePath<Godot.CSharpScript> InputManager_cs = "res://singletons/InputManager.cs";
        public static class Transitions
        {
            public static readonly ResourcePath<Godot.CSharpScript> Transitions_cs = "res://singletons/transitions/Transitions.cs";
            public static readonly ResourcePath<Godot.PackedScene> Transitions_tscn = "res://singletons/transitions/transitions.tscn";
        }
    }
    public static class Ui
    {
        public static class Common
        {
            public static readonly ResourcePath<Godot.Theme> MainTheme_theme = "res://ui/common/main_theme.theme";
            public static class Art
            {
            }
            public static class Fonts
            {
                public static readonly ResourcePath<Godot.FontFile> Merriweather_black_ttf = "res://ui/common/fonts/Merriweather-Black.ttf";
            }
        }
        public static class MainMenu
        {
            public static readonly ResourcePath<Godot.CSharpScript> MainMenu_cs = "res://ui/main_menu/MainMenu.cs";
            public static readonly ResourcePath<Godot.PackedScene> MainMenu_tscn = "res://ui/main_menu/main_menu.tscn";
        }
    }
}