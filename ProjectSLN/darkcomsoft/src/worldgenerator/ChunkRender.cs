using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using ProjectIND.darkcomsoft.src.debug;
using ProjectIND.darkcomsoft.src.engine;
using ProjectIND.darkcomsoft.src.render;
using ProjectIND.darkcomsoft.src.resources;

namespace ProjectIND.darkcomsoft.src.worldgenerator
{
    public class ChunkRender : ClassBase
    {
        private Chunk v_chunk;

        private ChunkMesh v_mesh;

        private Texture v_texture;
        private Shader v_shader;

        private int v_IBO;
        private int v_VAO;
        private int v_VBO;
        private int v_TBO;
        private int v_NBO;

        public ChunkRender(Chunk chunk, ChunkMesh mesh)
        {
            v_chunk = chunk;
            v_mesh = mesh;

            v_texture = ResourcesManager.GetTexture("BlockAtlas");
            v_shader = ResourcesManager.GetShader("ChunkShader");

            GenBuffers();
            BindBuffers();
        }

        protected override void OnDispose()
        {
            UnbindBuffers();

            v_shader = null;
            v_texture = null;
            v_chunk = null;
            base.OnDispose();
        }

        public void TickDraw()
        {
            if (v_chunk != null && Camera.main != null)
            {
                GL.FrontFace(FrontFaceDirection.Ccw);

                GL.CullFace(CullFaceMode.Front);
                GL.Enable(EnableCap.CullFace);
                GL.Enable(EnableCap.DepthTest);
                v_texture.Use();

                v_shader.Use();

                v_shader.Set("world", v_chunk.transform.GetTransformWorld);
                v_shader.Set("view", Camera.main.GetViewMatrix());
                v_shader.Set("projection", Camera.main.GetProjectionMatrix());

                //Set The Fog Values(this need to be in all mesh with use fog)
                v_shader.Set("FOG_Density", v_chunk.GetWorld.Density);
                v_shader.Set("FOG_Gradiante", v_chunk.GetWorld.Distance);
                v_shader.Set("FOG_Color", v_chunk.GetWorld.FogColor);

                v_shader.Set("AmbienceColor", v_chunk.GetWorld.AmbienceColor);

                GL.BindVertexArray(v_VAO);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, v_IBO);
                GL.DrawElements(BeginMode.Triangles, v_mesh.v_indices.Length, DrawElementsType.UnsignedInt, 0);//DrawCall

                if (Debug.isDebugEnabled)
                {
                    GL.DrawElements(BeginMode.LineLoop, v_mesh.v_indices.Length, DrawElementsType.UnsignedInt, 0);//Wire DrawCall - For Debug
                }

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                GL.BindVertexArray(0);

                GL.Disable(EnableCap.CullFace);
                GL.Disable(EnableCap.DepthTest);
            }
        }

        #region Bind
        private void GenBuffers()
        {
            v_IBO = GL.GenBuffer();
            v_VAO = GL.GenVertexArray();

            v_VBO = GL.GenBuffer();
            v_TBO = GL.GenBuffer();
            v_NBO = GL.GenBuffer();
        }

        private void BindBuffers()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, v_IBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, v_mesh.v_indices.Length * sizeof(int), v_mesh.v_indices, BufferUsageHint.DynamicDraw);

            GL.BindVertexArray(v_VAO);

            //Vertices(Vector3)
            GL.BindBuffer(BufferTarget.ArrayBuffer, v_VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, v_mesh.v_Vertices.Length * Vector3.SizeInBytes, v_mesh.v_Vertices, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            //Texture(Vector2)
            GL.BindBuffer(BufferTarget.ArrayBuffer, v_TBO);
            GL.BufferData(BufferTarget.ArrayBuffer, v_mesh.v_uvs.Length * Vector2.SizeInBytes, v_mesh.v_uvs, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);

            //Mesh Normals(Vector3)
            GL.BindBuffer(BufferTarget.ArrayBuffer, v_NBO);
            GL.BufferData(BufferTarget.ArrayBuffer, v_mesh.v_normals.Length * Vector3.SizeInBytes, v_mesh.v_normals, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(2);
        }

        private void UnbindBuffers()
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

            GL.DeleteBuffer(v_IBO);
            GL.DeleteBuffer(v_TBO);
            GL.DeleteBuffer(v_NBO);

            GL.DeleteVertexArray(v_VAO);
        }
        #endregion

        public void UpdateMeshRender(ChunkMesh chunkMesh)
        {
            v_mesh = chunkMesh;
            BindBuffers();
        }
    }
}
