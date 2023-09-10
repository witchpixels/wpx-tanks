shader_type spatial;

uniform float color_intensity = 0.9;
instance uniform vec3 tank_color: source_color = vec3(1., 0., 0.);

void fragment() {
	ALBEDO = tank_color.rgb;
	ALPHA = COLOR.r * color_intensity;
}
