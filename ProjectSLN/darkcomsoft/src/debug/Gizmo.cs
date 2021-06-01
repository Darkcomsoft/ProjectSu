using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using ProjectSLN.darkcomsoft.src.render;
using ProjectSLN.darkcomsoft.src.resources;

namespace ProjectSLN.darkcomsoft.src.debug
{
    public class Gizmo : ClassBase
    {
        private static Gizmo m_instance;

        private Shader m_gizmoShader;

        private Vector2[] m_rectangleVertices;
        private int VAO, VBO;

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

            m_rectangleVertices = null;
            m_gizmoShader = null;

            m_instance = null;
            base.OnDispose();
        }

        private void SetUpBuffers()
        {
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, m_rectangleVertices.Length * Vector2.SizeInBytes, m_rectangleVertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);
        }

        private void DisposeBuffers()
        {
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableVertexAttribArray(0);

            GL.DeleteBuffer(VBO);
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
        }

        public static void DrawCube()
        {
            if (!Debug.isDebugEnabled) { return; }
            if (m_instance == null) { return; }
            if (!m_instance.m_gizmoDisponivel) { return; }


        }

        /// <summary>
        /// Draw a 2d Rectangle using a world matrix and a projection, its commom used on the ui debug
        /// </summary>
        /// <param name="world"></param>
        /// <param name="projection"></param>
        /// <param name="color"></param>
        /// <param name="primitiveType"></param>
        public static void DrawRectangle(Matrix4 world, Matrix4 projection, Color4 color, PrimitiveType primitiveType)
        {
            if (!Debug.isDebugEnabled) { return; }
            if (m_instance == null) { return; }
            if (!m_instance.m_gizmoDisponivel) { return; }

            GL.Enable(EnableCap.Blend);
            GL.Disable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            //Draw Start
            GL.BindVertexArray(m_instance.VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, m_instance.VBO);

            m_instance.m_gizmoShader.Use();

            m_instance.m_gizmoShader.Set("world", world);
            m_instance.m_gizmoShader.Set("projection", projection);
            m_instance.m_gizmoShader.Set("gizmoColor", color);

            GL.DrawArrays(primitiveType, 0, m_instance.m_rectangleVertices.Length);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            //Draw End

            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
        }

        public static void DrawRectangle(float x, float y, float w, float h, Matrix4 projection, Color4 color, PrimitiveType primitiveType)
        {
            DrawRectangle(Matrix4.CreateScale(w,h,0) * Matrix4.CreateTranslation(x,y,0), projection, color, primitiveType);
        }
    }
}
