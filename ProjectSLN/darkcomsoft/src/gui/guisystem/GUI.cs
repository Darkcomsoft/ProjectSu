﻿using OpenTK.Mathematics;
using ProjectSLN.darkcomsoft.src.gui.guisystem.guielements;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;
using ProjectSLN.darkcomsoft.src.debug;
using ProjectSLN.darkcomsoft.src.misc;
using ProjectSLN.darkcomsoft.src.enums;
using System.Drawing;
using OpenTK.Windowing.Common;
using ProjectSLN.darkcomsoft.src.engine.window;
using ProjectSLN.darkcomsoft.src.engine;
using ProjectSLN.darkcomsoft.src.client;

namespace ProjectSLN.darkcomsoft.src.gui.guisystem
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

        private GUIBase m_currentFocusedGui;
        private GUIBase m_currentGuiHoverd;

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
        }

        public void Tick()
        {
            if (Input.GetKeyDown(Keys.I))
            {
                GameSettings.GuiScale += 0.1f;
                OnResize();
            }

            if (Input.GetKeyDown(Keys.O))
            {
                GameSettings.GuiScale -= 0.1f;
                OnResize();
            }

            for (int i = 0; i < m_guiList.Count; i++)
            {
                if (m_guiList[i].isEnabled)
                {
                    m_guiList[i].Tick();
                }
            }

            if (Input.GetKeyDown(Keys.Escape) || Input.GetKeyDown(Keys.Enter) || Input.GetKeyDown(Keys.KeyPadEnter))
            {
                if (m_currentFocusedGui != null)
                {
                    m_currentFocusedGui.SetStatus(GUIElementStatus.Focus, false);
                    m_currentFocusedGui = null;
                }
            }
        }

        public void Draw()
        {
            for (int i = 0; i < m_guiList.Count; i++)
            {
                if (m_guiList[i].isEnabled)
                {
                    m_guiList[i].Draw();
                }
            }
        }

        public void OnResize()
        {
            for (int i = 0; i < m_guiList.Count; i++)
            {
                m_guiList[i].Resize();
            }

            TickMouseHover();
        }

        public void OnMouseMove()
        {
            TickMouseHover();
        }

        public void OnMousePress(MouseButtonEventArgs e)
        {
            if (CursorManager.isLocked) { return; }
            if (!WindowMain.Instance.IsFocused) { return; }

            if (m_currentGuiHoverd != null)
            {
                m_currentFocusedGui = m_currentGuiHoverd;
                m_currentGuiHoverd.SetStatus(GUIElementStatus.Focus, true);
                m_currentGuiHoverd.MouseClick(e);
            }
            else
            {
                if (m_currentFocusedGui!=null)
                {
                    m_currentFocusedGui.SetStatus(GUIElementStatus.Focus, false);
                    m_currentFocusedGui = null;
                }
            }
        }

        public void OnMouseRelease(MouseButtonEventArgs e)
        {
            if (CursorManager.isLocked) { return; }
            if (!WindowMain.Instance.IsFocused) { return; }

            if (m_currentGuiHoverd != null)
            {
                m_currentGuiHoverd.MouseRelease(e);
            }
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

        public static void AddGUI(GUIBase gUIBase)
        {
            instance.m_guiList.Add(gUIBase);
        }

        public static void RemoveGUI(GUIBase gUIBase)
        {
            if (instance.m_guiList.Contains(gUIBase))
            {
                instance.m_guiList.Remove(gUIBase);
            }

            if (instance.m_guiDisabledList.Contains(gUIBase))
            {
                instance.m_guiDisabledList.Remove(gUIBase);
            }

            if (instance.m_currentFocusedGui == gUIBase)
            {
                instance.m_currentFocusedGui.SetStatus(GUIElementStatus.Focus, false);
                instance.m_currentFocusedGui = null;
            }

            if (instance.m_currentGuiHoverd == gUIBase)
            {
                instance.m_currentGuiHoverd.SetStatus(GUIElementStatus.Hover, false);
                instance.m_currentGuiHoverd = null;
            }
        }

        public static void TickMouseHover()
        {
            if (!CursorManager.isLocked)
            {
                if (instance.m_currentGuiHoverd != null && instance.m_currentGuiHoverd.IsMouseOn())
                {
                    return;
                }
                else if (instance.m_currentGuiHoverd != null)
                {
                    instance.m_currentGuiHoverd = null;
                }

                for (int i = instance.m_guiList.Count - 1; i >= 0; i--)
                {
                    if (instance.m_guiList[i].isInputEnabled)
                    {
                        if (instance.m_guiList[i].isMouseHover)
                        {
                            instance.m_guiList[i].SetStatus(GUIElementStatus.Hover, false);
                        }
                    }
                }

                for (int i = instance.m_guiList.Count - 1; i >= 0; i--)
                {
                    if (instance.m_guiList[i].isInputEnabled)
                    {
                        if (instance.m_guiList[i].IsMouseOn())
                        {
                            instance.m_guiList[i].SetStatus(GUIElementStatus.Hover, true);
                            instance.m_currentGuiHoverd = instance.m_guiList[i];
                            return;
                        }
                    }
                }
            }
        }

        public static GUI instance { get { return m_instance; } }
    }
}