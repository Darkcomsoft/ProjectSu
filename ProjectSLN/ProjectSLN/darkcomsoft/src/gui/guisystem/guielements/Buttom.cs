using OpenTK.Mathematics;
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

        public Buttom()
        {
            m_shader = ResourcesManager.GetShader("UI");
        }

        public Buttom(RectangleF positionSize) : base(positionSize)
        {
            m_shader = ResourcesManager.GetShader("UI");
        }

        public Buttom(RectangleF positionSize, GUIDock gUIDock) : base(positionSize, gUIDock)
        {
            m_shader = ResourcesManager.GetShader("UI");
        }

        public Buttom(RectangleF positionSize, GUIDock gUIDock, GUIPivot gUIPivot): base(positionSize, gUIDock, gUIPivot)
        {
            m_shader = ResourcesManager.GetShader("UI");
        }

        protected override void OnTick()
        {
            
            base.OnTick();
        }

        protected override void OnMouseStateUpdate(GUIMouseState gUIMouseState)
        {
            switch (gUIMouseState)
            {
                case GUIMouseState.Hover:
                    break;
                case GUIMouseState.UnHover:
                    break;
                case GUIMouseState.Click:
                    Debug.Log("Buttom Click!");
                    break;
                case GUIMouseState.ClickRelease:
                    Debug.Log("Buttom ClickRelease!");
                    break;
                case GUIMouseState.Focus:
                    break;
                case GUIMouseState.UnFocus:
                    break;
            }
            base.OnMouseStateUpdate(gUIMouseState);
        }

        protected override void OnDraw()
        {
            m_shader.Use();

            m_shader.Set("world", m_worldPosition);
            m_shader.Set("projection", m_projection);

            if (isMouseHover)
            {
                m_shader.Set("uicolor", Color4.Blue);
            }
            else
            {
                m_shader.Set("uicolor", Color4.Yellow);
            }

            GUI.instance.DrawRec(this);
            base.OnDraw();
        }

        protected override void OnDispose()
        {
            m_shader = null;
            base.OnDispose();
        }
    }
}
