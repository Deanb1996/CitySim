#version 330

in vec2 v_TexCoord;
uniform sampler2D s_Texture;

out vec4 Colour;

void main()
{
	Colour = texture2D(s_Texture, v_TexCoord);
}