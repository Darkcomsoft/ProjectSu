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
        private static Input m_instance;

        private RectangleF m_mousePositionRec = new Rectangle(0, 0, 1, 1);

        public KeyboardState keyboardState;
        public MouseState mouseState;

        public Input()
        {
            m_instance = this;

            keyboardState = WindowMain.Instance.KeyboardState;
            mouseState = WindowMain.Instance.MouseState;
        }

        protected override void OnDispose()
        {
            keyboardState = null;
            mouseState = null;
            m_instance = null;
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
        public static RectangleF GetMousePositionRec { get { instance.m_mousePositionRec.X = instance.mouseState.Position.X; instance.m_mousePositionRec.Y = WindowMain.Instance.Height - instance.mouseState.Position.Y; return instance.m_mousePositionRec; } }
        public static Input instance { get { return m_instance; } }
    }
}