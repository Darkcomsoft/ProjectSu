﻿using ProjectSu;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using System.Drawing;
using OpenTK.Input;
using ProjectSu.src.Engine.UI.GUIElements;
using OpenTK.Mathematics;

namespace ProjectSu.src.Engine.UI
{
    public abstract class GUIBase : ClassBase
    {
        private bool Enabled;
        private Rectangle _Rectangle;
        private Rectangle _FinalRectangle;
        private UIDock _Dock;

        private Color4 NormalColor;
        private Color4 HoverColor;
        private Color4 ClickColor;
        private Color4 FocusedColor;
        private Color4 NoInteractColor;

        public bool ShowBackGround = true;

        private bool IsReady = false;
        private bool CanShowFocus = false;

        private bool IP_Hover = false;
        private bool IP_Cliked = false;
        private bool IP_Interact = true;
        /// <summary>
        /// Activated is if the focus of the input is this
        /// </summary>
        private bool IP_Focused = false;

        public Action OnClick;

        #region RenderValues
        private int VAO, IBO, vbo, ubo;
        public Vector2[] _vertices;
        public Vector2[] _uvs;
        public int[] _indices;

        private Matrix4 _projection;
        #endregion

        public GUIBase(Rectangle rec)
        {
            GUI.AddGuiElement(this);

            _Rectangle = rec;
            Enabled = true;

            NormalColor = Color4.White;
            HoverColor = Color4.Gray;
            ClickColor = Color4.Black;
            NoInteractColor = Color4.White;

            SetFinalRectangle();

            SetGLBuffer();
        }

        public GUIBase(Rectangle rec, UIDock uIDock)
        {
            GUI.AddGuiElement(this);

            _Rectangle = rec;
            _Dock = uIDock;
            Enabled = true;

            NormalColor = Color4.White;
            HoverColor = Color4.Gray;
            ClickColor = Color4.Black;
            FocusedColor = Color4.PapayaWhip;
            NoInteractColor = Color4.White;

            SetFinalRectangle();

            SetGLBuffer();
        }

        public GUIBase(Rectangle rec, UIDock uIDock, Color4 nColor, Color4 hColor, Color4 cColor, Color4 fColor)
        {
            GUI.AddGuiElement(this);

            _Rectangle = rec;
            _Dock = uIDock;
            Enabled = true;

            NormalColor = nColor;
            HoverColor = hColor;
            ClickColor = cColor;
            FocusedColor = fColor;
            NoInteractColor = Color4.Gray;

            SetFinalRectangle();

            SetGLBuffer();
        }

        private void SetGLBuffer()
        {
            _indices = new int[]
            {
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
            };

            _uvs = new Vector2[] {
                new Vector2(0.0f, 1.0f),
                new Vector2(1.0f, 1.0f),

                new Vector2( 1.0f, 0.0f),
                new Vector2(0.0f, 0.0f)
            };

            _projection = Matrix4.CreateOrthographicOffCenter(Window.Instance.ClientRectangle.Left, Window.Instance.ClientRectangle.Right, Window.Instance.ClientRectangle.Bottom, Window.Instance.ClientRectangle.Top, 0f, 5.0f);


            VAO = GL.GenVertexArray();
            IBO = GL.GenBuffer();
            vbo = GL.GenBuffer();
            ubo = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(int), _indices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(VAO);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * Vector2.SizeInBytes, _vertices, BufferUsageHint.StreamDraw);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, ubo);
            GL.BufferData(BufferTarget.ArrayBuffer, _uvs.Length * Vector2.SizeInBytes, _uvs, BufferUsageHint.StreamDraw);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(1);

