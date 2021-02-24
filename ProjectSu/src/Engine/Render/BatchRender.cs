using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using ProjectSu.src.Engine;
using ProjectSu.src.Engine.AssetsPipeline;
using ProjectSu.src.Engine.Render;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.Engine.Render
{
    /// <summary>
    /// BathingRendering, oly for static objects
    /// </summary>
    public class BatchRender : ClassBase
    {
        public List<BatchObject> gameObjectsList = new List<BatchObject>();

        private Mesh meshBatch;

        public string _shader;
        public string _texture;
        private int IBO, VAO, vbo, dbo, tbo, nbo;

        private string spacename;
        private string ModelName;

        public BatchRender(string model,string shader, string texture)
        {
            ModelName = model;

            spacename = "ElbriumSpace";

            _shader = shader;
            _texture = texture;

            meshBatch = new Mesh(null);

            IBO = GL.GenBuffer();
            VAO = GL.GenVertexArray();

            vbo = GL.GenBuffer();
            dbo = GL.GenBuffer();
            tbo = GL.GenBuffer();
            nbo = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, meshBatch._indices.Length * sizeof(int), meshBatch._indices, BufferUsageHint.DynamicDraw);

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, meshBatch._vertices.Length * Vector3.SizeInBytes, meshBatch._vertices, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            //Colors
            GL.BindBuffer(BufferTarget.ArrayBuffer, dbo);
            GL.BufferData(BufferTarget.ArrayBuffer, meshBatch._Colors.Length * sizeof(float), meshBatch._Colors, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);

            //Texture
            GL.BindBuffer(BufferTarget.ArrayBuffer, tbo);
            GL.BufferData(BufferTarget.ArrayBuffer, meshBatch._texCoords.Length * Vector2.SizeInBytes, meshBatch._texCoords, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(2);

            GL.BindBuffer(BufferTarget.ArrayBuffer, nbo);
            GL.BufferData(BufferTarget.ArrayBuffer, meshBatch._Normals.Length * Vector3.SizeInBytes, meshBatch._Normals, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(3);
        }

        public void TickDraw()
        {
            if (gameObjectsList.Count > 0)
            {
                if (Camera.main != null && spacename != string.Empty)
                {
                    if (Ambience.GetEnvironment(spacename) == null) { return; }

                    GL.FrontFace(FrontFaceDirection.Cw);

                    GL.Enable(EnableCap.CullFace);
                    GL.CullFace(CullFaceMode.Front);

                    GL.BindVertexArray(VAO);
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);

                    AssetManager.UseTexture(_texture);

                    AssetManager.UseShader(_shader);

                    AssetManager.ShaderSet(_shader, "world", Matrix4.CreateTranslation(0,0,0));
                    AssetManager.ShaderSet(_shader, "view", Camera.main.GetViewMatrix());
                    AssetManager.ShaderSet(_shader, "projection", Camera.main.GetProjectionMatrix());

                    //Set The Fog Values(this need to be in all mesh with use fog)
                    AssetManager.ShaderSet(_shader, "FOG_Density", Ambience.GetEnvironment(spacename).Density);
                    AssetManager.ShaderSet(_shader, "FOG_Gradiante", Ambience.GetEnvironment(spacename).Distance);
                    AssetManager.ShaderSet(_shader, "FOG_Color", Ambience.GetEnvironment(spacename).FogColor);

                    AssetManager.ShaderSet(_shader, "AmbienceColor", Ambience.GetEnvironment(spacename).AmbienceColor);

                    GL.DrawElements(Debug.GLBeginMode, meshBatch._indices.Length, DrawElementsType.UnsignedInt, 0);
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                    GL.BindVertexArray(0);

                    GL.Disable(EnableCap.CullFace);
                    GL.FrontFace(FrontFaceDirection.Ccw);
                }
            }
        }

        private void RefreshPool()
        {
            List<Vector3> _vertices = new List<Vector3>();
            List<int> _indices = new List<int>();
            List<Vector2> _texCoords = new List<Vector2>();
            List<Color4> _Colors = new List<Color4>();

            Mesh mesh = AssetManager.GetMesh(ModelName);

            foreach (var item in gameObjectsList)
            {
                for (int i = 0; i < mesh._vertices.Length; i++)
                {
                    _vertices.Add(mesh._vertices[i] + (Vector3)item.position);
                }

                _indices.AddRange(mesh._indices);
                _texCoords.AddRange(mesh._texCoords);
                _Colors.AddRange(mesh._Colors);
            }

            meshBatch = new Mesh(_vertices.ToArray(), _texCoords.ToArray(), _Colors.ToArray(), _indices.ToArray());

            UpdateBuffers();
        }

        private void UpdateBuffers()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableVertexAttribArray(0);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableVertexAttribArray(1);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableVertexAttribArray(2);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableVertexAttribArray(3);

            GL.DeleteBuffer(IBO);

            GL.DeleteBuffer(vbo);
            GL.DeleteBuffer(dbo);
            GL.DeleteBuffer(tbo);
            GL.DeleteBuffer(nbo);

            GL.DeleteVertexArray(VAO);

            ////

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, meshBatch._indices.Length * sizeof(int), meshBatch._indices, BufferUsageHint.DynamicDraw);

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, meshBatch._vertices.Length * Vector3.SizeInBytes, meshBatch._vertices, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            //Colors
            GL.BindBuffer(BufferTarget.ArrayBuffer, dbo);
            GL.BufferData(BufferTarget.ArrayBuffer, meshBatch._Colors.Length * sizeof(float), meshBatch._Colors, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);

            //Texture
            GL.BindBuffer(BufferTarget.ArrayBuffer, tbo);
            GL.BufferData(BufferTarget.ArrayBuffer, meshBatch._texCoords.Length * Vector2.SizeInBytes, meshBatch._texCoords, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(2);

            GL.BindBuffer(BufferTarget.ArrayBuffer, nbo);
            GL.BufferData(BufferTarget.ArrayBuffer, meshBatch._Normals.Length * Vector3.SizeInBytes, meshBatch._Normals, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(3);
        }

        public void AddObjectToPool(string spaceName,BatchObject gObject)
        {
            if (!gameObjectsList.Contains(gObject))
            {
                gameObjectsList.Add(gObject);
                RefreshPool();
            }
        }

        public void RemoveObjectToPool(string spaceName,BatchObject gObject)
        {
            if (gameObjectsList.Contains(gObject))
            {
                gameObjectsList.Remove(gObject);
                RefreshPool();
            }
        }

        protected override void OnDispose()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableVertexAttribArray(0);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableVertexAttribArray(1);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableVertexAttribArray(2);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableVertexAttribArray(3);

            GL.DeleteBuffer(IBO);

            GL.DeleteBuffer(vbo);
            GL.DeleteBuffer(dbo);
            GL.DeleteBuffer(tbo);
            GL.DeleteBuffer(nbo);

            GL.DeleteVertexArray(VAO);
            base.OnDispose();
        }
    }

    public struct BatchObject
    {
        public Vector3d position;
        public Quaterniond rotation;
        public Vector3d size;
        public string Model;

        public BatchObject(Vector3d position, Quaterniond rotation, Vector3d size, string model)
        {
            this.position = position;
            this.rotation = rotation;
            this.size = size;
            Model = model;
        }
    }
}
