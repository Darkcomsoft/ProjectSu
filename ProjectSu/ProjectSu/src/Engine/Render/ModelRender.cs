using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using ProjectSu.src.Engine;
using ProjectSu.src.Engine.AssetsPipeline;
using ProjectSu.src.Engine.Render;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.Engine.Render
{

    public class ModelRender : ClassBase
    {
        public Mesh _mesh;
        public string _shader;
        public string _texture;
        private int IBO, VAO, vbo, dbo, tbo, nbo;

        public CullFaceMode _cullType;
        public bool Transparency = false;

        public ModelRender(Mesh mesh, string shader, string texture)
        {
            _cullType = CullFaceMode.Front;

            _mesh = mesh;
            _shader = shader;
            _texture = texture;

            /*if (_shader != null)
            {
                _shader.Use();
            }*/

            IBO = GL.GenBuffer();
            VAO = GL.GenVertexArray();

            vbo = GL.GenBuffer();
            dbo = GL.GenBuffer();
            tbo = GL.GenBuffer();
            nbo = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _mesh._indices.Length * sizeof(int), _mesh._indices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _mesh._vertices.Length * Vector3.SizeInBytes, _mesh._vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            //Colors
            GL.BindBuffer(BufferTarget.ArrayBuffer, dbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _mesh._Colors.Length * sizeof(float), _mesh._Colors, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);

            //Texture
            GL.BindBuffer(BufferTarget.ArrayBuffer, tbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _mesh._texCoords.Length * Vector2.SizeInBytes, _mesh._texCoords, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(2);

            GL.BindBuffer(BufferTarget.ArrayBuffer, nbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _mesh._Normals.Length * Vector3.SizeInBytes, _mesh._Normals, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(3);
        }

        public void DrawModel(GameObject obj)
        {
            if (_shader != null && Camera.main != null)
            {
                if (Transparency)
                {
                    GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                    GL.Enable(EnableCap.Blend);
                }

                if (_cullType != CullFaceMode.FrontAndBack)
                {
                    GL.CullFace(_cullType);
                    GL.Enable(EnableCap.CullFace);
                }

                GL.BindVertexArray(VAO);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);

                AssetManager.UseTexture(_texture);

                AssetManager.UseShader(_shader);

                AssetManager.ShaderSet(_shader, "world", obj.transform.GetTransformWorld);
                AssetManager.ShaderSet(_shader, "view", Camera.main.GetViewMatrix());
                AssetManager.ShaderSet(_shader, "projection", Camera.main.GetProjectionMatrix());

                //Set The Fog Values(this need to be in all mesh with use fog)
                AssetManager.ShaderSet(_shader, "FOG_Density", Ambience.GetEnvironment(obj.SpaceName).Density);
                AssetManager.ShaderSet(_shader, "FOG_Gradiante", Ambience.GetEnvironment(obj.SpaceName).Distance);
                AssetManager.ShaderSet(_shader, "FOG_Color", Ambience.GetEnvironment(obj.SpaceName).FogColor);

                AssetManager.ShaderSet(_shader, "AmbienceColor", Ambience.GetEnvironment(obj.SpaceName).AmbienceColor);

                GL.DrawElements(Debug.GLBeginMode, _mesh._indices.Length, DrawElementsType.UnsignedInt, 0);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                GL.BindVertexArray(0);

                if (_cullType != CullFaceMode.FrontAndBack)
                {
                    GL.Disable(EnableCap.CullFace);
                }

                if (Transparency)
                {
                    GL.Disable(EnableCap.Blend);
                }
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
}
