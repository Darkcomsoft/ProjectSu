using OpenTK;
using OpenTK.Graphics.OpenGL4;
using ProjectSu.src.Engine.AssetsPipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.Engine
{
    public class SkySphere : ClassBase
    {
        public Mesh _mesh;
        public string _shader;
        private int IBO, VAO, vbo, tbo;

        private Ambience ambience;

        public SkySphere(Ambience ambience, string shader)
        {
            this.ambience = ambience;

            _mesh = AssetManager.GetMesh("skysphere");

            _shader = shader;

            VAO = GL.GenVertexArray();
            IBO = GL.GenBuffer();
            vbo = GL.GenBuffer();
            tbo = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _mesh._indices.Length * sizeof(int), _mesh._indices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _mesh._vertices.Length * Vector3.SizeInBytes, _mesh._vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            //Texture
            GL.BindBuffer(BufferTarget.ArrayBuffer, tbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _mesh._texCoords.Length * Vector2.SizeInBytes, _mesh._texCoords, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);

            TickManager.OnDraw += TickDraw;
        }

        public void TickDraw()
        {
            if (_shader != null && Camera.main != null)
            {
                GL.CullFace(CullFaceMode.Front);

                GL.Enable(EnableCap.CullFace);
                GL.Enable(EnableCap.TextureCubeMap);
                GL.Disable(EnableCap.DepthTest);

                GL.BindVertexArray(VAO);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);

                AssetManager.UseShader(_shader);

                AssetManager.ShaderSet(_shader, "world", Matrix4.CreateScale(50, 50, 50));
                AssetManager.ShaderSet(_shader, "view", Camera.main.GetViewMatrix());
                AssetManager.ShaderSet(_shader, "projection", Camera.main.GetProjectionMatrix());

                AssetManager.ShaderSet(_shader, "SKY_Color", ambience.SkyColor);
                AssetManager.ShaderSet(_shader, "SKY_HoriColor", ambience.SkyHorizonColor);

                GL.DrawElements(BeginMode.Triangles, _mesh._indices.Length, DrawElementsType.UnsignedInt, 0);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                GL.BindVertexArray(0);

                GL.Enable(EnableCap.DepthTest);
                GL.Disable(EnableCap.TextureCubeMap);
                GL.Disable(EnableCap.CullFace);
            }
        }

        protected override void OnDispose()
        {
            TickManager.OnDraw -= TickDraw;

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableVertexAttribArray(0);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableVertexAttribArray(1);

            GL.DeleteBuffer(IBO);

            GL.DeleteBuffer(vbo);
            GL.DeleteBuffer(tbo);

            GL.DeleteVertexArray(VAO);
            base.OnDispose();
        }
    }
}
