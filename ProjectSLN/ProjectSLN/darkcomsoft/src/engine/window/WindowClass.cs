using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Projectsln.darkcomsoft.src.engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;

namespace Projectsln.darkcomsoft.src.engine.window
{
    public class WindowClass : GWindow
    {
        private static WindowClass instance;

        private Application application;

        private bool isRunning = false;

        private int width;
        private int height;

        public Rectangle WindowRectangle;

        public WindowClass(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            instance = this;
        }

        protected override void OnLoad()
        {
            GL.ClearColor(Color4.Black);

            VSync = VSyncMode.Off;
            WindowBorder = WindowBorder.Resizable;

            Input.keyboardState = KeyboardState;
            Input.mouseState = MouseState;

            UpdateFrame += Tick;
            RenderFrame += TickRender;

            Thread.CurrentThread.IsBackground = true;//THIS IS A TEST REMOVE THIS AFTER

            base.OnLoad();
        }

        private void Tick(FrameEventArgs e)
        {
            Time._DeltaTime = e.Time;
            Time._DTime += e.Time;

            if (Input.GetKeyDown(Keys.F11, 0))
            {
                SetFullScreen();
            }

            if (!isRunning)
            {
                isRunning = true;
                application = new Application();
                Size = new Vector2i(800, 600);
            }
            else
            {
                if (application != null)
                {
                    application.Tick(e.Time);
                }
            }

            Time.UPS = (int)(1 / e.Time);
        }

        private void TickRender(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.DepthFunc(DepthFunction.Lequal);

            if (application != null)
            {
                application.TickDraw(e.Time);
            }

            SwapBuffers();
            Time.FPS = (int)(1f / e.Time);

            Time._Time++;
            Time._Tick = Time._Time % 60;

            if (Time._Time >= int.MaxValue)
            {
                Time._Time = -Time._Time;
            }
            
            Title = string.Format("UPS:{0} FPS:{1}", Time.UPS, Time.FPS);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            width = e.Width;
            height = e.Height;

            WindowRectangle.X = 0;
            WindowRectangle.Y = 0;
            WindowRectangle.Width = width;
            WindowRectangle.Height = height;

            GL.Viewport(0, 0, e.Width, e.Height);

            //engineMain?.OnResize();
            base.OnResize(e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            //engineMain?.OnKeyPress(e);
            base.OnKeyDown(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            //engineMain?.OnMouseMove(e);
        }

        protected override void OnUnload()
        {
            if (application != null)
            {
                application.Dispose();
                application = null;
            }
            base.OnUnload();
        }

        protected override void OnClosed()
        {
            Close();
            base.OnClosed();
        }

        protected override void OnRefresh()
        {
            base.OnRefresh();
        }

        public void SetFullScreen()
        {
            if (WindowState == WindowState.Fullscreen)
            {
                WindowBorder = WindowBorder.Resizable;
                WindowState = WindowState.Normal;
                Size = new Vector2i(800, 600);

                CenterWindow();
            }
            else
            {
                WindowBorder = WindowBorder.Hidden;
                WindowState = WindowState.Fullscreen;
            }
        }

        public int Width { get { return instance.width; } }
        public int Height { get { return instance.height; } }

        public static WindowClass Instance { get => instance; private set { } }
    }
}