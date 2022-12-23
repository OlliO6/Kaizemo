shader_type canvas_item;

#include "res://Shaders/Includes/constants.gdshaderinc"

uniform bool rainbow  = false; //Activate the rainbow or select you color
uniform vec4 line_color : source_color = vec4(1.0, 1.0, 1.0, 1.0);//color line
uniform float line_scale : hint_range(0, 20) = 1.2;    // thickness of the line
uniform float frequency : hint_range(0.0, 2.0) = 0.5;  // frequency of the rainbow
uniform float light_offset : hint_range(0.00001, 1.0) = 0.5;   // this offsets all color channels;
uniform float alpha : hint_range(0.0, 1.0) = 1.0;

void fragment() 
{
	vec2 size = GAME_PIXEL_SIZE * line_scale;
	
	float outline = texture(SCREEN_TEXTURE, SCREEN_UV + vec2(-size.x, 0)).a;
	outline += texture(SCREEN_TEXTURE, SCREEN_UV + vec2(0, size.y)).a;
	outline += texture(SCREEN_TEXTURE, SCREEN_UV + vec2(size.x, 0)).a;
	outline += texture(SCREEN_TEXTURE, SCREEN_UV + vec2(0, -size.y)).a;
//	outline += texture(SCREEN_TEXTURE, SCREEN_UV + vec2(-size.x, size.y)).a;
//	outline += texture(SCREEN_TEXTURE, SCREEN_UV + vec2(size.x, size.y)).a;
//	outline += texture(SCREEN_TEXTURE, SCREEN_UV + vec2(-size.x, -size.y)).a;
//	outline += texture(SCREEN_TEXTURE, SCREEN_UV + vec2(size.x, -size.y)).a;
	outline = min(outline, 1.0);
	
	vec4 animated_line_color = vec4(light_offset + sin(2.0 * PI * frequency * TIME),
							   light_offset + sin(2.0 * PI * frequency * TIME + radians(180)),
							   light_offset + sin(2.0 * PI * frequency * TIME + radians(180)),
							   alpha);
	
	vec4 color = texture(SCREEN_TEXTURE, SCREEN_UV);
	
	COLOR = mix(color, rainbow ? animated_line_color : line_color, outline - color.a);
}
