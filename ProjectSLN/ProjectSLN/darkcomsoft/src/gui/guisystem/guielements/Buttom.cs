using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Projectsln.darkcomsoft.src.debug;
using Projectsln.darkcomsoft.src.enums;
using Projectsln.darkcomsoft.src.gui.guisystem.font;
using Projectsln.darkcomsoft.src.render;
using Projectsln.darkcomsoft.src.resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Projectsln.darkcomsoft.src.gui.guisystem.guielements
{
    /// <summary>
    /// A simple gui buttom, only one function click
    /// </summary>
    public class Buttom : GUIBase
    {
        private Shader m_shader;
        private bool m_isclick = false;
        private MouseButton m_mouseKey = MouseButton.Left;

        private Color4 m_normalColor = Color4.White;
        private Color4 m_clickColor = Color4.SlateGray;
        private Color4 m_hoverColor = Color4.Gray;
        private Color4 m_focusColor = Color4.DarkGray;
        private Color4 m_disableColor = Color4.DarkSlateGray;

        private FontRender m_fontRender;

        public Action OnClick;

        public Buttom()
        {
            m_shader = ResourcesManager.GetShader("UI");
            m_fontRender = new FontRender("Buttom Text", 25, 1, this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            m_inputEnable = true;
        }

        public Buttom(string buttomText)
        {
            m_shader = ResourcesManager.GetShader("UI");
            m_fontRender = new FontRender(buttomText, 25, 1, this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            m_inputEnable = true;
        }

        public Buttom(string buttomText, RectangleF positionSize) : base(positionSize)
        {
            m_shader = ResourcesManager.GetShader("UI");
            m_fontRender = new FontRender(buttomText, 25, 1, this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            m_inputEnable = true;
        }

        public Buttom(string buttomText, RectangleF positionSize, GUIDock gUIDock) : base(positionSize, gUIDock)
        {
            m_shader = ResourcesManager.GetShader("UI");
            m_fontRender = new FontRender(buttomText, 25, 1, this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            m_inputEnable = true;
        }

        public Buttom(string buttomText, RectangleF positionSize, GUIDock gUIDock, GUIPivot gUIPivot) : base(positionSize, gUIDock, gUIPivot)
        {
            m_shader = ResourcesManager.GetShader("UI");
            m_fontRender = new FontRender(buttomText, 25, 1, this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));

            m_inputEnable = true;
        }

        protected override void OnDraw()
        {
            m_shader.Use();

            m_shader.Set("world", m_worldPosition);
            m_shader.Set("projection", m_projection);

            if (!m_inputEnable)
            {
                m_shader.Set("uicolor", m_disableColor);
            }
            else
            {
                if (isMouseHover)
                {
                    if (m_isclick)
                    {
                        m_shader.Set("uicolor", m_clickColor);
                    }
                    else
                    {
                        m_shader.Set("uicolor", m_hoverColor);
                    }
                }
                else
                {
                    if (m_Focused)
                    {
                        m_shader.Set("uicolor", m_focusColor);
                    }
                    else
                    {
                        m_shader.Set("uicolor", m_normalColor);
                    }
                }
            }

            GUI.instance.DrawRec(this);
            Gizmo.DrawRectangle(m_worldPosition, m_projection, Color4.Blue, OpenTK.Graphics.OpenGL.PrimitiveType.LineLoop);
            m_fontRender.Draw();
            base.OnDraw();
        }

        protected override void OnResize(bool after)
        {
            m_fontRender?.OnResize();
            base.OnResize(after);
        }

        protected override void OnMouseClick(MouseButtonEventArgs e)
        {
            if (e.Button == m_mouseKey)
            {
                m_isclick = true;
                OnClick?.Invoke();
            }

            base.OnMouseClick(e);
        }

        protected override void OnMouseRelease(MouseButtonEventArgs e)
        {
            if (e.Button == m_mouseKey)
            {
                m_isclick = false;
            }
            base.OnMouseRelease(e);
        }

        protected override void OnStatusChange(GUIElementStatus gUIElementStatus, params object[] parame)
        {
            if (!m_mouseHover && m_isclick)
            {
                m_isclick = false;
            }
            base.OnStatusChange(gUIElementStatus, parame);
        }

        protected override void OnDispose()
        {
            m_fontRender.Dispose();

            OnClick = null;

            m_fontRender = null;
            m_shader = null;
            OnClick = null;
            base.OnDispose();
        }

        public void SetText(string text)
        {
            m_fontRender?.SetText(text);
        }

        public void SetTextAling(TextAling textAling)
        {
            m_fontRender?.SetTextPivot(textAling);
        }

        public void SetTextColor(Color4 color)
        {
            m_fontRender?.SetColor(color);
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
            m_normalColor = normalColor;
            m_clickColor = clickColor;
            m_hoverColor = hoverColor;
            m_focusColor = focusColor;
        }

        public void SetButtomDisableColor(Color4 disableColor)
        {
            m_disableColor = disableColor;
        }

        /// <summary>
        /// this is for set the key, used to click, Default is the mouse left buttom
        /// </summary>
        public void SetMouseButtom(MouseButton mouseButton)
        {
            m_mouseKey = mouseButton;
        }
    }
}