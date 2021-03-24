using OpenTK.Mathematics;
using Projectsln.darkcomsoft.src.gui.guisystem.guielements;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;
using Projectsln.darkcomsoft.src.engine.window;

namespace Projectsln.darkcomsoft.src.gui.guisystem
{
    public class GUI : ClassBase
    {
        private static GUI m_instance;

        private List<GUIBase> m_guiList;

        private int[] m_rectangleIndices;
        private Vector2[] m_rectangleVertices;
        private Vector2[] m_rectangleUv;

        private int IBO, VAO, VBO, UBO;

        public GUI()
        {
            m_instance = this;

            m_guiList = new List<GUIBase>();
            StartRectangleMesh();

            BindRectangleBuffers();//Bind the rectangle data to videoCard

            for (int i = 0; i < 1; i++)
            {
                m_guiList.Add(new GUIBase());
            }
        }

        public void Tick(double time)
        { 
            for (int i = 0; i < m_guiList.Count; i++)
            {
                m_guiList[i].Tick();
            }
        }

        public void Draw(double time)
        {
             GL.Enable(EnableCap.Blend);
            GL.Disable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);

            for (int i = 0; i < m_guiList.Count; i++)
            {
                m_guiList[i].Draw();
                GL.DrawElements(PrimitiveType.Triangles, m_rectangleIndices.Length, DrawElementsType.UnsignedInt, 0);
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);

            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
        }

        protected override void OnDispose()
        {
            foreach (var item in m_guiList)
            {
                item.Dispose();
            }

            m_guiList.Clear();
            m_guiList = null;

            UnbindRectangleBuffers();

            m_instance = null;
            m_rectangleVertices = null;
            m_rectangleIndices = null;
            m_rectangleUv = null;
            base.OnDispose();
        }

        private void StartRectangleMesh()
        {
            m_rectangleVertices = new Vector2[4]
            {
                 new Vector2(0.5f,  0.5f), // top right
                 new Vector2(0.5f, -0.5f), // bottom right
                new Vector2(-0.5f, -0.5f), // bottom left
                new Vector2(-0.5f,  0.5f) // top left
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

        private void BindRectangleBuffers()
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

        private void UnbindRectangleBuffers()
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


        public static GUI instance { get { return m_instance; } }
    }
}
