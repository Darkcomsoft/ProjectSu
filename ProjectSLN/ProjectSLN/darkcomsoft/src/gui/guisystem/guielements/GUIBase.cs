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
        private Rectangle m_finalPosition;
        private Rectangle m_startPosition;

        private bool m_isEnabled = true;
        private bool m_isInteractable = true;

        private GUIDock m_dockType = GUIDock.RightBottom;

        private bool m_mouseHover = false;

        private Shader m_shader;

        private Matrix4 worldPosition;


        public GUIBase()
        {
            m_startPosition = new Rectangle(0, 0, 50, 50);
            m_shader = ResourcesManager.GetShader("UI");

            OnResize();
        }

        public GUIBase(Rectangle positionSize)
        {
            m_startPosition = positionSize;
            m_shader = ResourcesManager.GetShader("UI");

            OnResize();
        }

        public void Tick()
        {
            if (m_finalPosition.IntersectsWith(Input.GetMousePositionRec))
            {
                m_mouseHover = true;
            }
            else {
                m_mouseHover = false;
            }

            Debug.Log(Input.GetMousePositionRec.ToString());
        }

        public void Draw()
        {
            m_shader.Use();

            m_shader.Set("world", worldPosition);
            m_shader.Set("projection", Matrix4.CreateOrthographic(WindowMain.Instance.Width, WindowMain.Instance.Height, 0f, 5.0f));
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
                    m_finalPosition.X = (WindowMain.Instance.Width / 2) - m_finalPosition.Width;
                    m_finalPosition.Y = (WindowMain.Instance.Height / 2) - m_finalPosition.Height;

                    m_finalPosition.Width = m_startPosition.Width;
                    m_finalPosition.Height = m_startPosition.Height;
                    break;
                default:
                    break;
            }

            worldPosition = Matrix4.CreateScale(m_finalPosition.Width, m_finalPosition.Height, 0) * Matrix4.CreateTranslation(m_finalPosition.X, -m_finalPosition.Y, 0);
        }

        public void Enable()
        {
            m_isEnabled = true;
        }

        public void Disable()
        {
            m_isEnabled = false;
        }
    }
}
