using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;

namespace Projectsln.darkcomsoft.src.engine.window
{
    public class WindowClass : WindowMain
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
            

            base.OnLoad();
        }

        protected override void OnTick(double time)
        {
            
            base.OnTick(time);
        }

        protected override void OnTickDraw(double time)
        {
            
            base.OnTickDraw(time);
        }

        

        

        protected override void OnUnload()
        {
            
            base.OnUnload();
        }

        
    }
}