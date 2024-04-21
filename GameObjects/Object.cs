using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Globalization;
using System.Text;

namespace GameObjects {
 class Obj {

  int vertexBuffer;
  int elementBuffer;

  float[] verticesArray;
  uint[] indicesArray;

  public float middleX = 0.0f, middleY = 0.0f, middleZ = 0.0f;

  protected Obj(List<Vector3> vertices, List<int> faces) {
   verticesArray = new float[vertices.Count * 3];
   indicesArray = new uint[faces.Count];

   float lowX = 0.0f, lowY = 0.0f, lowZ = 0.0f;
   float highX = 0.0f, highY = 0.0f, highZ = 0.0f;

   for (int i = 0; i < vertices.Count; i++) {
    int value = i * 3;
    if (lowX > vertices[i].X) lowX = vertices[i].X;
    if (lowY > vertices[i].Y) lowY = vertices[i].Y;
    if (lowZ > vertices[i].Z) lowZ = vertices[i].Z;
    if (highX < vertices[i].X) highX = vertices[i].X;
    if (highY < vertices[i].Y) highY = vertices[i].Y;
    if (highZ < vertices[i].Z) highZ = vertices[i].Z;

    verticesArray[value] = vertices[i].X;
    verticesArray[value + 1] = vertices[i].Y;
    verticesArray[value + 2] = vertices[i].Z;
   }

   middleX = (lowX + highX) / 2;
   middleY = (lowY + highY) / 2;
   middleZ = (lowZ + highZ) / 2;

   for (int i = 0; i < faces.Count; i++) {
    indicesArray[i] = (uint) faces[i];
   }

   vertexBuffer = GL.GenBuffer();
   GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
   GL.BufferData(BufferTarget.ArrayBuffer, verticesArray.Length * sizeof(float), verticesArray, BufferUsageHint.StaticDraw);

   elementBuffer = GL.GenBuffer();
   GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBuffer);
   GL.BufferData(BufferTarget.ElementArrayBuffer, indicesArray.Length * sizeof(uint), indicesArray, BufferUsageHint.StaticDraw);
  }

  public void Draw() {
   GL.DrawElements(PrimitiveType.Triangles, indicesArray.Length, DrawElementsType.UnsignedInt, 0);
  }

  public void Use() {
   GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
   GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBuffer);

   GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
   GL.EnableVertexAttribArray(0);
  }

  public void Unload() {
   GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
   GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
   GL.DeleteBuffer(vertexBuffer);
   GL.DeleteBuffer(elementBuffer);
  }

  public static Obj FromFile(string path, int bufferSize) {
   List<Vector3> vertices = new List<Vector3>();
   List<int> faces = new List<int>();

   using (var fileStream = File.OpenRead(path))  {
    using(var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, bufferSize)) {
     string line;
     while ((line = streamReader.ReadLine()) != null) {
      if (string.IsNullOrEmpty(line) || line.StartsWith("#")) continue;
      string[] split = Split(line);
      if (split[0] == "v") {
       float x = float.Parse(split[1]);
       float y = float.Parse(split[2]);
       float z = float.Parse(split[3]);

       Vector3 vec = new Vector3(x, y, z);
       vertices.Add(vec);
      }
      if (split[0] == "f") {
       for (int i = 1; i < split.Length; i++) {
        string[] faceSplit = split[i].Split('/');
        int val = int.Parse(faceSplit[0]);
        faces.Add(val - 1);
       }
      }
     }

     return new Obj(vertices, faces);
    }
   }  
  }

  private static string[] Split(string str) {
   List<string> list = new List<string>();
   StringBuilder builder = new StringBuilder();
   
   for (int i = 0; i <= str.Length; i++) {
    if (i == str.Length && builder.Length > 0) {
     list.Add(builder.ToString());
     return list.ToArray();
    }

    char c = str[i];
    if (c == ' ' && builder.Length > 0) {
     list.Add(builder.ToString());
     builder = new StringBuilder();
     continue;
    }

    if (c != ' ') builder.Append(c);
   }

   return list.ToArray();
  }
 }
}