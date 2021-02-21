using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace ProjectSu.src.Engine
{
    /// <summary>
    /// All input calculation/system, mouse, keyboard, and more in near future...
    /// </summary>
    public static class Input
    {
        private static List<Keys> keyToggleList = new List<Keys>();
        private static List<MouseButton> mouseButtomToggleList = new List<MouseButton>();
        
        private static Vector2 _mousePosition;

        public static KeyboardState keyboardState;
        public static MouseState mouseState;

        internal static float ScrollWheelValue
        {
            get
            {
                if (!Window.Instance.IsFocused) { return 0; }
                return mouseState.Scroll.Y;
            }
        }

        internal static Vector2 GetMousePosition { get { return _mousePosition; } }

        internal static bool GetKeyDown(Keys key)
        {
            if (!Window.Instance.IsFocused) { return false; }

            Input.keyboardState = Window.Instance.KeyboardState;
            Input.mouseState = Window.Instance.MouseState;

            if (!keyToggleList.Contains(key) && keyboardState.IsKeyDown(key))
            {
                keyToggleList.Add(key);
                return true;
            }
            else
            {
                if (!keyboardState.IsKeyDown(key))
                {
                    keyToggleList.Remove(key);
                    return false;
                }
                return false;
            }
        }
        internal static bool GetKeyDown(MouseButton buttom)
        {
            if (!Window.Instance.IsFocused) { return false; }

            Input.keyboardState = Window.Instance.KeyboardState;
            Input.mouseState = Window.Instance.MouseState;

            if (!mouseButtomToggleList.Contains(buttom) && mouseState.IsButtonDown(buttom))
            {
                mouseButtomToggleList.Add(buttom);
                return true;
            }
            else
            {
                if (!mouseState.IsButtonDown(buttom))
                {
                    mouseButtomToggleList.Remove(buttom);
                    return false;
                }
                return false;
            }
        }

        internal static bool GetKeyUp(MouseButton buttom)
        {
            if (!Window.Instance.IsFocused) { return false; }

            Input.keyboardState = Window.Instance.KeyboardState;
            Input.mouseState = Window.Instance.MouseState;

            if (!mouseButtomToggleList.Contains(buttom) && !mouseState.IsButtonDown(buttom))
            {
                mouseButtomToggleList.Add(buttom);
                return true;
            }
            else
            {
                if (mouseState.IsButtonDown(buttom))
                {
                    mouseButtomToggleList.Remove(buttom);
                    return false;
                }
                return false;
            }
        }
        internal static bool GetKeyUp(Keys key)
        {
            if (!Window.Instance.IsFocused) { return false; }

            Input.keyboardState = Window.Instance.KeyboardState;
            Input.mouseState = Window.Instance.MouseState;

            if (!keyToggleList.Contains(key) && !keyboardState.IsKeyDown(key))
            {
                keyToggleList.Add(key);
                return true;
            }
            else
            {
                if (keyboardState.IsKeyDown(key))
                {
                    keyToggleList.Remove(key);
                    return false;
                }
                return false;
            }
        }

        internal static bool GetKey(Keys key)
        {
            if (!Window.Instance.IsFocused) { return false; }

            Input.keyboardState = Window.Instance.KeyboardState;
            Input.mouseState = Window.Instance.MouseState;

            return keyboardState.IsKeyDown(key);
        }
        internal static bool GetKey(MouseButton buttom)
        {
            if (!Window.Instance.IsFocused) { return false; }

            Input.keyboardState = Window.Instance.KeyboardState;
            Input.mouseState = Window.Instance.MouseState;

            return mouseState.IsButtonDown(buttom);
        }

        internal static void SetMousePos(Vector2 position)
        {
            _mousePosition = position;
        }
    }
}
