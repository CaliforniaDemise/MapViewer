using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MapViewer {
 public class Shader : IDisposable {
  int handle;

  private bool disposedValue = false;

  protected virtual void Dispose(bool disposing) {
   if (!disposedValue) {
    GL.DeleteProgram(handle);
    disposedValue = true;
   }
  }

  public Shader(string vertPath, string fragPath) {
   int vertShader = CompileShader(vertPath, ShaderType.VertexShader);
   int fragShader = CompileShader(fragPath, ShaderType.FragmentShader);

   handle = GL.CreateProgram();
   
   GL.AttachShader(handle, vertShader);
   GL.AttachShader(handle, fragShader);

   GL.LinkProgram(handle);

   GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out int success);
   if (success == 0) {
    string infoLog = GL.GetProgramInfoLog(handle);
    Console.WriteLine(infoLog);
   }
   
   GL.DetachShader(handle, vertShader);
   GL.DetachShader(handle, fragShader);
   GL.DeleteShader(vertShader);
   GL.DeleteShader(fragShader);
  }

  public void Use() {
   GL.UseProgram(handle);
  }

  public void Dispose() {
   Dispose(true);
   GC.SuppressFinalize(this);
  }

  public void SetVec2(string name, Vector2 vector) {
   Use();
   int location = GL.GetUniformLocation(handle, name);
   GL.Uniform2(location, vector);
  }

  public void setVec2(string name, float x, float y) {
   Use();
   int location = GL.GetUniformLocation(handle, name);
   GL.Uniform2(location, x, y);
  }

  public void SetVec3(string name, Vector3 vector) {
   Use();
   int location = GL.GetUniformLocation(handle, name);
   GL.Uniform3(location, vector);
  }

  public void setVec3(string name, float x, float y, float z) {
   Use();
   int location = GL.GetUniformLocation(handle, name);
   GL.Uniform3(location, x, y, z);
  }

  public void SetVec4(string name, Vector4 vector) {
   Use();
   int location = GL.GetUniformLocation(handle, name);
   GL.Uniform4(location, vector);
  }

  public void setVec4(string name, float x, float y, float z, float w) {
   Use();
   int location = GL.GetUniformLocation(handle, name);
   GL.Uniform4(location, x, y, z, w);
  }

  public void SetMatrix4(string name, Matrix4 matrix) {
   Use();
   int location = GL.GetUniformLocation(handle, name);
   GL.UniformMatrix4(location, false, ref matrix);
  }

  private int CompileShader(string shaderPath, ShaderType shaderType) {
   string shaderSource = File.ReadAllText(shaderPath);
   int shader = GL.CreateShader(shaderType);
   GL.ShaderSource(shader, shaderSource);
   GL.CompileShader(shader);
   GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
   if (code != (int) All.True) {
    string infoLog = GL.GetShaderInfoLog(shader);
    throw new Exception($"Error occured whilst compiling Shader({shader}).\n\n{infoLog}");
   }
   return shader;
  }
 }
}