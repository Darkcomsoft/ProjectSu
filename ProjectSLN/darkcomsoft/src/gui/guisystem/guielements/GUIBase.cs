using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using ProjectIND.darkcomsoft.src.client;
using ProjectIND.darkcomsoft.src.debug;
using ProjectIND.darkcomsoft.src.engine;
using ProjectIND.darkcomsoft.src.engine.window;
using ProjectIND.darkcomsoft.src.enums;
using ProjectIND.darkcomsoft.src.render;
using ProjectIND.darkcomsoft.src.resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ProjectIND.darkcomsoft.src.gui.guisystem.guielements
{
    /// <summary>
    /// Base of all GuiElements
    /// </summary>
    public abstract class GUIBase : ClassBase
    {
        protected RectangleF v_finalPosition;
        protected RectangleF v_startPosition;

        protected bool v_isEnabled = true;
        protected bool v_isInteractable = true;
        protected bool v_mouseHover = false;
        protected bool v_Focused = false;
        protected bool v_inputEnable = false;

        protected GUIDock v_dockType = GUIDock.Center;
        protected GUIPivot v_guiPivot = GUIPivot.Default;
        protected Matrix4 v_worldPosition;
        protected Matrix4 v_projection;

        public GUIBase()
        {
            v_startPosition = new RectangleF(0, 0, 50, 50);
            Resize();
            GUI.AddGUI(this);
        }

        public GUIBase(RectangleF positionSize)
        {
            v_startPosition = positionSize;
            Resize();
            GUI.AddGUI(this);
        }

        public GUIBase(RectangleF positionSize, GUIDock gUIDock)
        {
            v_startPosition = positionSize;
            v_dockType = gUIDock;

            Resize();
            GUI.AddGUI(this);
        }

        public GUIBase(RectangleF positionSize, GUIDock gUIDock, GUIPivot gUIPivot)
        {
            v_startPosition = positionSize;
            v_dockType = gUIDock;
            v_guiPivot = gUIPivot;

            Resize();
            GUI.AddGUI(this);
        }

        public void Tick()
        {
            if (!v_isEnabled) { return; }
            OnTick();
        }

        public void Draw()
        {
            if (!v_isEnabled) { return; }
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
            v_projection = Matrix4.CreateOrthographicOffCenter(0.0f, WindowMain.Instance.Width, 0.0f, WindowMain.Instance.Height, -1.0f, 1.0f);
            UpdateTransform();
            OnResize(true);
            Refresh();
        }

        #region TrnasformStuff
        private void UpdateTransform()
        {
            v_finalPosition.Width = v_startPosition.Width * GameSettings.GuiScale;
            v_finalPosition.Height = v_startPosition.Height * GameSettings.GuiScale;

            if (v_guiPivot == GUIPivot.Default)//If is default, use the preset pivot, down below
            {
                switch (v_dockType)
                {
                    case GUIDock.Free:
                        v_finalPosition.X = v_startPosition.X - (v_finalPosition.Width / 2);
                        v_finalPosition.Y = v_startPosition.Y - (v_finalPosition.Height / 2);
                        break;
                    case GUIDock.Center:
                        v_finalPosition.X = (WindowMain.Instance.Width / 2) - (v_finalPosition.Width / 2);
                        v_finalPosition.Y = (WindowMain.Instance.Height / 2) - (v_finalPosition.Height / 2);
                        break;
                    case GUIDock.Left:
                        v_finalPosition.X = 0;
                        v_finalPosition.Y = (WindowMain.Instance.Height / 2) - (v_finalPosition.Width / 2);
                        break;
                    case GUIDock.Right:
                        v_finalPosition.X = (WindowMain.Instance.Width) - v_finalPosition.Width;
                        v_finalPosition.Y = (WindowMain.Instance.Height / 2) - (v_finalPosition.Height / 2);
                        break;
                    case GUIDock.Bottom:
                        v_finalPosition.X = (WindowMain.Instance.Width / 2) - (v_finalPosition.Width / 2);
                        v_finalPosition.Y = 0;
                        break;
                    case GUIDock.Top:
                        v_finalPosition.X = (WindowMain.Instance.Width / 2) - (v_finalPosition.Width / 2);
                        v_finalPosition.Y = (WindowMain.Instance.Height) - v_finalPosition.Height;
                        break;
                    case GUIDock.LeftBottom:
                        v_finalPosition.X = 0;
                        v_finalPosition.Y = 0;
                        break;
                    case GUIDock.LeftTop:
                        v_finalPosition.X = 0;
                        v_finalPosition.Y = (WindowMain.Instance.Height) - v_finalPosition.Height;
                        break;
                    case GUIDock.RightBottom:
                        v_finalPosition.X = (WindowMain.Instance.Width) - v_finalPosition.Width;
                        v_finalPosition.Y = 0;
                        break;
                    case GUIDock.RightTop:
                        v_finalPosition.X = (WindowMain.Instance.Width) - v_finalPosition.Width;
                        v_finalPosition.Y = (WindowMain.Instance.Height) - v_finalPosition.Height;
                        break;
                    default:
                        v_finalPosition.X = v_startPosition.X;
                        v_finalPosition.Y = v_startPosition.Y;
                        break;
                }
            }
            else//if not default, use pivot set
            {
                switch (v_dockType)
                {
                    case GUIDock.Free:
                        v_finalPosition.X = v_startPosition.X;
                        v_finalPosition.Y = v_startPosition.Y;
                        break;
                    case GUIDock.Center:
                        v_finalPosition.X = WindowMain.Instance.Width / 2;
                        v_finalPosition.Y = WindowMain.Instance.Height / 2;
                        break;
                    case GUIDock.Left:
                        v_finalPosition.X = 0;
                        v_finalPosition.Y = WindowMain.Instance.Height / 2;
                        break;
                    case GUIDock.Right:
                        v_finalPosition.X = WindowMain.Instance.Width;
                        v_finalPosition.Y = WindowMain.Instance.Height / 2;
                        break;
                    case GUIDock.Bottom:
                        v_finalPosition.X = WindowMain.Instance.Width / 2;
                        v_finalPosition.Y = 0;
                        break;
                    case GUIDock.Top:
                        v_finalPosition.X = WindowMain.Instance.Width / 2;
                        v_finalPosition.Y = WindowMain.Instance.Height;
                        break;
                    case GUIDock.LeftBottom:
                        v_finalPosition.X = 0;
                        v_finalPosition.Y = 0;
                        break;
                    case GUIDock.LeftTop:
                        v_finalPosition.X = 0;
                        v_finalPosition.Y = WindowMain.Instance.Height;
                        break;
                    case GUIDock.RightBottom:
                        v_finalPosition.X = WindowMain.Instance.Width;
                        v_finalPosition.Y = 0;
                        break;
                    case GUIDock.RightTop:
                        v_finalPosition.X = WindowMain.Instance.Width;
                        v_finalPosition.Y = WindowMain.Instance.Height;
                        break;
                    default:
                        v_finalPosition.X = v_startPosition.X;
                        v_finalPosition.Y = v_startPosition.Y;
                        break;
                }

                UpdatePivot();//Calaculate the guii pivot, if is using default pivot this dont do nothing
            }

            ApplyStartPosition();// Apply the start position seted when GuiElement instance is created

            v_worldPosition = Matrix4.CreateScale(v_finalPosition.Width / 2, v_finalPosition.Height / 2, 0) * Matrix4.CreateTranslation(v_finalPosition.X + v_finalPosition.Width / 2, v_finalPosition.Y + v_finalPosition.Height / 2, 0);
        }

        private void ApplyStartPosition()
        {
            switch (v_dockType)
            {
                case GUIDock.Center:
                    v_finalPosition.X += v_startPosition.X;
                    v_finalPosition.Y += v_startPosition.Y;
                    break;
                case GUIDock.Left:
                    v_finalPosition.X += v_startPosition.X;
                    v_finalPosition.Y += v_startPosition.Y;
                    break;
                case GUIDock.Right:
                    v_finalPosition.X -= v_startPosition.X;
                    v_finalPosition.Y += v_startPosition.Y;
                    break;
                case GUIDock.Top:
                    v_finalPosition.X += v_startPosition.X;
                    v_finalPosition.Y -= v_startPosition.Y;
                    break;
                case GUIDock.Bottom:
                    v_finalPosition.X += v_startPosition.X;
                    v_finalPosition.Y += v_startPosition.Y;
                    break;
                case GUIDock.LeftTop:
                    v_finalPosition.X += v_startPosition.X;
                    v_finalPosition.Y -= v_startPosition.Y;
                    break;
                case GUIDock.LeftBottom:
                    v_finalPosition.X += v_startPosition.X;
                    v_finalPosition.Y += v_startPosition.Y;
                    break;
                case GUIDock.RightTop:
                    v_finalPosition.X -= v_startPosition.X;
                    v_finalPosition.Y -= v_startPosition.Y;
                    break;
                case GUIDock.RightBottom:
                    v_finalPosition.X -= v_startPosition.X;
                    v_finalPosition.Y += v_startPosition.Y;
                    break;
            }
        }

        private void UpdatePivot()
        {
            switch (v_guiPivot)
            {
                case GUIPivot.Center:
                    v_finalPosition.X = v_finalPosition.X - (v_finalPosition.Width / 2);
                    v_finalPosition.Y = v_finalPosition.Y - (v_finalPosition.Height / 2);
                    break;
                case GUIPivot.Left:
                    v_finalPosition.X = v_finalPosition.X - 0;
                    v_finalPosition.Y = v_finalPosition.Y - (v_finalPosition.Width / 2);
                    break;
                case GUIPivot.Right:
                    v_finalPosition.X = v_finalPosition.X - (v_finalPosition.Width);
                    v_finalPosition.Y = v_finalPosition.Y - (v_finalPosition.Height / 2);
                    break;
                case GUIPivot.Bottom:
                    v_finalPosition.X = v_finalPosition.X - (v_finalPosition.Width / 2);
                    v_finalPosition.Y = v_finalPosition.Y - 0;
                    break;
                case GUIPivot.Top:
                    v_finalPosition.X = v_finalPosition.X - (v_finalPosition.Width / 2);
                    v_finalPosition.Y = v_finalPosition.Y - (v_finalPosition.Height);
                    break;
                case GUIPivot.LeftBottom:
                    v_finalPosition.X = v_finalPosition.X - 0;
                    v_finalPosition.Y = v_finalPosition.Y - 0;
                    break;
                case GUIPivot.LeftTop:
                    v_finalPosition.X = v_finalPosition.X - 0;
                    v_finalPosition.Y = v_finalPosition.Y - (v_finalPosition.Height);
                    break;
                case GUIPivot.RightBottom:
                    v_finalPosition.X = v_finalPosition.X - (v_finalPosition.Width);
                    v_finalPosition.Y = v_finalPosition.Y - 0;
                    break;
                case GUIPivot.RightTop:
                    v_finalPosition.X = v_finalPosition.X - (v_finalPosition.Width);
                    v_finalPosition.Y = v_finalPosition.Y - (v_finalPosition.Height);
                    break;
            }
        }
        #endregion

        public void Dock(GUIDock gUIDock)
        {
            v_dockType = gUIDock;
            Resize();
        }

        public void Pivot(GUIPivot gUIPivot)
        {
            v_guiPivot = gUIPivot;
            Resize();
        }


        public void SetPosition(float x, float y)
        {
            v_startPosition.X = x;
            v_startPosition.Y = y;

            Resize();
        }

        public void AddPosition(float x, float y)
        {
            v_startPosition.X += x;
            v_startPosition.Y += y;

            Resize();
        }

        public void RemovePosition(float x, float y)
        {
            v_startPosition.X -= x;
            v_startPosition.Y -= y;

            Resize();
        }

        public void SetSize(float w, float h)
        {
            v_startPosition.Width = w;
            v_startPosition.Height = h;

            Resize();
        }

        public void AddSize(float w, float h)
        {
            v_startPosition.Width += w;
            v_startPosition.Height += h;

            Resize();
        }

        public void RemoveSize(float w, float h)
        {
            v_startPosition.Width -= w;
            v_startPosition.Height -= h;

            Resize();
        }


        public void Enable()
        {
            v_isEnabled = true;
            GUI.instance.s_EnableGUI(this);
            OnEnable();
        }

        public void Disable()
        {
            v_isEnabled = false;
            GUI.instance.s_DisableGUI(this);
            OnDisable();
        }

        public void Interact()
        {
            v_isInteractable = true;
            OnInteractable(true);
        }

        public void NoInteract()
        {
            v_isInteractable = false;
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
                    v_mouseHover = (bool)parame[0];
                    break;
                case GUIElementStatus.Focus:
                    v_Focused = (bool)parame[0];
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

        public Matrix4 GetWorldMatrix { get { return v_worldPosition; } }
        public Matrix4 GetProjectionMatrix { get { return v_projection; } }
        public RectangleF GetFinalPosition { get { return v_finalPosition; } }

        public bool IsMouseOn() { return v_finalPosition.IntersectsWith(Input.GetMousePositionRec); }
        public bool isEnabled { get { return v_isEnabled; } }
        public bool isInteractable { get { return v_isInteractable; } }
        public bool isMouseHover { get { return v_mouseHover; } }
        public bool isFocused { get { return v_Focused; } }
        public bool isInputEnabled { get { return v_inputEnable; } }
    }
}