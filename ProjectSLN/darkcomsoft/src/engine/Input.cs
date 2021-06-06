using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ProjectIND.darkcomsoft.src.engine.window;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ProjectIND.darkcomsoft.src.engine
{
    /// <summary>
    /// All input calculation/system, mouse, keyboard, and more in near future...
    /// </summary>
    public class Input : ClassBase
    {
        private static Input v_instance;

        private RectangleF v_mousePositionRec = new Rectangle(0, 0, 1, 1);

        public KeyboardState keyboardState;
        public MouseState mouseState;

        public Input()
        {
            v_instance = this;

            keyboardState = WindowMain.Instance.KeyboardState;
            mouseState = WindowMain.Instance.MouseState;
        }

        protected override void OnDispose()
        {
            keyboardState = null;
            mouseState = null;
            v_instance = null;
            base.OnDispose();
        }

        public static bool GetKey(Keys key)
        {
            if (instance == null) { return false; }
            if (!WindowMain.Instance.IsFocused) { return false; }

            return instance.keyboardState.IsKeyDown(key);
        }

        public static bool GetKeyDown(Keys key)
        {
            if (instance == null) { return false; }
            if (!WindowMain.Instance.IsFocused) { return false; }

            return instance.keyboardState.IsKeyPressed(key);
        }

        public static bool GetKeyUp(Keys key)
        {
            if (instance == null) { return false; }
            if (!WindowMain.Instance.IsFocused) { return false; }

            return instance.keyboardState.IsKeyReleased(key);
        }

        public static bool GetButtom(MouseButton mouseButton)
        {
            if (instance == null) { return false; }
            if (!WindowMain.Instance.IsFocused) { return false; }

            return instance.mouseState.IsButtonDown(mouseButton);
        }

        /// <summary>
        /// Get mouse position RectangleF relative to bottom-left of the screen 
        /// </summary>
        public static RectangleF GetMousePositionRec { get { instance.v_mousePositionRec.X = instance.mouseState.Position.X; instance.v_mousePositionRec.Y = WindowMain.Instance.Height - instance.mouseState.Position.Y; return instance.v_mousePositionRec; } }
        public static Input instance { get { return v_instance; } }
        public static MouseState getMouse{get{ return WindowMain.Instance.MouseState; } }
    }
}