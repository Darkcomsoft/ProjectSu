using OpenTK.Mathematics;
using ProjectIND.darkcomsoft.src.gui.guisystem.guielements;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;
using ProjectIND.darkcomsoft.src.debug;
using ProjectIND.darkcomsoft.src.misc;
using ProjectIND.darkcomsoft.src.enums;
using System.Drawing;
using OpenTK.Windowing.Common;
using ProjectIND.darkcomsoft.src.engine.window;
using ProjectIND.darkcomsoft.src.engine;
using ProjectIND.darkcomsoft.src.client;

namespace ProjectIND.darkcomsoft.src.gui.guisystem
{
    public class GUI : ClassBase
    {
        private static GUI v_instance;

        /// <summary>
        /// This list is used to tick and draw all activated guielements
        /// </summary>
        private List<GUIBase> v_guiList;
        /// <summary>
        /// This list is used for store all disabled gui elements
        /// </summary>
        private List<GUIBase> v_guiDisabledList;

        private GUIBase v_currentFocusedGui;
        private GUIBase v_currentGuiHoverd;

        private int[] v_rectangleIndices;
        private Vector2[] v_rectangleVertices;
        private Vector2[] v_rectangleUv;
        private int IBO, VAO, VBO, UBO;

        public GUI()
        {
            v_instance = this;

            v_guiList = new List<GUIBase>();
            v_guiDisabledList = new List<GUIBase>();

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

            for (int i = 0; i < v_guiList.Count; i++)
            {
                if (v_guiList[i].isEnabled)
                {
                    v_guiList[i].Tick();
                }
            }

            if (Input.GetKeyDown(Keys.Escape) || Input.GetKeyDown(Keys.Enter) || Input.GetKeyDown(Keys.KeyPadEnter))
            {
                if (v_currentFocusedGui != null)
                {
                    v_currentFocusedGui.SetStatus(GUIElementStatus.Focus, false);
                    v_currentFocusedGui = null;
                }
            }
        }

        public void Draw()
        {
            for (int i = 0; i < v_guiList.Count; i++)
            {
                if (v_guiList[i].isEnabled)
                {
                    v_guiList[i].Draw();
                }
            }
        }

        public void OnResize()
        {
            for (int i = 0; i < v_guiList.Count; i++)
            {
                v_guiList[i].Resize();
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

            if (v_currentGuiHoverd != null)
            {
                v_currentFocusedGui = v_currentGuiHoverd;
                v_currentGuiHoverd.SetStatus(GUIElementStatus.Focus, true);
                v_currentGuiHoverd.MouseClick(e);
            }
            else
            {
                if (v_currentFocusedGui!=null)
                {
                    v_currentFocusedGui.SetStatus(GUIElementStatus.Focus, false);
                    v_currentFocusedGui = null;
                }
            }
        }

        public void OnMouseRelease(MouseButtonEventArgs e)
        {
            if (CursorManager.isLocked) { return; }
            if (!WindowMain.Instance.IsFocused) { return; }

            if (v_currentGuiHoverd != null)
            {
                v_currentGuiHoverd.MouseRelease(e);
            }
        }

        protected override void OnDispose()
        {
            foreach (var item in v_guiList)
            {
                item.Dispose();
            }

            v_guiList.Clear();
            v_guiList = null;

            UnbindRectangleBuffers();

            v_instance = null;
            v_rectangleVertices = null;
            v_rectangleIndices = null;
            v_rectangleUv = null;
            base.OnDispose();
        }

        private void StartRectangleMesh()
        {
            v_rectangleVertices = new Vector2[4]
            {
                 new Vector2(1f,  1f), // top right
                 new Vector2(1f, -1f), // bottom right
                new Vector2(-1f, -1f), // bottom left
                new Vector2(-1f,  1f) // top left
            };

            v_rectangleIndices = new int[6]
            {
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
            };

            v_rectangleUv = new Vector2[4] {
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
            GL.BufferData(BufferTarget.ElementArrayBuffer, v_rectangleIndices.Length * sizeof(int), v_rectangleIndices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, v_rectangleVertices.Length * Vector2.SizeInBytes, v_rectangleVertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, UBO);
            GL.BufferData(BufferTarget.ArrayBuffer, v_rectangleUv.Length * Vector2.SizeInBytes, v_rectangleUv, BufferUsageHint.StaticDraw);
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
            v_guiDisabledList.Add(gUI);
            v_guiList.Remove(gUI);
        }

        public void s_DisableGUI(GUIBase gUI)
        {
            v_guiDisabledList.Remove(gUI);
            v_guiList.Add(gUI);
        }

        public void DrawRec(GUIBase gUIBase)
        {
            GL.Enable(EnableCap.Blend);
            GL.Disable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);

            GL.DrawElements(PrimitiveType.Triangles, v_rectangleIndices.Length, DrawElementsType.UnsignedInt, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);

            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Blend);
        }

        public static void AddGUI(GUIBase gUIBase)
        {
            instance.v_guiList.Add(gUIBase);
        }

        public static void RemoveGUI(GUIBase gUIBase)
        {
            if (instance.v_guiList.Contains(gUIBase))
            {
                instance.v_guiList.Remove(gUIBase);
            }

            if (instance.v_guiDisabledList.Contains(gUIBase))
            {
                instance.v_guiDisabledList.Remove(gUIBase);
            }

            if (instance.v_currentFocusedGui == gUIBase)
            {
                instance.v_currentFocusedGui.SetStatus(GUIElementStatus.Focus, false);
                instance.v_currentFocusedGui = null;
            }

            if (instance.v_currentGuiHoverd == gUIBase)
            {
                instance.v_currentGuiHoverd.SetStatus(GUIElementStatus.Hover, false);
                instance.v_currentGuiHoverd = null;
            }
        }

        public static void TickMouseHover()
        {
            if (!CursorManager.isLocked)
            {
                if (instance.v_currentGuiHoverd != null && instance.v_currentGuiHoverd.IsMouseOn())
                {
                    return;
                }
                else if (instance.v_currentGuiHoverd != null)
                {
                    instance.v_currentGuiHoverd = null;
                }

                for (int i = instance.v_guiList.Count - 1; i >= 0; i--)
                {
                    if (instance.v_guiList[i].isInputEnabled)
                    {
                        if (instance.v_guiList[i].isMouseHover)
                        {
                            instance.v_guiList[i].SetStatus(GUIElementStatus.Hover, false);
                        }
                    }
                }

                for (int i = instance.v_guiList.Count - 1; i >= 0; i--)
                {
                    if (instance.v_guiList[i].isInputEnabled)
                    {
                        if (instance.v_guiList[i].IsMouseOn())
                        {
                            instance.v_guiList[i].SetStatus(GUIElementStatus.Hover, true);
                            instance.v_currentGuiHoverd = instance.v_guiList[i];
                            return;
                        }
                    }
                }
            }
        }

        public static GUI instance { get { return v_instance; } }
    }
}