            IsReady = true;
        }

        /*public virtual void Tick()
        {
            if (Enabled)
            {
                if (IP_Focused)
                {

                }

                if (IP_Hover)
                {
                    
                }
            }
        }*/

        public void TickRender()
        {
            if (IsReady && Enabled)
            {
                RenderBefor();

                GL.Enable(EnableCap.Blend);
                GL.Disable(EnableCap.DepthTest);
                GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

                GL.BindVertexArray(VAO);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);

                GUI.GetShader.Use();

                TickRenderCustom();//use the ui elements custom values, like images, text etc.

                if (ShowBackGround)
                {
                    if (IP_Interact)
                    {
                        if (CanShowFocus)
                        {
                            if (IP_Focused)
                            {
                                GUI.GetShader.SetColor("MainColor", FocusedColor);
                            }
                            else
                            {
                                if (IP_Hover)
                                {
                                    if (IP_Cliked)
                                    {
                                        GUI.GetShader.SetColor("MainColor", ClickColor);
                                    }
                                    else
                                    {
                                        GUI.GetShader.SetColor("MainColor", HoverColor);
                                    }
                                }
                                else
                                {
                                    GUI.GetShader.SetColor("MainColor", NormalColor);
                                }
                            }
                        }
                        else
                        {
                            if (IP_Hover)
                            {
                                if (IP_Cliked)
                                {
                                    GUI.GetShader.SetColor("MainColor", ClickColor);
                                }
                                else
                                {
                                    GUI.GetShader.SetColor("MainColor", HoverColor);
                                }
                            }
                            else
                            {
                                GUI.GetShader.SetColor("MainColor", NormalColor);
                            }
                        }
                    }
                    else
                    {
                        GUI.GetShader.SetColor("MainColor", NoInteractColor);
                    }

                    GUI.GetShader.SetMatrix4("projection", _projection);

                    GL.DrawElements(BeginMode.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
                }

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                GL.BindVertexArray(0);

                GL.Enable(EnableCap.DepthTest);
                GL.Disable(EnableCap.Blend);

                RenderAfter();
            }
        }

        public void Resize()
        {
            _projection = Matrix4.CreateOrthographicOffCenter(Window.Instance.ClientRectangle.Left, Window.Instance.ClientRectangle.Right, Window.Instance.ClientRectangle.Bottom, Window.Instance.ClientRectangle.Top, 0f, 5.0f);

            IsReady = false;

            SetFinalRectangle();

            if (ShowBackGround)
            {
                GL.BindVertexArray(VAO);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * Vector2.SizeInBytes, _vertices, BufferUsageHint.StreamDraw);
                GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
                GL.EnableVertexAttribArray(0);
            }

            OnResize();

            IsReady = true;
        }

        private void TickRenderCustom()
        {
            GUI.GetShader.Setbool("HaveTexture", false);

            RenderCustomValues();
        }

        public virtual void RenderCustomValues()
        {

        }

        /// <summary>
        /// Render First, render behind the background quad 
        /// </summary>
        public virtual void RenderBefor()
        {

        }

        /// <summary>
        /// Render last of all, render front of the background quad
        /// </summary>
        public virtual void RenderAfter()
        {

        }

        private void SetFinalRectangle()
        {
            _FinalRectangle = _Rectangle;

            //_FinalRectangle.Width = _Rectangle.Width / (Window.Instance.Width + Window.Instance.Height / 2);
            //_FinalRectangle.Height = _Rectangle.Height / (Window.Instance.Height + Window.Instance.Width / 2);

            switch (_Dock)
            {
                case UIDock.Cennter:
                    _FinalRectangle.X = (Window.Instance.Width / 2) - _Rectangle.X - (_Rectangle.Width / 2);
                    _FinalRectangle.Y = (Window.Instance.Height / 2) - _Rectangle.Y - (_Rectangle.Height / 2);
                    break;
                case UIDock.CenterTop:
                    _FinalRectangle.X = (Window.Instance.Width / 2) + _Rectangle.X - (_Rectangle.Width / 2);
                    _FinalRectangle.Y = _Rectangle.Y + (_Rectangle.Height / _Rectangle.Height) - 1;
                    break;
                case UIDock.CenterBottom:
                    _FinalRectangle.X = (Window.Instance.Width / 2) + _Rectangle.X - (_Rectangle.Width / 2);
                    _FinalRectangle.Y = Window.Instance.Height - _Rectangle.Y - (_Rectangle.Height);
                    break;
                case UIDock.CenterLeft:
                    _FinalRectangle.X = _Rectangle.X + (_Rectangle.Width / _Rectangle.Width) - 1;
                    _FinalRectangle.Y = (Window.Instance.Height / 2) + _Rectangle.Y - (_Rectangle.Height / 2);
                    break;
                case UIDock.CenterRight:
                    _FinalRectangle.X = (Window.Instance.Width) + _Rectangle.X - (_Rectangle.Width);
                    _FinalRectangle.Y = (Window.Instance.Height / 2) + _Rectangle.Y - (_Rectangle.Height / 2);
                    break;
                case UIDock.TopLeft:
                    _FinalRectangle.X = _Rectangle.X + (_Rectangle.Width / _Rectangle.Width) - 1;
                    _FinalRectangle.Y = _Rectangle.Y + (_Rectangle.Height / _Rectangle.Height) - 1;
                    break;
                case UIDock.TopRight:
                    _FinalRectangle.X = (Window.Instance.Width) - _Rectangle.X - (_Rectangle.Width);
                    _FinalRectangle.Y = _Rectangle.Y + (_Rectangle.Height / _Rectangle.Height) - 1;
                    break;
                case UIDock.BottomLeft:
                    _FinalRectangle.X = _Rectangle.X + (_Rectangle.Width / _Rectangle.Width) - 1;
                    _FinalRectangle.Y = Window.Instance.Height - _Rectangle.Y - (_Rectangle.Height);
                    break;
                case UIDock.BottomRight:
                    _FinalRectangle.X = (Window.Instance.Width) - _Rectangle.X - (_Rectangle.Width);
                    _FinalRectangle.Y = Window.Instance.Height - _Rectangle.Y - (_Rectangle.Height);
                    break;



                case UIDock.SizeBottom:
                    _FinalRectangle.X = _Rectangle.X + (_Rectangle.Width / 2);
                    _FinalRectangle.Y = Window.Instance.Height + _Rectangle.Y - (_Rectangle.Height);

                    _FinalRectangle.Width = Window.Instance.Width - _Rectangle.Width;
                    break;
                case UIDock.SizeTop:
                    _FinalRectangle.X = _Rectangle.X + (_Rectangle.Width / 2);
                    _FinalRectangle.Y = _Rectangle.Y + (_Rectangle.Height / _Rectangle.Height) - 1;

                    _FinalRectangle.Width = Window.Instance.Width - _Rectangle.Width;
                    break;
                case UIDock.SizeLeft:
                    _FinalRectangle.X = _Rectangle.X + (_Rectangle.Width / _Rectangle.Width) - 1;
                    _FinalRectangle.Y = _Rectangle.Y + (_Rectangle.Height / 2);

                    _FinalRectangle.Height = Window.Instance.Height - _Rectangle.Height;
                    break;
                case UIDock.SizeRight:
                    _FinalRectangle.X = (Window.Instance.Width) + _Rectangle.X - (_Rectangle.Width);
                    _FinalRectangle.Y = _Rectangle.Y + (_Rectangle.Height / 2);

                    _FinalRectangle.Height = Window.Instance.Height - _Rectangle.Height;
                    break;
                case UIDock.ScreenSize:
                    _FinalRectangle.Width = Window.Instance.Width - _Rectangle.Width;
                    _FinalRectangle.Height = Window.Instance.Height - _Rectangle.Height;
                    break;
                case UIDock.ScreenSizeRatio:
                    _FinalRectangle.Width = (Window.Instance.Width + Window.Instance.Height / 2) - _Rectangle.Width;
                    _FinalRectangle.Height = (Window.Instance.Height + Window.Instance.Width / 2) - _Rectangle.Height;
                    break;
            }

            _vertices = new Vector2[]
            {
                    new Vector2(_FinalRectangle.X + _FinalRectangle.Width/ 2,  _FinalRectangle.Y + _FinalRectangle.Height / 2) + new Vector2(-_FinalRectangle.Width / 2, -_FinalRectangle.Height/ 2), // top right
                    new Vector2(_FinalRectangle.X + _FinalRectangle.Width/ 2,  _FinalRectangle.Y + _FinalRectangle.Height/ 2) + new Vector2(_FinalRectangle.Width/ 2, -_FinalRectangle.Height/ 2), // bottom right
                    new Vector2(_FinalRectangle.X + _FinalRectangle.Width/ 2,  _FinalRectangle.Y + _FinalRectangle.Height/ 2) + new Vector2(_FinalRectangle.Width/ 2, _FinalRectangle.Height/ 2), // bottom left
                    new Vector2(_FinalRectangle.X + _FinalRectangle.Width/ 2,  _FinalRectangle.Y + _FinalRectangle.Height/ 2) + new Vector2(-_FinalRectangle.Width/ 2, _FinalRectangle.Height/ 2) // top left
            };
        }

        public virtual void OnKeyPress(char KeyChar)
        {

        }

        public virtual void Enable()
        {
            Enabled = true;
        }

        public virtual void Disable()
        {
            Enabled = false;

            IP_Hover = false;
            IP_Cliked = false;
        }

        public virtual void Click()
        {
            if (OnClick != null)
            {
                OnClick();
            }
            IP_Cliked = true;
        }

        public virtual void ClickReleased()
        {
            IP_Cliked = false;
        }

        public virtual void Hover()
        {
            IP_Hover = true;
        }

        public virtual void UnHover()
        {
            IP_Hover = false;
        }

        public virtual void Focus()
        {
            IP_Focused = true;
        }

        public virtual void UnFocus()
        {
            IP_Focused = false;
        }

        public virtual void Dock(UIDock uIDock)
        {
            _Dock = uIDock;
        }

        public virtual void ShowFocus()
        {
            CanShowFocus = true;
        }

        public virtual void HideFocus()
        {
            CanShowFocus = false;
        }

        public virtual void Interactable()
        {
            IP_Interact = true;
        }

        public virtual void NoInteractable()
        {
            IP_Interact = false;
        }

        /// <summary>
        /// Used when you cant wait for next frame, this is gona render the at the current frame
        /// </summary>
        public virtual void ForceRender()
        {
            TickRender();
        }

        public virtual void SetPosition(int x, int y)
        {
            _Rectangle.X = x;
            _Rectangle.Y = y;

            Resize();
        }

        public virtual void SetSize(int Width, int Height)
        {
            _Rectangle.Width = Width;
            _Rectangle.Height = Height;

            Resize();
        }

        public void SetBackColors(Color4 nColor, Color4 hColor, Color4 cColor, Color4 fColor)
        {
            NormalColor = nColor;
            HoverColor = hColor;
            ClickColor = cColor;
            FocusedColor = fColor;
        }

        public virtual void SetInteractColor(Color4 color)
        {
            NoInteractColor = color;
        }

        protected override void OnDispose()
        {
            GUI.RemoveGuiElement(this);

            if (IsReady)
            {
                IsReady = false;

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                GL.BindVertexArray(0);

                GL.BindVertexArray(0);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.DisableVertexAttribArray(0);

                GL.BindVertexArray(0);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.DisableVertexAttribArray(0);

                GL.DeleteBuffer(IBO);
                GL.DeleteBuffer(vbo);
                GL.DeleteBuffer(ubo);
                GL.DeleteVertexArray(VAO);
            }
            base.OnDispose();
        }

        public virtual void OnResize() { }

        public Rectangle GetRectangle { get { return _FinalRectangle; } }
        public Rectangle GetLocalRectangle { get { return _Rectangle; } }

        public bool IsEnabled { get { return Enabled; } }
        public bool IsHover { get { return IP_Hover; } }
        public bool IsFocused { get { return IP_Focused; } }
        public bool IsInteract { get { return IP_Interact; } }
    }
}