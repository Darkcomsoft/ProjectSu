using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.enums;
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

        public Buttom()
        {
            m_shader = ResourcesManager.GetShader("UI");
            m_inputEnable = true;
        }

        public Buttom(RectangleF positionSize) : base(positionSize)
        {
            m_shader = ResourcesManager.GetShader("UI");
            m_inputEnable = true;
        }

        public Buttom(RectangleF positionSize, GUIDock gUIDock) : base(positionSize, gUIDock)
        {
            m_shader = ResourcesManager.GetShader("UI");
            m_inputEnable = true;
        }

        public Buttom(RectangleF positionSize, GUIDock gUIDock, GUIPivot gUIPivot): base(positionSize, gUIDock, gUIPivot)
        {
            m_shader = ResourcesManager.GetShader("UI");
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

            if (isMouseHover)
            {
                if (m_isclick)
                {
                    m_shader.Set("uicolor", Color4.DarkOliveGreen);
                }
                else
                {
                    m_shader.Set("uicolor", Color4.Blue);
                }
            }
            else
            {
                if (m_Focused)
                {
                    m_shader.Set("uicolor", Color4.Bisque);
                }
                else
                {
                    m_shader.Set("uicolor", Color4.Yellow);
                }
            }

            GUI.instance.DrawRec(this);
            base.OnDraw();
        }

        protected override void OnMouseClick(MouseButtonEventArgs e)
        {
            m_isclick = true;
            AddSize(10, 10);
            base.OnMouseClick(e);
        }

        protected override void OnMouseRelease(MouseButtonEventArgs e)
        {
            m_isclick = false;
            RemoveSize(10,10);
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
            m_shader = null;
            base.OnDispose();
        }
    }
}
