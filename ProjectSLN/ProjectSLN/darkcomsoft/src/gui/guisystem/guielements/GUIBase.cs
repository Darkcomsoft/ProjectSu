using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using Projectsln.darkcomsoft.src.debug;
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
    /// <summary>
    /// Base of all GuiElements
    /// </summary>
    public abstract class GUIBase : ClassBase
    {
        protected RectangleF m_finalPosition;
        protected RectangleF m_startPosition;

        protected bool m_isEnabled = true;
        protected bool m_isInteractable = true;
        protected bool m_mouseHover = false;
        protected bool m_Focused = false;
        protected bool m_inputEnable = false;

        protected GUIDock m_dockType = GUIDock.Center;
        protected GUIPivot m_guiPivot = GUIPivot.Default;
        protected Matrix4 m_worldPosition;
        protected Matrix4 m_projection;

        public GUIBase()
        {
            m_startPosition = new RectangleF(0, 0, 50, 50);
            Resize();
            GUI.AddGUI(this);
        }

        public GUIBase(RectangleF positionSize)
        {
            m_startPosition = positionSize;
            Resize();
            GUI.AddGUI(this);
        }

        public GUIBase(RectangleF positionSize, GUIDock gUIDock)
        {
            m_startPosition = positionSize;
            m_dockType = gUIDock;

            Resize();
            GUI.AddGUI(this);
        }

        public GUIBase(RectangleF positionSize, GUIDock gUIDock, GUIPivot gUIPivot)
        {
            m_startPosition = positionSize;
            m_dockType = gUIDock;
            m_guiPivot = gUIPivot;

            Resize();
            GUI.AddGUI(this);
        }

        public void Tick()
        {
            if (!m_isEnabled) { return; }
            OnTick();
        }

        public void Draw()
        {
            if (!m_isEnabled) { return; }
            UseUniforms();
            OnDraw();
        }

        protected override void OnDispose()
        {
            GUI.RemoveGUI(this);
            base.OnDispose();
        }

        public void Resize()
        {
            OnResize(false);
            m_projection = Matrix4.CreateOrthographicOffCenter(0.0f, WindowMain.Instance.Width, 0.0f, WindowMain.Instance.Height, -1.0f, 1.0f);
            UpdateTransform();
            OnResize(true);
            Refresh();
        }

        #region TrnasformStuff
        private void UpdateTransform()
        {
            if (GameSettings.GuiScale <= 0) { GameSettings.GuiScale = 1; }

            m_finalPosition.Width = m_startPosition.Width + GameSettings.GuiScale;
            m_finalPosition.Height = m_startPosition.Height + GameSettings.GuiScale;

            if (m_guiPivot == GUIPivot.Default)//If is default, use the preset pivot, down below
            {
                switch (m_dockType)
                {
                    case GUIDock.Free:
                        m_finalPosition.X = m_startPosition.X - (m_finalPosition.Width / 2);
                        m_finalPosition.Y = m_startPosition.Y - (m_finalPosition.Height / 2);
                        break;
                    case GUIDock.Center:
                        m_finalPosition.X = (WindowMain.Instance.Width / 2) - (m_finalPosition.Width / 2);
                        m_finalPosition.Y = (WindowMain.Instance.Height / 2) - (m_finalPosition.Height / 2);
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
                        m_finalPosition.X = (WindowMain.Instance.Width / 2) - (m_finalPosition.Width / 2);
                        m_finalPosition.Y = 0;
                        break;
                    case GUIDock.Bottom:
                        m_finalPosition.X = (WindowMain.Instance.Width / 2) - (m_finalPosition.Width / 2);
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
            }
            else//if not default, use pivot set
            {
                switch (m_dockType)
                {
                    case GUIDock.Free:
                        m_finalPosition.X = m_startPosition.X;
                        m_finalPosition.Y = m_startPosition.Y;
                        break;
                    case GUIDock.Center:
                        m_finalPosition.X = WindowMain.Instance.Width / 2;
                        m_finalPosition.Y = WindowMain.Instance.Height / 2;
                        break;
                    case GUIDock.Left:
                        m_finalPosition.X = 0;
                        m_finalPosition.Y = WindowMain.Instance.Height / 2;
                        break;
                    case GUIDock.Right:
                        m_finalPosition.X = WindowMain.Instance.Width;
                        m_finalPosition.Y = WindowMain.Instance.Height / 2;
                        break;
                    case GUIDock.Top:
                        m_finalPosition.X = WindowMain.Instance.Width / 2;
                        m_finalPosition.Y = 0;
                        break;
                    case GUIDock.Bottom:
                        m_finalPosition.X = WindowMain.Instance.Width / 2;
                        m_finalPosition.Y = WindowMain.Instance.Height;
                        break;
                    case GUIDock.LeftTop:
                        m_finalPosition.X = 0;
                        m_finalPosition.Y = 0;
                        break;
                    case GUIDock.LeftBottom:
                        m_finalPosition.X = 0;
                        m_finalPosition.Y = WindowMain.Instance.Height;
                        break;
                    case GUIDock.RightTop:
                        m_finalPosition.X = WindowMain.Instance.Width;
                        m_finalPosition.Y = 0;
                        break;
                    case GUIDock.RightBottom:
                        m_finalPosition.X = WindowMain.Instance.Width;
                        m_finalPosition.Y = WindowMain.Instance.Height;
                        break;
                    default:
                        m_finalPosition.X = m_startPosition.X;
                        m_finalPosition.Y = m_startPosition.Y;
                        break;
                }

                UpdatePivot();//Calaculate the guii pivot, if is using default pivot this dont do nothing
            }

            m_finalPosition.X += m_startPosition.X;
            m_finalPosition.Y += m_startPosition.Y;

            m_worldPosition = Matrix4.CreateScale(m_finalPosition.Width / 2, m_finalPosition.Height / 2, 0) * Matrix4.CreateTranslation(m_finalPosition.X + m_finalPosition.Width / 2, m_finalPosition.Y + m_finalPosition.Height / 2, 0);
        }

        private void UpdatePivot()
        {
            switch (m_guiPivot)
            {
                case GUIPivot.Center:
                    m_finalPosition.X = m_finalPosition.X - (m_finalPosition.Width / 2);
                    m_finalPosition.Y = m_finalPosition.Y - (m_finalPosition.Height / 2);
                    break;
                case GUIPivot.Left:
                    m_finalPosition.X = m_finalPosition.X - 0;
                    m_finalPosition.Y = m_finalPosition.Y - (m_finalPosition.Width / 2);
                    break;
                case GUIPivot.Right:
                    m_finalPosition.X = m_finalPosition.X - (m_finalPosition.Width);
                    m_finalPosition.Y = m_finalPosition.Y - (m_finalPosition.Height / 2);
                    break;
                case GUIPivot.Top:
                    m_finalPosition.X = m_finalPosition.X - (m_finalPosition.Width / 2);
                    m_finalPosition.Y = m_finalPosition.Y - 0;
                    break;
                case GUIPivot.Bottom:
                    m_finalPosition.X = m_finalPosition.X - (m_finalPosition.Width / 2);
                    m_finalPosition.Y = m_finalPosition.Y - (m_finalPosition.Height);
                    break;
                case GUIPivot.LeftTop:
                    m_finalPosition.X = m_finalPosition.X - 0;
                    m_finalPosition.Y = m_finalPosition.Y - 0;
                    break;
                case GUIPivot.LeftBottom:
                    m_finalPosition.X = m_finalPosition.X - 0;
                    m_finalPosition.Y = m_finalPosition.Y - (m_finalPosition.Height);
                    break;
                case GUIPivot.RightTop:
                    m_finalPosition.X = m_finalPosition.X - (m_finalPosition.Width);
                    m_finalPosition.Y = m_finalPosition.Y - 0;
                    break;
                case GUIPivot.RightBottom:
                    m_finalPosition.X = m_finalPosition.X - (m_finalPosition.Width);
                    m_finalPosition.Y = m_finalPosition.Y - (m_finalPosition.Height);
                    break;
            }
        }
        #endregion

        public void Dock(GUIDock gUIDock)
        {
            m_dockType = gUIDock;
            Resize();
        }

        public void Pivot(GUIPivot gUIPivot)
        {
            m_guiPivot = gUIPivot;
            Resize();
        }


        public void SetPosition(float x, float y)
        {
            m_startPosition.X = x;
            m_startPosition.Y = y;

            Resize();
        }

        public void AddPosition(float x, float y)
        {
            m_startPosition.X += x;
            m_startPosition.Y += y;

            Resize();
        }

        public void RemovePosition(float x, float y)
        {
            m_startPosition.X -= x;
            m_startPosition.Y -= y;

            Resize();
        }

        public void SetSize(float w, float h)
        {
            m_startPosition.Width = w;
            m_startPosition.Height = h;

            Resize();
        }

        public void AddSize(float w, float h)
        {
            m_startPosition.Width += w;
            m_startPosition.Height += h;

            Resize();
        }

        public void RemoveSize(float w, float h)
        {
            m_startPosition.Width -= w;
            m_startPosition.Height -= h;

            Resize();
        }


        public void Enable()
        {
            m_isEnabled = true;
            GUI.instance.s_EnableGUI(this);
            OnEnable();
        }

        public void Disable()
        {
            m_isEnabled = false;
            GUI.instance.s_DisableGUI(this);
            OnDisable();
        }

        public void Interact()
        {
            m_isInteractable = true;
            OnInteractable(true);
        }

        public void NoInteract()
        {
            m_isInteractable = false;
            OnInteractable(false);
        }

        public void UseUniforms()
        {
            OnUseUniforms();
        }

        public void StopUseUniforms()
        {
            OnStopUseUniforms();
        }

        public void SetStatus(GUIElementStatus gUIElementStatus, params object[] parame)
        {
            switch (gUIElementStatus)
            {
                case GUIElementStatus.none:
                    break;
                case GUIElementStatus.Hover:
                    m_mouseHover = (bool)parame[0];
                    break;
                case GUIElementStatus.Focus:
                    m_Focused = (bool)parame[0];
                    break;
            }

            OnStatusChange(gUIElementStatus, parame);
        }

        public void MouseClick(MouseButtonEventArgs e)
        {
            OnMouseClick(e);
        }

        public void MouseRelease(MouseButtonEventArgs e)
        {
            OnMouseRelease(e);
        }

        private void Refresh()
        {
            GUI.TickMouseHover();
        }

        protected virtual void OnTick() { }
        protected virtual void OnDraw() { }
        protected virtual void OnResize(bool after) { }
        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
        protected virtual void OnInteractable(bool isInteractable) { }
        protected virtual void OnStatusChange(GUIElementStatus gUIElementStatus, params object[] parame) { }
        protected virtual void OnUseUniforms() { }
        protected virtual void OnStopUseUniforms() { }
        protected virtual void OnMouseClick(MouseButtonEventArgs e) { }
        protected virtual void OnMouseRelease(MouseButtonEventArgs e) { }

        public Matrix4 GetWorldMatrix { get { return m_worldPosition; } }
        public Matrix4 GetProjectionMatrix { get { return m_projection; } }
        public RectangleF GetFinalPosition { get { return m_finalPosition; } }

        public bool IsMouseOn() { return m_finalPosition.IntersectsWith(Input.GetMousePositionRec); }
        public bool isEnabled { get { return m_isEnabled; } }
        public bool isInteractable { get { return m_isInteractable; } }
        public bool isMouseHover { get { return m_mouseHover; } }
        public bool isFocused { get { return m_Focused; } }
        public bool isInputEnabled { get { return m_inputEnable; } }
    }
}