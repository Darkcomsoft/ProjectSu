﻿using ProjectSu.src.Engine.AssetsPipeline;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace ProjectSu.src.Engine.Render
{
    public class WaterMeshRender : ClassBase
    {
        private GameObject gameObject;

        public Mesh _Mesh;
        public string _shader;
        public string _texture;
        private int IBO, VAO, vbo, dbo, nbo;
        private int IndiceLength;

        private bool isReady;

        public WaterMeshRender(GameObject gameObject, Mesh mesh, string shader, string texture)
        {
            isReady = false;

            this.gameObject = gameObject;

            _Mesh = new Mesh(mesh);

            _shader = shader;
            _texture = texture;

            IndiceLength = _Mesh._indices.Length;

            IBO = GL.GenBuffer();
            VAO = GL.GenVertexArray();

            vbo = GL.GenBuffer();
            dbo = GL.GenBuffer();
            nbo = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _Mesh._indices.Length * sizeof(int), _Mesh._indices, BufferUsageHint.StreamDraw);

            GL.BindVertexArray(VAO);

            //Vertices(Vector3d)
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _Mesh._vertices.Length * Vector3.SizeInBytes, _Mesh._vertices, BufferUsageHint.StreamDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            //Colors(Vectro4|Color)
            GL.BindBuffer(BufferTarget.ArrayBuffer, dbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _Mesh._Colors.Length * Vector4.SizeInBytes, _Mesh._Colors, BufferUsageHint.StreamDraw);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);

            //Mesh Normals(Vector3d)
            GL.BindBuffer(BufferTarget.ArrayBuffer, nbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _Mesh._Normals.Length * Vector3.SizeInBytes, _Mesh._Normals, BufferUsageHint.StreamDraw);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(2);

            isReady = true;
        }

        public void DrawWater()
        {
            if (Camera.main != null && isReady)
            {
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
                GL.Enable(EnableCap.Blend);

                AssetManager.UseTexture(_texture);

                AssetManager.UseShader(_shader);

                AssetManager.ShaderSet(_shader, "world", gameObject.transform.GetTransformWorld);
                AssetManager.ShaderSet(_shader, "view", Camera.main.GetViewMatrix());
                AssetManager.ShaderSet(_shader, "projection", Camera.main.GetProjectionMatrix());
                AssetManager.ShaderSet(_shader, "time", Time._Tick);

                //Set The Fog Values(this need to be in all mesh with use fog)
                AssetManager.ShaderSet(_shader, "FOG_Density", Ambience.GetEnvironment(gameObject.SpaceName).Density);
                AssetManager.ShaderSet(_shader, "FOG_Gradiante", Ambience.GetEnvironment(gameObject.SpaceName).Distance);
                AssetManager.ShaderSet(_shader, "FOG_Color", Ambience.GetEnvironment(gameObject.SpaceName).FogColor);

                AssetManager.ShaderSet(_shader, "AmbienceColor", Ambience.GetEnvironment(gameObject.SpaceName).AmbienceColor);

                GL.BindVertexArray(VAO);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
                GL.DrawElements(Debug.GLBeginMode, IndiceLength, DrawElementsType.UnsignedInt, 0);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                GL.BindVertexArray(0);

                GL.Disable(EnableCap.Blend);
            }
        }

        public void UpdateMeshRender(Mesh mesh)
        {
            if (isReady)
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

                isReady = false;
            }

            _Mesh = new Mesh(mesh);

            IndiceLength = _Mesh._indices.Length;

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _Mesh._indices.Length * sizeof(int), _Mesh._indices, BufferUsageHint.StreamDraw);

            GL.BindVertexArray(VAO);

            //Vertices(Vector3d)
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _Mesh._vertices.Length * Vector3.SizeInBytes, _Mesh._vertices, BufferUsageHint.StreamDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            //Colors(Vectro4|Color)
            GL.BindBuffer(BufferTarget.ArrayBuffer, dbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _Mesh._Colors.Length * Vector4.SizeInBytes, _Mesh._Colors, BufferUsageHint.StreamDraw);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);

            //Mesh Normals(Vector3d)
            GL.BindBuffer(BufferTarget.ArrayBuffer, nbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _Mesh._Normals.Length * Vector3.SizeInBytes, _Mesh._Normals, BufferUsageHint.StreamDraw);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(2);

            isReady = true;
        }

        protected override void OnDispose()
        {
            if (isReady)
            {
                isReady = false;

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

                GL.DeleteBuffer(IBO);

                GL.DeleteBuffer(vbo);
                GL.DeleteBuffer(dbo);
                GL.DeleteBuffer(nbo);

                GL.DeleteVertexArray(VAO);

                gameObject = null;
            }
            base.OnDispose();
        }
    }
}