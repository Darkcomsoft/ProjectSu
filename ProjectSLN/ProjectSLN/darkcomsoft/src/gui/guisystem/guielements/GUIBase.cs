using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.engine.window;
using Projectsln.darkcomsoft.src.enums;
using Projectsln.darkcomsoft.src.render;
using Projectsln.darkcomsoft.src.resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Projectsln.darkcomsoft.src.gui.guisystem.guielements
{
    public class GUIBase : ClassBase
    {
        private RectangleF m_finalPosition;
        private RectangleF m_startPosition;

        private bool m_isEnabled = true;
        private bool m_isInteractable = true;
        private bool m_mouseHover = false;

        private GUIDock m_dockType = GUIDock.RightBottom;
        private Shader m_shader;
        private Matrix4 worldPosition;


        public GUIBase()
        {
            m_startPosition = new RectangleF(0, 0, 50, 50);
            m_shader = ResourcesManager.GetShader("UI");

            OnResize();
        }

        public GUIBase(RectangleF positionSize)
        {
            m_startPosition = positionSize;
            m_shader = ResourcesManager.GetShader("UI");

            OnResize();
        }

        public GUIBase(RectangleF positionSize, GUIDock gUIDock)
        {
            m_startPosition = positionSize;
            m_shader = ResourcesManager.GetShader("UI");

            m_dockType = gUIDock;

            OnResize();
        }

        public void Tick()
        {
            if (!m_isEnabled) { return; }

            if (m_finalPosition.IntersectsWith(Input.GetMousePositionRec))
            {
                m_mouseHover = true;
            }
            else {
                m_mouseHover = false;
            }
        }

        public void Draw()
        {
            if (!m_isEnabled) { return; }

            m_shader.Use();

            m_shader.Set("world", worldPosition);
            m_shader.Set("projection", Matrix4.CreateOrthographicOffCenter(0.0f, WindowMain.Instance.Width, WindowMain.Instance.Height,0.0f, -1.0f, 1.0f));
         
            if (m_mouseHover)
            {
                m_shader.Set("uicolor", Color4.Blue);
            }
            else
            {
                m_shader.Set("uicolor", Color4.Yellow);
            }
        }

        protected override void OnDispose()
        {
            m_shader = null;
            base.OnDispose();
        }

        public virtual void OnResize()
        {
            RefreshTrnasform();
        }

        private void RefreshTrnasform()
        {
            switch (m_dockType)
            {
                case GUIDock.Free:
                    m_finalPosition.X = m_startPosition.X;
                    m_finalPosition.Y = m_startPosition.Y;

                    m_finalPosition.Width = m_startPosition.Width;
                    m_finalPosition.Height = m_startPosition.Height;
                    break;
                case GUIDock.Left:
                    break;
                case GUIDock.Right:
                    break;
                case GUIDock.Top:
                    break;
                case GUIDock.Bottom:
                    break;
                case GUIDock.LeftTop:
                    break;
                case GUIDock.LeftBottom:
                    break;
                case GUIDock.RightTop:
                    break;
                case GUIDock.RightBottom:
                    m_finalPosition.X = (WindowMain.Instance.Width) - m_finalPosition.Width;
                    m_finalPosition.Y = (WindowMain.Instance.Height) - m_finalPosition.Height;

                    m_finalPosition.Width = m_startPosition.Width;
                    m_finalPosition.Height = m_startPosition.Height;
                    break;
                default:
                    break;
            }

            worldPosition = Matrix4.CreateScale(m_finalPosition.Width / 2, m_finalPosition.Height / 2, 0) * Matrix4.CreateTranslation(m_finalPosition.X + m_finalPosition.Width /2, m_finalPosition.Y + m_finalPosition.Height /2, 0);
        }

        public void Dock(GUIDock gUIDock)
        {
            m_dockType = gUIDock;

            OnResize();
        }

        public void Enable()
        {
            m_isEnabled = true;
            GUI.instance.s_EnableGUI(this);
        }

        public void Disable()
        {
            m_isEnabled = false;
            GUI.instance.s_DisableGUI(this);
        }

        public bool isEnabled { get { return m_isEnabled; } }
    }
}
