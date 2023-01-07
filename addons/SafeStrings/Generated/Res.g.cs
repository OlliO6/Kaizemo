namespace SafeStrings;

public static class Res
{
    public static readonly ResourcePath<Godot.AudioBusLayout> default_bus_layout_tres = "res://default_bus_layout.tres";
    public static class game
    {
        public static readonly ResourcePath<Godot.PackedScene> playground_tscn = "res://game/playground.tscn";
        public static class ball
        {
            public static readonly ResourcePath<Godot.CSharpScript> Ball_cs = "res://game/ball/Ball.cs";
            public static readonly ResourcePath<Godot.CompressedTexture2D> ball_png = "res://game/ball/ball.png";
            public static readonly ResourcePath<Godot.PackedScene> ball_tscn = "res://game/ball/ball.tscn";
        }
        public static class common
        {
            public static readonly ResourcePath<Godot.CSharpScript> LevelMap_cs = "res://game/common/LevelMap.cs";
            public static class interfaces
            {
                public static readonly ResourcePath<Godot.CSharpScript> IKillable_cs = "res://game/common/interfaces/IKillable.cs";
                public static readonly ResourcePath<Godot.CSharpScript> ILoadAbilityObtainer_cs = "res://game/common/interfaces/ILoadAbilityObtainer.cs";
                public static readonly ResourcePath<Godot.CSharpScript> IThrowableItem_cs = "res://game/common/interfaces/IThrowableItem.cs";
                public static readonly ResourcePath<Godot.CSharpScript> IThrowableItemHolder_cs = "res://game/common/interfaces/IThrowableItemHolder.cs";
            }
            public static class state_machine
            {
                public static readonly ResourcePath<Godot.CSharpScript> IState_cs = "res://game/common/state_machine/IState.cs";
                public static readonly ResourcePath<Godot.CSharpScript> PhysicsUtils_cs = "res://game/common/state_machine/PhysicsUtils.cs";
                public static readonly ResourcePath<Godot.CSharpScript> ProcessCallbackDelegate_cs = "res://game/common/state_machine/ProcessCallbackDelegate.cs";
                public static readonly ResourcePath<Godot.CSharpScript> State_cs = "res://game/common/state_machine/State.cs";
                public static readonly ResourcePath<Godot.CSharpScript> StateMachine_cs = "res://game/common/state_machine/StateMachine.cs";
                public static readonly ResourcePath<Godot.PackedScene> state_machine_tscn = "res://game/common/state_machine/state_machine.tscn";
            }
        }
        public static class load_abilities
        {
            public static readonly ResourcePath<Godot.CSharpScript> LoadAbility_cs = "res://game/load_abilities/LoadAbility.cs";
            public static readonly ResourcePath<Godot.CSharpScript> LoadAbilitySpenderPoint_cs = "res://game/load_abilities/LoadAbilitySpenderPoint.cs";
            public static class dive_spender
            {
                public static readonly ResourcePath<Godot.CSharpScript> DiveSpender_cs = "res://game/load_abilities/dive_spender/DiveSpender.cs";
                public static readonly ResourcePath<Godot.PackedScene> dive_spender_tscn = "res://game/load_abilities/dive_spender/dive_spender.tscn";
            }
        }
        public static class player
        {
            public static readonly ResourcePath<Godot.CSharpScript> Player_cs = "res://game/player/Player.cs";
            public static readonly ResourcePath<Godot.CSharpScript> PlayerAnimationTree_cs = "res://game/player/PlayerAnimationTree.cs";
            public static readonly ResourcePath<Godot.CSharpScript> PlayerCam_cs = "res://game/player/PlayerCam.cs";
            public static readonly ResourcePath<Godot.CompressedTexture2D> player_png = "res://game/player/player.png";
            public static readonly ResourcePath<Godot.PackedScene> player_tscn = "res://game/player/player.tscn";
            public static class sounds
            {
                public static readonly ResourcePath<Godot.AudioStreamOggVorbis> die_ogg = "res://game/player/sounds/die.ogg";
                public static readonly ResourcePath<Godot.AudioStreamOggVorbis> dive_ogg = "res://game/player/sounds/dive.ogg";
                public static readonly ResourcePath<Godot.AudioStreamOggVorbis> gain_dive_ogg = "res://game/player/sounds/gain_dive.ogg";
                public static readonly ResourcePath<Godot.AudioStreamOggVorbis> jump_ogg = "res://game/player/sounds/jump.ogg";
            }
        }
        public static class tilesets
        {
            public static readonly ResourcePath<Godot.CompressedTexture2D> grass_platform_png = "res://game/tilesets/grass_platform.png";
        }
    }
    public static class musics
    {
        public static readonly ResourcePath<Godot.AudioStreamOggVorbis> mystical_song_ogg = "res://musics/mystical_song.ogg";
    }
    public static class particles
    {
        public static class dust
        {
            public static readonly ResourcePath<Godot.CompressedTexture2D> dust_particle_png = "res://particles/dust/dust_particle.png";
            public static readonly ResourcePath<Godot.CanvasItemMaterial> dust_particles_material = "res://particles/dust/dust_particles.material";
        }
    }
    public static class shaders
    {
        public static class common
        {
            public static readonly ResourcePath<Godot.Shader> canvas_group_outline_4_gdshader = "res://shaders/common/canvas_group_outline_4.gdshader";
            public static readonly ResourcePath<Godot.Shader> canvas_group_outline_8_gdshader = "res://shaders/common/canvas_group_outline_8.gdshader";
            public static readonly ResourcePath<Godot.Shader> color_replace_gdshader = "res://shaders/common/color_replace.gdshader";
        }
        public static class includes
        {
            public static readonly ResourcePath<Godot.ShaderInclude> constants_gdshaderinc = "res://shaders/includes/constants.gdshaderinc";
        }
    }
    public static class singletons
    {
        public static readonly ResourcePath<Godot.CSharpScript> InputManager_cs = "res://singletons/InputManager.cs";
    }
}