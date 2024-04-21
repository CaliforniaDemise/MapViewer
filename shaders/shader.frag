#version 330 core

in vec3 depth;

out vec4 FragColor;

void main() {
 FragColor = vec4(depth, 1.0);
}