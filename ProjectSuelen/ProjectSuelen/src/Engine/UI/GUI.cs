using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using ProjectSuelen;
using System.Drawing;
using OpenTK.Input;
using ProjectSuelen.src.Engine.Render;
using ProjectSuelen.src.Engine.AssetsPipeline;

namespace ProjectSuelen.src.Engine.UI
{
    public class GUI : ClassBase
    {
        private static GUI Instance; 

        private Shader _Shader;
        private Shader _FontShader;

        private List<GUIBase> GuiBaseList;

        public static float GUISize = 2;

        private GUIBase UpUI;
        private GUIBase FocusedUI;
        private bool pressed;

        public GUI()
        {
            Instance = this;

            GuiBaseList = new List<GUIBase>();

            _Shader = AssetManager.GetShader("GUI");
            _FontShader = AssetManager.GetShader("Font");
        }

        public void Tick()
        {
            if (pressed)
            {
                if (Mouse.GetCursorState().LeftButton == ButtonState.Released)
                {
                    if (UpUI != null)
                    {
                        UpUI.ClickReleased();
                    }
                    pressed = false;
                }
            }
            else
            {
                if (Mouse.GetCursorState().LeftButton == ButtonState.Pressed)
                {
                    if (UpUI != null)
                    {
                        if (UpUI.IsHover)
                        {
                            if (FocusedUI != null)
                            {
                                FocusedUI.UnFocus();
                            }

                            UpUI.Click();
                            FocusedUI = UpUI;
                            
                            FocusedUI.Focus();
                        }
                        else
                        {
                            if (FocusedUI != null)
                            {
                                FocusedUI.UnFocus();
                            }
                        }
                    }
                    pressed = true;
                }
            }

            if (Input.GetKeyDown(Key.Escape) || Input.GetKeyDown(Key.Enter) || Input.GetKeyDown(Key.KeypadEnter))
            {
                if (FocusedUI != null)
                {
                    FocusedUI.UnFocus();
                }
            }

            /*for (int i = 0; i < GuiBaseList.Count; i++)
            {
                GuiBaseList[i].Tick();
            }*/
        }

        public void TickRender()
        {
            for (int i = 0; i < GuiBaseList.Count; i++)
            {
                GuiBaseList[i].TickRender();
            }
        }

        public void OnResize()
        {
            for (int i = 0; i < GuiBaseList.Count; i++)
            {
                for (int v = 0; v < 2; v++)//Check the size changes twice
                {
                    GuiBaseList[i].Resize();
                }
            }
        }

        public void OnMouseMove(MouseMoveEventArgs e)
        {
            if (!MouseCursor.MouseLocked)
            {
                Point point = new Point(e.X, e.Y);
                
                foreach (var item in GuiBaseList)
                {
                    if (item.IsInteract)
                    {
                        if (item.IsEnabled)
                        {
                            if (item.GetRectangle.Contains(point))
                            {
                                UpUI = item;

                                item.UnHover();
                                //item.UnFocus();
                            }
                            else
                            {
                                item.UnHover();
                                //item.UnFocus();
                            }
                        }
                    }
                    else
                    {
                        item.UnHover();
                        item.UnFocus();
                    }
                }

                if (UpUI != null)
                {
                    if (UpUI.GetRectangle.Contains(point))
                    {
                        UpUI.Hover();
                    }
                    else
                    {
                        UpUI.UnHover();
                    }
                }
            }
        }

        public void OnKeyPress(KeyPressEventArgs e)
        {
            if (!MouseCursor.MouseLocked)
            {
                if (FocusedUI != null)
                {
                    FocusedUI.OnKeyPress(e.KeyChar);
                }
            }
        }

        protected override void OnDispose()
        {
            for (int i = 0; i < GuiBaseList.Count; i++)
            {
                GuiBaseList[i].Dispose();
            }

            GuiBaseList.Clear();
            GuiBaseList = null;
            Instance = null;
            base.OnDispose();
        }

        public static void AddGuiElement(GUIBase baseGui)
        {
            GUI.instance.GuiBaseList.Add(baseGui);
        }

        public static void RemoveGuiElement(GUIBase baseGui)
        {
            GUI.instance.GuiBaseList.Remove(baseGui);
        }

        public static void CallTickRender()
        {
            GUI.instance.Tick();
            GUI.instance.TickRender();
        }

        public static Shader GetShader { get { return GUI.Instance._Shader; } }
        public static Shader GetFontShader { get { return GUI.Instance._FontShader; } }
        public static GUI instance { get { return Instance; } private set { } }
    }

    public enum UIDock : byte
    {
        Free,
        Cennter, CenterTop, CenterBottom, CenterLeft, CenterRight,
        TopLeft, TopRight,
        BottomLeft, BottomRight,

        SizeBottom, SizeTop, SizeLeft, SizeRight,//this is for dock position, but resize the ui element
        ScreenSize, ScreenSizeRatio
    }

    public enum TextureType
    {
        Empty, AssetTexture, BitMapTextures
    }

    public struct UIRectangle
    {
        public float x;
        public float y;
        public float w;
        public float h;

        public UIRectangle(float x, float y, float w, float h)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }

        public bool In(Rectangle rec, Point Point)
        {
            return Point.X >= rec.Location.X && rec.Y >= rec.Location.Y &&
                Point.X < rec.Location.X + rec.Size.Width && Point.Y < rec.Location.Y + rec.Size.Height;
        }
    }
}
