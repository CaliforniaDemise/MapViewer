using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using System;

namespace GameObjects {
 public class Player {
  static float speed = 1.5f;
  static float rotSpeed = 90f;

  float x, y, z;
  float pitch, yaw;

  public Player() {
   x = 0; y = 0; z = 0;
   pitch = 0; yaw = 0;
  }

  public void Update(double time, float deltaTime, bool w, bool s, bool a, bool d, float deltaX, float deltaY) {
   HandleMovement(time, deltaTime, w, s, d, a);
   HandleRotation(time, deltaTime, deltaX, deltaY);
  }

  protected void HandleMovement(double time, float deltaTime, bool forward, bool backward, bool right, bool left) {
   if (forward || backward) moveZ(time, deltaTime, backward);
   if (left || right) moveX(time, deltaTime, left);
  }

  private void moveZ(double time, float deltaTime, bool backward) {
   float speed = Player.speed;
   float rot = yaw + 90;
   
   float sin = (float) Math.Sin(MathHelper.DegreesToRadians(rot));
   float cos = (float) Math.Cos(MathHelper.DegreesToRadians(rot));

   if (backward) speed *= -1.0f;

   float speedZ = sin * speed * deltaTime;
   float speedX = cos * speed * deltaTime;

   z += sin * speed * deltaTime;
   x += cos * speed * deltaTime;
  }

  private void moveX(double time, float deltaTime, bool left) {
   float speed = Player.speed;

   float sin = (float) Math.Sin(MathHelper.DegreesToRadians(yaw));
   float cos = (float) Math.Cos(MathHelper.DegreesToRadians(yaw));

   if (left) speed *= -1.0f;

   float speedZ = sin * speed * deltaTime;
   float speedX = cos * speed * deltaTime;

   z += sin * speed * deltaTime;
   x += cos * speed * deltaTime;
  }

  protected void HandleRotation(double time, float deltaTime, float deltaX, float deltaY) {
   yaw += deltaX * rotSpeed * deltaTime;
   if (pitch > 89.0f) pitch = 89.0f;
   else if (pitch < -89.0f) pitch = -89.0f;
   else pitch -= deltaY * rotSpeed * deltaTime;
  }

  // Getters - Setters //
  public float X { get => x; set => x = value; }
  public float Y { get => y; set => y = value; }
  public float Z { get => z; set => z = value; }

  public float Pitch { get => pitch; set => pitch = value; }
  public float Yaw { get => yaw; set => yaw = value; }

  public void setXYZ(float x, float y, float z) {
   this.x = x; this.y = y; this.z = z;
  }

  public void setPY(float pitch, float yaw) {
   this.pitch = pitch; this.yaw = yaw;
  }
 }
}
