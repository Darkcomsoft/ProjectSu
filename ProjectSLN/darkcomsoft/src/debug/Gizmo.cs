using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using ProjectIND.darkcomsoft.src.render;
using ProjectIND.darkcomsoft.src.resources;

namespace ProjectIND.darkcomsoft.src.debug
{
    public class Gizmo : ClassBase
    {
        private static Gizmo v_instance;

        private Shader v_gizmoShader;

        private Vector2[] v_rectangleVertices;
        private int VAO, VBO;

        private bool v_gizmoDisponivel = false;

        public Gizmo()
        {
            v_gizmoShader = ResourcesManager.GetShader("Gizmo");

            v_instance = this;
            CreateQuad();
            SetUpBuffers();
            v_gizmoDisponivel = true;
        }

        protected override void OnDispose()
        {
            v_gizmoDisponivel = false;

            DisposeBuffers();

            v_rectangleVertices = null;
            v_gizmoShader = null;

            v_instance = null;
            base.OnDispose();
        }

        private void SetUpBuffers()
        {
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, v_rectangleVertices.Length * Vector2.SizeInBytes, v_rectangleVertices, BufferUsageHint.StaticDraw);
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
            v_rectangleVertices = new Vector2[4]
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
            if (v_instance == null) { return; }
            if (!v_instance.v_gizmoDisponivel) { return; }


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
            if (v_instance == null) { return; }
            if (!v_instance.v_gizmoDisponivel) { return; }

            GL.Enable(EnableCap.Blend);
            GL.Disable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            //Draw Start
            GL.BindVertexArray(v_instance.VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, v_instance.VBO);

            v_instance.v_gizmoShader.Use();

            v_instance.v_gizmoShader.Set("world", world);
            v_instance.v_gizmoShader.Set("projection", projection);
            v_instance.v_gizmoShader.Set("gizmoColor", color);

            GL.DrawArrays(primitiveType, 0, v_instance.v_rectangleVertices.Length);

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
