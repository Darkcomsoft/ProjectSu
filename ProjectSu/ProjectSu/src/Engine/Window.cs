using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
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

        public Window(int width, int height, string title) : base(width, height, GraphicsMode.Default, title, GameWindowFlags.Default, DisplayDevice.Default, 330, 330, GraphicsContextFlags.Default)
        {
            instance = this;
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(Color4.Black);

            TargetRenderFrequency = 60;
            TargetUpdateFrequency = 60;
            
            VSync = VSyncMode.Adaptive;
            WindowBorder = WindowBorder.Resizable;


            UpdateFrame += Tick;
            RenderFrame += TickRender;
        }

        private void Tick(object sender, FrameEventArgs e)
        {
            Time._DeltaTime = e.Time;
            Time._DTime += e.Time;

            if (!engineIsRunning)
            {
                engineIsRunning = true;
                engineMain = new EngineMain();
            }

            engineMain?.Tick();

            Time.UPS = (int)(1 / e.Time);
        }

        private void TickRender(object sender, FrameEventArgs e)
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

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            engineMain?.OnResize();
            base.OnResize(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            engineMain?.OnKeyPress(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            engineMain?.OnMouseMove(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            engineMain?.Dispose();

            UpdateFrame -= Tick;
            RenderFrame -= TickRender;

            engineIsRunning = false;
        }

        public static Window Instance { get => instance; private set { } }
    }
}