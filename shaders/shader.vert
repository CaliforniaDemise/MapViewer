#version 330 core
layout (location = 0) in vec3 aPos;

uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;

out vec2 oTex;
out vec3 depth;

void main() {
 gl_Position = projection * view * model * vec4(aPos, 1.0);
 depth = aPos;
}