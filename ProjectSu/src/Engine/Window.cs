using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectSu.src.Engine
{
    public class Window : GameWindow
    {
        private static Window instance;

        private EngineMain engineMain;
        private bool engineIsRunning = false;

        private int width;
        private int height;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            instance = this;
        }

        protected override void OnLoad()
        {
            GL.ClearColor(Color4.Black);

            VSync = VSyncMode.Adaptive;
            WindowBorder = WindowBorder.Resizable;

            

            UpdateFrame += Tick;
            RenderFrame += TickRender;
            base.OnLoad();
        }

        private void Tick(FrameEventArgs e)
        {
            Time._DeltaTime = e.Time;
            Time._DTime += e.Time;

            Input.keyboardState = KeyboardState;
            Input.mouseState = MouseState;

            if (!engineIsRunning)
            {
                engineIsRunning = true;
                engineMain = new EngineMain();
            }

            engineMain?.Tick();

            Time.UPS = (int)(1 / e.Time);
        }

        private void TickRender(FrameEventArgs e)
        {
            Thread.Sleep(10);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.FrontFace(FrontFaceDirection.Cw);

            engineMain?.TickRender();

            SwapBuffers();
            Time.FPS = (int)(1 / e.Time);

            Time._Time++;
            Time._Tick = Time._Time % 60;

            if (Time._Time >= int.MaxValue)
            {
                Time._Time = -Time._Time;
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            width = e.Width;
            height = e.Height;

            GL.Viewport(0, 0, e.Width, e.Height);
            engineMain?.OnResize();
            base.OnResize(e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            engineMain?.OnKeyPress(e);
            base.OnKeyDown(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            engineMain?.OnMouseMove(e);
        }

        protected override void OnUnload()
        {
            engineMain?.Dispose();

            UpdateFrame -= Tick;
            RenderFrame -= TickRender;

            engineIsRunning = false;
            base.OnUnload();
        }

        public int Width { get { return instance.width; } }
        public int Height { get { return instance.height; } }

        public static Window Instance { get => instance; private set { } }
    }
}