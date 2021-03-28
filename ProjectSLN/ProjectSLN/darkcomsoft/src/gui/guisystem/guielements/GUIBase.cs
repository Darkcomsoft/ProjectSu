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
        private bool m_Focused = false;

        private Shader m_shader;
        private GUIDock m_dockType = GUIDock.RightBottom;
        private GUIPivot m_guiPivot = GUIPivot.Default;
        private Matrix4 m_worldPosition;
        private Matrix4 m_projection;


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

            /*if (m_finalPosition.IntersectsWith(Input.GetMousePositionRec))
            {
                m_mouseHover = true;
            }
            else {
                m_mouseHover = false;
            }*/
        }

        public void Draw()
        {
            if (!m_isEnabled) { return; }

            m_shader.Use();

            m_shader.Set("world", m_worldPosition);
            m_shader.Set("projection", m_projection);

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
            m_projection = Matrix4.CreateOrthographicOffCenter(0.0f, WindowMain.Instance.Width, WindowMain.Instance.Height, 0.0f, -1.0f, 1.0f);
            RefreshTrnasform();
        }

        private void RefreshTrnasform()
        {
            if (GameSettings.GuiScale <= 0) { GameSettings.GuiScale = 1; }

            m_finalPosition.Width = m_startPosition.Width + GameSettings.GuiScale;
            m_finalPosition.Height = m_startPosition.Height + GameSettings.GuiScale;

            switch (m_dockType)
            {
                case GUIDock.Free:
                    m_finalPosition.X = m_startPosition.X;
                    m_finalPosition.Y = m_startPosition.Y;
                    break;
                case GUIDock.Center:
                    m_finalPosition.X = (WindowMain.Instance.Width /2) - (m_finalPosition.Width / 2);
                    m_finalPosition.Y = (WindowMain.Instance.Height/2) - (m_finalPosition.Height /2);
                    break;
                case GUIDock.Left:
                    m_finalPosition.X = 0;
                    m_finalPosition.Y = (WindowMain.Instance.Height / 2) - (m_finalPosition.Width / 2);
                    break;
                case GUIDock.Right:
                    m_finalPosition.X = (WindowMain.Instance.Width) - m_finalPosition.Width;
                    m_finalPosition.Y = (WindowMain.Instance.Height / 2) - (m_finalPosition.Height / 2);
                    break;
                case GUIDock.Top:
                    m_finalPosition.X = (WindowMain.Instance.Width / 2) - (m_finalPosition.Width /2);
                    m_finalPosition.Y = 0;
                    break;
                case GUIDock.Bottom:
                    m_finalPosition.X = (WindowMain.Instance.Width/2) - (m_finalPosition.Width / 2);
                    m_finalPosition.Y = (WindowMain.Instance.Height) - m_finalPosition.Height;
                    break;
                case GUIDock.LeftTop:
                    m_finalPosition.X = 0;
                    m_finalPosition.Y = 0;
                    break;
                case GUIDock.LeftBottom:
                    m_finalPosition.X = 0;
                    m_finalPosition.Y = (WindowMain.Instance.Height) - m_finalPosition.Height;
                    break;
                case GUIDock.RightTop:
                    m_finalPosition.X = (WindowMain.Instance.Width) - m_finalPosition.Width;
                    m_finalPosition.Y = 0;
                    break;
                case GUIDock.RightBottom:
                    m_finalPosition.X = (WindowMain.Instance.Width) - m_finalPosition.Width;
                    m_finalPosition.Y = (WindowMain.Instance.Height) - m_finalPosition.Height;
                    break;
                default:
                    m_finalPosition.X = m_startPosition.X;
                    m_finalPosition.Y = m_startPosition.Y;
                    break;
            }

            m_worldPosition = Matrix4.CreateScale(m_finalPosition.Width / 2, m_finalPosition.Height / 2, 0) * Matrix4.CreateTranslation(m_finalPosition.X + m_finalPosition.Width / 2, m_finalPosition.Y + m_finalPosition.Height / 2, 0);
        }

        public void Dock(GUIDock gUIDock)
        {
            m_dockType = gUIDock;
            OnResize();
        }

        public void Pivot(GUIPivot gUIPivot)
        {
            m_guiPivot = gUIPivot;
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

        public void Interact()
        {
            m_isInteractable = true;
        }

        public void NoInteract()
        {
            m_isInteractable = false;
        }

        public void Hover()
        {
            m_mouseHover = true;
        }

        public void UnHover()
        {
            m_mouseHover = false;
        }

        public bool IsMouseOn() { return m_finalPosition.IntersectsWith(Input.GetMousePositionRec); }
        public bool isEnabled { get { return m_isEnabled; } }
        public bool isInteractable { get { return m_isInteractable; } }
    }
}
