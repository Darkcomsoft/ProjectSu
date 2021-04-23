using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Projectsln.darkcomsoft.src.render;
using Projectsln.darkcomsoft.src.resources;

namespace Projectsln.darkcomsoft.src.debug
{
    public class Gizmo : ClassBase
    {
        private static Gizmo m_instance;

        private Shader m_gizmoShader;

        private int[] m_rectangleIndices;
        private Vector2[] m_rectangleVertices;
        private Vector2[] m_rectangleUv;
        private int IBO, VAO, VBO, UBO;

        private bool m_gizmoDisponivel = false;

        public Gizmo()
        {
            m_gizmoShader = ResourcesManager.GetShader("Gizmo");

            m_instance = this;
            CreateQuad();
            SetUpBuffers();
            m_gizmoDisponivel = true;
        }

        protected override void OnDispose()
        {
            m_gizmoDisponivel = false;

            DisposeBuffers();

            m_rectangleIndices = null;
            m_rectangleVertices = null;
            m_rectangleUv = null;
            m_gizmoShader = null;

            m_instance = null;
            base.OnDispose();
        }

        private void SetUpBuffers()
        {
            VAO = GL.GenVertexArray();//VertexArray Buffer
            IBO = GL.GenBuffer();//Index Buffer

            VBO = GL.GenBuffer();//Vertex Buffer
            UBO = GL.GenBuffer();//Uv Buffer

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, m_rectangleIndices.Length * sizeof(int), m_rectangleIndices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, m_rectangleVertices.Length * Vector2.SizeInBytes, m_rectangleVertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, UBO);
            GL.BufferData(BufferTarget.ArrayBuffer, m_rectangleUv.Length * Vector2.SizeInBytes, m_rectangleUv, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);
        }

        private void DisposeBuffers()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableVertexAttribArray(0);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableVertexAttribArray(0);

            GL.DeleteBuffer(IBO);
            GL.DeleteBuffer(VBO);
            GL.DeleteBuffer(UBO);
            GL.DeleteVertexArray(VAO);
        }

        private void CreateQuad()
        {
            m_rectangleVertices = new Vector2[4]
           {
                 new Vector2(1f,  1f), // top right
                 new Vector2(1f, -1f), // bottom right
                new Vector2(-1f, -1f), // bottom left
                new Vector2(-1f,  1f) // top left
           };

            m_rectangleIndices = new int[6]
            {
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
            };

            m_rectangleUv = new Vector2[4] {
                new Vector2(0.0f, 1.0f),
                new Vector2(1.0f, 1.0f),

                new Vector2( 1.0f, 0.0f),
                new Vector2(0.0f, 0.0f)
            };
        }

        public static void DrawCube()
        {

        }

        public static void DrawRectangle(Matrix4 world, Matrix4 projection, Color4 color, PrimitiveType primitiveType)
        {
            if (m_instance == null) { return; }
            if (!m_instance.m_gizmoDisponivel) { return; }

            color.A = 0.5f;

            GL.Enable(EnableCap.Blend);
            GL.Disable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.BindVertexArray(m_instance.VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, m_instance.IBO);

            m_instance.m_gizmoShader.Use();

            m_instance.m_gizmoShader.Use();

            m_instance.m_gizmoShader.Set("world", world);
            m_instance.m_gizmoShader.Set("projection", projection);
            m_instance.m_gizmoShader.Set("gizmoColor", color);

            GL.DrawElements(primitiveType, m_instance.m_rectangleIndices.Length, DrawElementsType.UnsignedInt, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);

            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
        }

        public static void DrawRectangle(float x, float y, float w, float h, Matrix4 projection, Color4 color, PrimitiveType primitiveType)
        {

        }
    }

    public enum GizmoDrawMode
    {
        Wire, Full
    }
}
