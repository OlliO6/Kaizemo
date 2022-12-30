shader_type canvas_item;

uniform vec4 oldReplace : source_color;
uniform vec4 newColor : source_color;
uniform bool apply;

void fragment()
{
	vec4 color = texture(TEXTURE, UV);
	
	if (color == oldReplace && apply)
	{
		color = newColor;
	}
	
	COLOR = color;
}
