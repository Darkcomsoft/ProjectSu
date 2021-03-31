using OpenTK.Mathematics;
using Projectsln.darkcomsoft.src.gui.guisystem.guielements;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;
using Projectsln.darkcomsoft.src.engine.window;
using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.misc;
using Projectsln.darkcomsoft.src.enums;
using System.Drawing;

namespace Projectsln.darkcomsoft.src.gui.guisystem
{
    public class GUI : ClassBase
    {
        private static GUI m_instance;

        /// <summary>
        /// This list is used to tick and draw all activated guielements
        /// </summary>
        private List<GUIBase> m_guiList;
        /// <summary>
        /// This list is used for store all disabled gui elements
        /// </summary>
        private List<GUIBase> m_guiDisabledList;

        private int[] m_rectangleIndices;
        private Vector2[] m_rectangleVertices;
        private Vector2[] m_rectangleUv;
        private int IBO, VAO, VBO, UBO;

        public GUI()
        {
            m_instance = this;

            m_guiList = new List<GUIBase>();
            m_guiDisabledList = new List<GUIBase>();

            StartRectangleMesh();

            BindRectangleBuffers();//Bind the rectangle data to videoCard

            m_guiList.Add(new GUIBase(new RectangleF(0, 0, 50f, 50f), GUIDock.RightBottom));
            m_guiList.Add(new GUIBase(new RectangleF(0, 0, 50f, 50f), GUIDock.RightTop));
            m_guiList.Add(new GUIBase(new RectangleF(0, 0, 50f, 50f), GUIDock.LeftBottom));
            m_guiList.Add(new GUIBase(new RectangleF(0, 0, 50f, 50f), GUIDock.LeftTop));

            m_guiList.Add(new GUIBase(new RectangleF(0, 0, 50f, 50f), GUIDock.Top));
            m_guiList.Add(new GUIBase(new RectangleF(0, 0, 50f, 50f), GUIDock.Bottom));

            m_guiList.Add(new GUIBase(new RectangleF(0, 0, 50f, 50f), GUIDock.Left));
            m_guiList.Add(new GUIBase(new RectangleF(0, 0, 50f, 50f), GUIDock.Right));

            m_guiList.Add(new GUIBase(new RectangleF(0, 0, 50f, 50f), GUIDock.Center, GUIPivot.Center));
        }

        public void Tick(double time)
        {
            if (Input.GetKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.I, 566))
            {
                GameSettings.GuiScale += 50;
                OnResize();
            }

            if (Input.GetKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.O, 234))
            {
                GameSettings.GuiScale -= 50;
                OnResize();
            }

            for (int i = 0; i < m_guiList.Count; i++)
            {
                if (m_guiList[i].isEnabled)
                {
                    m_guiList[i].Tick();
                }
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
                if (m_guiList[i].isEnabled)
                {
                    m_guiList[i].Draw();
                    GL.DrawElements(PrimitiveType.Triangles, m_rectangleIndices.Length, DrawElementsType.UnsignedInt, 0);
                }
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);

            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
        }

        public void OnResize()
        {
            for (int i = 0; i < m_guiList.Count; i++)
            {
                m_guiList[i].Resize();
            }

            RefreshInput();
        }

        public void OnMouseMove()
        {
            RefreshInput();
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

        public void s_EnableGUI(GUIBase gUI)
        {
            m_guiDisabledList.Add(gUI);
            m_guiList.Remove(gUI);
        }

        public void s_DisableGUI(GUIBase gUI)
        {
            m_guiDisabledList.Remove(gUI);
            m_guiList.Add(gUI);
        }

        public void DrawRec(GUIBase gUIBase)
        {
            GL.Enable(EnableCap.Blend);
            GL.Disable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);

            GL.DrawElements(PrimitiveType.Triangles, m_rectangleIndices.Length, DrawElementsType.UnsignedInt, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);

            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
        }

        public static void RefreshInput()
        {
            if (!CursorManager.isLocked)
            {
                for (int i = instance.m_guiList.Count - 1; i >= 0; i--)
                {
                    instance.m_guiList[i].UpdateMouseStatus(GUIMouseState.UnHover);
                }

                for (int i = instance.m_guiList.Count - 1; i >= 0; i--)
                {
                    if (instance.m_guiList[i].IsMouseOn())
                    {
                        instance.m_guiList[i].UpdateMouseStatus(GUIMouseState.Hover);
                        return;
                    }
                }
            }
        }

        public static GUI instance { get { return m_instance; } }
    }
}
