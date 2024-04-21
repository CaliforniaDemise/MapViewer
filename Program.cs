using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GameObjects;

namespace MapViewer {
 public class Game : GameWindow {

  Shader shader;

  Player player = new Player();
  double time = 0.0d;

  Matrix4 projection, view, model;

  Vector2 lastMousePos;
  bool firstMove = true;

  int vertexArray;

  Obj cube;

  public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title }) {
   shader = new Shader("./shaders/shader.vert", "./shaders/shader.frag");
  }
  
  public static void Main(string[] args) {
   using (Game game = new Game(800, 600, "MapViewer")) { game.Run(); }
  }

  protected override void OnLoad() {
   base.OnLoad();

   CursorState = CursorState.Grabbed;
   
   GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

   vertexArray = GL.GenVertexArray();
   GL.BindVertexArray(vertexArray);

   cube = Obj.FromFile("./resources/cube.obj", 32);

   projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), Size.X / Size.Y, 0.1f, 100.0f);

   shader.Use();
   shader.SetMatrix4("projection", projection);

   GL.Enable(EnableCap.DepthTest);

   cube.Use();
  }

  protected override void OnUpdateFrame(FrameEventArgs e) {
   base.OnUpdateFrame(e);

   if (CursorState == CursorState.Grabbed) {
    if (firstMove) {
     lastMousePos = MousePosition;
     firstMove = false;  
    }
    else {
     float deltaX = MousePosition.X - lastMousePos.X;
     float deltaY = MousePosition.Y - lastMousePos.Y;
     lastMousePos = MousePosition;
     player.Update(time, (float) e.Time, KeyboardState.IsKeyDown(Keys.W), KeyboardState.IsKeyDown(Keys.S), KeyboardState.IsKeyDown(Keys.D), KeyboardState.IsKeyDown(Keys.A), deltaX, deltaY);
    }
   }

   if (KeyboardState.IsKeyPressed(Keys.LeftControl)) {
    if (CursorState == CursorState.Grabbed) CursorState = CursorState.Normal;
    else CursorState = CursorState.Grabbed;
   }

   if (KeyboardState.IsKeyDown(Keys.Escape)) Close();
  }

  protected override void OnRenderFrame(FrameEventArgs e) {
   base.OnRenderFrame(e);

   time += e.Time * 50;

   cube.Use();
   shader.Use();

   model = Matrix4.CreateTranslation(-cube.middleX, -cube.middleY, -cube.middleZ);
   model = Matrix4.Mult(model, Matrix4.CreateRotationY((float) MathHelper.DegreesToRadians(time)));
   view = Matrix4.CreateTranslation(player.X, player.Y, player.Z + -3.0f);
   view = Matrix4.Mult(view, Matrix4.CreateRotationY(MathHelper.DegreesToRadians(player.Yaw)));
   view = Matrix4.Mult(view, Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-player.Pitch)));

   shader.SetMatrix4("model", model);
   shader.SetMatrix4("view", view);

   GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

   cube.Draw();

   SwapBuffers();
  }

  protected override void OnUnload() {
   cube.Unload();
   shader.Dispose();
  }

  protected override void OnFramebufferResize(FramebufferResizeEventArgs e) {
   base.OnFramebufferResize(e);
   GL.Viewport(0, 0, e.Width, e.Height);
  }
 }
}