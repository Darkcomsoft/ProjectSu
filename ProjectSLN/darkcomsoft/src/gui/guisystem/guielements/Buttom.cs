using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ProjectIND.darkcomsoft.src.debug;
using ProjectIND.darkcomsoft.src.enums;
using ProjectIND.darkcomsoft.src.gui.guisystem.font;
using ProjectIND.darkcomsoft.src.render;
using ProjectIND.darkcomsoft.src.resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ProjectIND.darkcomsoft.src.gui.guisystem.guielements
{
    /// <summary>
    /// A simple gui buttom, only one function click
    /// </summary>
    public class Buttom : GUIBase
    {
        private Shader v_shader;
        private bool v_isclick = false;
        private MouseButton v_mouseKey = MouseButton.Left;

        private Color4 v_normalColor = Color4.White;
        private Color4 v_clickColor = Color4.SlateGray;
        private Color4 v_hoverColor = Color4.Gray;
        private Color4 v_focusColor = Color4.DarkGray;
        private Color4 v_disableColor = Color4.DarkSlateGray;

        private FontRender v_fontRender;

        public event Action OnClick;

        public Buttom()
        {
            v_shader = ResourcesManager.GetShader("UI");
            v_fontRender = new FontRender("Buttom Text", 25, 1, this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            v_inputEnable = true;
        }

        public Buttom(string buttomText)
        {
            v_shader = ResourcesManager.GetShader("UI");
            v_fontRender = new FontRender(buttomText, 25, 1, this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            v_inputEnable = true;
        }

        public Buttom(string buttomText, RectangleF positionSize) : base(positionSize)
        {
            v_shader = ResourcesManager.GetShader("UI");
            v_fontRender = new FontRender(buttomText, 25, 1, this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            v_inputEnable = true;
        }

        public Buttom(string buttomText, RectangleF positionSize, GUIDock gUIDock) : base(positionSize, gUIDock)
        {
            v_shader = ResourcesManager.GetShader("UI");
            v_fontRender = new FontRender(buttomText, 25, 1, this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            v_inputEnable = true;
        }

        public Buttom(string buttomText, RectangleF positionSize, GUIDock gUIDock, GUIPivot gUIPivot) : base(positionSize, gUIDock, gUIPivot)
        {
            v_shader = ResourcesManager.GetShader("UI");
            v_fontRender = new FontRender(buttomText, 25, 1, this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));

            v_inputEnable = true;
        }

        protected override void OnDraw()
        {
            v_shader.Use();

            v_shader.Set("world", v_worldPosition);
            v_shader.Set("projection", v_projection);

            if (!v_inputEnable)
            {
                v_shader.Set("uicolor", v_disableColor);
            }
            else
            {
                if (isMouseHover)
                {
                    if (v_isclick)
                    {
                        v_shader.Set("uicolor", v_clickColor);
                    }
                    else
                    {
                        v_shader.Set("uicolor", v_hoverColor);
                    }
                }
                else
                {
                    if (v_Focused)
                    {
                        v_shader.Set("uicolor", v_focusColor);
                    }
                    else
                    {
                        v_shader.Set("uicolor", v_normalColor);
                    }
                }
            }

            GUI.instance.DrawRec(this);
            Gizmo.DrawRectangle(v_worldPosition, v_projection, Color4.Blue, OpenTK.Graphics.OpenGL.PrimitiveType.LineLoop);
            v_fontRender.Draw();
            base.OnDraw();
        }

        protected override void OnResize(bool after)
        {
            v_fontRender?.OnResize();
            base.OnResize(after);
        }

        protected override void OnMouseClick(MouseButtonEventArgs e)
        {
            if (e.Button == v_mouseKey)
            {
                v_isclick = true;
                OnClick?.Invoke();
            }

            base.OnMouseClick(e);
        }

        protected override void OnMouseRelease(MouseButtonEventArgs e)
        {
            if (e.Button == v_mouseKey)
            {
                v_isclick = false;
            }
            base.OnMouseRelease(e);
        }

        protected override void OnStatusChange(GUIElementStatus gUIElementStatus, params object[] parame)
        {
            if (!v_mouseHover && v_isclick)
            {
                v_isclick = false;
            }
            base.OnStatusChange(gUIElementStatus, parame);
        }

        protected override void OnDispose()
        {
            v_fontRender.Dispose();

            OnClick = null;

            v_fontRender = null;
            v_shader = null;
            OnClick = null;
            base.OnDispose();
        }

        public void SetText(string text)
        {
            v_fontRender?.SetText(text);
        }

        public void SetTextAling(TextAling textAling)
        {
            v_fontRender?.SetTextPivot(textAling);
        }

        public void SetTextColor(Color4 color)
        {
            v_fontRender?.SetColor(color);
        }

        /// <summary>
        /// Set all buttom colors, NormalColor, ClickColor, HoverColor, FocusColor
        /// </summary>
        /// <param name="normalColor">When buttom is in neutral state, any input. hover, click, or focused</param>
        /// <param name="clickColor">when click with the mouse buttom, on the buttom</param>
        /// <param name="hoverColor">when mouse is hover the buttom</param>
        /// <param name="focusColor">when you click and release mouse buttom, this buttom is gona be focused by the ui system</param>
        public void SetButtomColors(Color4 normalColor, Color4 clickColor, Color4 hoverColor, Color4 focusColor)
        {
            v_normalColor = normalColor;
            v_clickColor = clickColor;
            v_hoverColor = hoverColor;
            v_focusColor = focusColor;
        }

        public void SetButtomDisableColor(Color4 disableColor)
        {
            v_disableColor = disableColor;
        }

        /// <summary>
        /// this is for set the key, used to click, Default is the mouse left buttom
        /// </summary>
        public void SetMouseButtom(MouseButton mouseButton)
        {
            v_mouseKey = mouseButton;
        }
    }
}