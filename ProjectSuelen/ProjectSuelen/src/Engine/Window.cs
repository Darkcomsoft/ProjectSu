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

namespace ProjectSuelen.src.Engine
{
    public class Window : GameWindow
    {
        private static Window instance;

        private EngineMain engineMain;

        public Window(int width, int height, string title) : base(width, height, GraphicsMode.Default, title)
        {
            instance = this;
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(Color4.Black);

            //VSync = VSyncMode.Off;
            WindowBorder = WindowBorder.Resizable;

            engineMain = new EngineMain();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Time._DeltaTime = e.Time;
            Time._DTime += e.Time;

            if (engineMain != null)
            {
                engineMain.Tick();
            }

            Time.UPS = (int)(1d / e.Time);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (engineMain !=null)
            {
                engineMain.TickRender();
            }

            Time.FPS = (int)(1f / e.Time);

            Time._Time++;
            Time._Tick = Time._Time % 60;

            if (Time._Time >= int.MaxValue)
            {
                Time._Time = -Time._Time;
            }
            //Thread.Sleep(60 / 5);

            SwapBuffers();
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            if (engineMain != null)
            {
                engineMain.OnResize();
            }
            base.OnResize(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (engineMain != null)
            {
                engineMain.OnKeyPress(e);
            }
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (engineMain != null)
            {
                engineMain.OnMouseMove(e);
            }
        }

        protected override void OnUnload(EventArgs e)
        {
            if (engineMain != null)
            {
                engineMain.Dispose();
            }
        }

        public static Window Instance { get => instance; private set { } }
    }
}