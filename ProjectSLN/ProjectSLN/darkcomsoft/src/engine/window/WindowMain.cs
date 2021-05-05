using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Projectsln.darkcomsoft.src.debug;
using Projectsln.darkcomsoft.src.enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Projectsln.darkcomsoft.src.engine.window
{
    /// <summary>
    /// my implementation of the OpenTk GameWindow class
    /// </summary>
    public class WindowMain : NativeWindow
    {
        private static WindowMain instance;
        private VSyncMode _vSync;

        private Application application;
        private bool isRunning = false;
        private int width;
        private int height;
        public Rectangle WindowRectangle;

        private System.Diagnostics.Stopwatch _watchUpdate;
        private System.Diagnostics.Stopwatch _watchTick;
        private System.Diagnostics.Stopwatch _watchDraw;

        /// <summary>
        /// Tick Rate Default: 60, this is not Frames per seconds, this is for the tick per seconds, recommended use 60
        /// </summary>
        private int TickRate = 60;
        /// <summary>
        /// The Frame rate, is the rate of the rendering, IS NOT IMPLEMENTED
        /// </summary>
        private int FrameRate = 60;

        public WindowMain(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(nativeWindowSettings)
        {
            instance = this;

            FrameRate = (int)gameWindowSettings.RenderFrequency;
            TickRate = (int)gameWindowSettings.UpdateFrequency;

            VSync = VSyncMode.Off;//set the VSync on start, because fuck-it, nobody wants this (:
        }

        public void Run()
        {
            Context.MakeCurrent();

            OnLoad();

            OnResize(new ResizeEventArgs(Size));

            RunGameLoop();
        }

        private void RunGameLoop()
        {
            _watchUpdate = new System.Diagnostics.Stopwatch();
            _watchTick = new System.Diagnostics.Stopwatch();
            _watchDraw = new System.Diagnostics.Stopwatch();

            _watchUpdate.Start();
            _watchTick.Start();
            _watchDraw.Start();

            var l_lastTime = _watchUpdate.ElapsedMilliseconds;
            var l_mspertick = 1000.0d / TickRate;
            //var l_Dmspertick = 1000.0d / FrameRate;
            var l_noprocess = 0d;
            //var l_Dnoprocess = 0d;
            var l_frames = 0;
            var l_ticks = 0;
            var lastTimer1 = _watchUpdate.ElapsedMilliseconds;
            long now;
            bool dorender = true;

            while (true)
            {
                now = _watchUpdate.ElapsedMilliseconds;
                l_noprocess += (now - l_lastTime) / l_mspertick;
                //l_Dnoprocess += (now - l_lastTime) / l_Dmspertick;
                l_lastTime = now;

                if (!Exists || IsExiting)
                {
                    DestroyWindow();
                    return;
                }

                while (l_noprocess >= 1)
                {
                    l_ticks++;
                    ProcessEvents();
                    Tick();
                    l_noprocess -= 1;
                    dorender = true;
                }
                _watchTick.Restart();

                /*while (l_Dnoprocess >= 1)
                {
                    l_frames++;
                    TickDraw();
                    l_Dnoprocess -= 1;
                }*/

                Thread.Sleep(1);

                if (dorender)
                {
                    l_frames++;
                    TickDraw();
                }
                _watchDraw.Restart();

                if (_watchUpdate.ElapsedMilliseconds - lastTimer1 > 1000)
                {
                    Debug.Log(l_ticks + " ticks, " + l_frames + " fps", "GAMELOOP");
                    lastTimer1 += 1000;
                    l_frames = 0;
                    l_ticks = 0;
                }
            }
        }

        private void Tick()
        {
            OnTick(_watchTick.Elapsed.TotalSeconds);
            //_watchTick.Restart();
        }

        private void TickDraw()
        {
            OnTickDraw(_watchDraw.Elapsed.TotalSeconds);
            //_watchDraw.Restart();

            if (_vSync == VSyncMode.Adaptive)
            {
                GLFW.SwapInterval(0);
            }
        }

        protected virtual void OnLoad()
        {
            GL.ClearColor(Color4.Black);

            VSync = VSyncMode.Off;//set the VSync on start, because fuck-it, nobody wants this (:
            WindowBorder = WindowBorder.Resizable;
        }

        protected virtual void OnUnload()
        {
            application?.Dispose();
            application = null;
        }

        protected virtual void OnTick(double time)
        {
            if (IsExiting) { return; }

            Time._DeltaTime = time;
            Time._DTime += time;

            CursorManager.Tick();

            if (Input.GetKeyDown(Keys.F11))
            {
                SetFullScreen();
            }

            if (!isRunning)
            {
                isRunning = true;
                application = new Application(ApplicationType.Client);
                Size = new Vector2i(800, 600);
            }
            else
            {
                if (application != null)
                {
                    application.Tick(time);
                }
            }

            Time.UPS = (int)(1 / time);
        }

        protected virtual void OnTickDraw(double time)
        {
            if (IsExiting) { return; }

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //GL.DepthFunc(DepthFunction.Lequal);//VER SE ISSO E NECESSARIO, SE NAO REMOVER ESTA INCIVERL FUINÇAO

            if (application != null)
            {
                application.TickDraw(time);
            }

            SwapBuffers();
            Time.FPS = (int)(1f / time);

            Time._Time++;
            Time._Tick = Time._Time % 60;

            if (Time._Time >= int.MaxValue)
            {
                Time._Time = 0;
            }

            if (Time._Tick == 0 || Time._Tick == 60)
            {
                Title = string.Format("UPS:{0} FPS:{1}", Time.UPS, Time.FPS);
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            for (int i = 0; i < 2; i++)
            {
                GL.Viewport(0, 0, e.Width, e.Height);

                width = e.Width;
                height = e.Height;

                WindowRectangle.X = 0;
                WindowRectangle.Y = 0;
                WindowRectangle.Width = width;
                WindowRectangle.Height = height;

                application?.OnResize();
                //engineMain?.OnResize();
            }
            base.OnResize(e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            //engineMain?.OnKeyPress(e);
            base.OnKeyDown(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            application?.OnMouseMove();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            application?.OnMouseDown(e);
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            application?.OnMouseUp(e);
            base.OnMouseUp(e);
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

        /// <summary>
        /// Gets or sets the VSyncMode.
        /// </summary>
        public VSyncMode VSync
        {
            get => _vSync;
            set
            {
                switch (value)
                {
                    case VSyncMode.On:
                        GLFW.SwapInterval(1);
                        break;

                    case VSyncMode.Off:
                        GLFW.SwapInterval(0);
                        break;

                    case VSyncMode.Adaptive:
                        //GLFW.SwapInterval(IsRunningSlowly ? 0 : 1);
                        break;
                }

                _vSync = value;
            }
        }

        /// <summary>
        /// Swaps the front and back buffers of the current GraphicsContext, presenting the rendered scene to the user.
        /// </summary>
        public virtual void SwapBuffers()
        {
            Context.SwapBuffers();
        }

        public override void Close()
        {
            OnUnload();
            base.Close();
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

        public static WindowMain Instance { get => instance; private set { } }
    }
}
