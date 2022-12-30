shader_type canvas_item;

#include "res://Shaders/Includes/constants.gdshaderinc"

uniform vec4 line_color : source_color = vec4(1.0, 1.0, 1.0, 1.0);//color line
uniform float line_scale : hint_range(0, 20) = 1.0;    // thickness of the line

void fragment() 
{
	vec2 size = GAME_PIXEL_SIZE * line_scale;
	
	float outline = texture(SCREEN_TEXTURE, SCREEN_UV + vec2(-size.x, 0)).a;
	outline += texture(SCREEN_TEXTURE, SCREEN_UV + vec2(0, size.y)).a;
	outline += texture(SCREEN_TEXTURE, SCREEN_UV + vec2(size.x, 0)).a;
	outline += texture(SCREEN_TEXTURE, SCREEN_UV + vec2(0, -size.y)).a;
	outline += texture(SCREEN_TEXTURE, SCREEN_UV + vec2(-size.x, size.y)).a;
	outline += texture(SCREEN_TEXTURE, SCREEN_UV + vec2(size.x, size.y)).a;
	outline += texture(SCREEN_TEXTURE, SCREEN_UV + vec2(-size.x, -size.y)).a;
	outline += texture(SCREEN_TEXTURE, SCREEN_UV + vec2(size.x, -size.y)).a;
	outline = min(outline, 1.0);
	
	vec4 color = texture(SCREEN_TEXTURE, SCREEN_UV);
	
	COLOR = mix(color, line_color, outline - color.a);
}
