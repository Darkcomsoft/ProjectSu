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
    /// Buttom is a simple gui buttom
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
            m_fontRender = new FontRender("Text Teste ?!*:;@%#$%R0123456789", 50, 100, new RectangleF(0,0,100,100), this,ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            m_inputEnable = true;
        }

        public Buttom(RectangleF positionSize) : base(positionSize)
        {
            m_shader = ResourcesManager.GetShader("UI");
            m_fontRender = new FontRender("Text Teste ?!*:;@%#$%R0123456789", 50, 100, new RectangleF(0, 0, 100, 100), this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            m_inputEnable = true;
        }

        public Buttom(RectangleF positionSize, GUIDock gUIDock) : base(positionSize, gUIDock)
        {
            m_shader = ResourcesManager.GetShader("UI");
            m_fontRender = new FontRender("Text Teste ?!*:;@%#$%R0123456789", 50, 100, new RectangleF(0, 0, 100, 100), this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            m_inputEnable = true;
        }

        public Buttom(RectangleF positionSize, GUIDock gUIDock, GUIPivot gUIPivot): base(positionSize, gUIDock, gUIPivot)
        {
            m_shader = ResourcesManager.GetShader("UI");
            m_fontRender = new FontRender("Text Teste ?!*:;@%#$%R0123456789", 50, 100, new RectangleF(0, 0, 100, 100), this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            m_inputEnable = true;
        }

        protected override void OnTick()
        {
            
            base.OnTick();
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

            m_fontRender.Draw();
            base.OnDraw();
        }

        protected override void OnResize(bool after)
        {
            
            base.OnResize(after);
        }

        protected override void OnMouseClick(MouseButtonEventArgs e)
        {
            if (e.Button == m_mouseKey)
            {
                m_isclick = true;
                AddSize(10, 10);
                OnClick?.Invoke();
            }
            
            base.OnMouseClick(e);
        }

        protected override void OnMouseRelease(MouseButtonEventArgs e)
        {
            if (e.Button == m_mouseKey)
            {
                m_isclick = false;
                RemoveSize(10, 10);
            }
            base.OnMouseRelease(e);
        }

        protected override void OnStatusChange(GUIElementStatus gUIElementStatus, params object[] parame)
        {
            if (!m_mouseHover && m_isclick)
            {
                m_isclick = false;
                RemoveSize(10, 10);
            }
            base.OnStatusChange(gUIElementStatus, parame);
        }

        protected override void OnDispose()
        {
            m_fontRender.Dispose();

            m_fontRender = null;
            m_shader = null;
            OnClick = null;
            base.OnDispose();
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
