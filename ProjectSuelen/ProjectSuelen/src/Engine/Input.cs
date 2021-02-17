using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK.Input;

namespace ProjectSu.src.Engine
{
    /// <summary>
    /// All input calculation/system, mouse, keyboard, and more in near future...
    /// </summary>
    public static class Input
    {
        private static List<Key> keyToggleList = new List<Key>();
        private static List<MouseButton> mouseButtomToggleList = new List<MouseButton>();
        
        private static Point _mousePosition;

        public static KeyboardState keyboardState;

        internal static int ScrollWheelValue
        {
            get
            {
                if (!Window.Instance.Focused) { return 0; }
                return Mouse.GetState().ScrollWheelValue;
            }
        }

        internal static Point GetMousePosition { get { return _mousePosition; } }

        internal static bool GetKeyDown(Key key)
        {
            if (!Window.Instance.Focused) { return false; }

            if (!keyToggleList.Contains(key) && keyboardState.IsKeyDown(key))
            {
                keyToggleList.Add(key);
                return true;
            }
            else
            {
                if (keyboardState.IsKeyUp(key))
                {
                    keyToggleList.Remove(key);
                    return false;
                }
                return false;
            }
        }
        internal static bool GetKeyDown(MouseButton buttom)
        {
            if (!Window.Instance.Focused) { return false; }

            if (!mouseButtomToggleList.Contains(buttom) && Mouse.GetState().IsButtonDown(buttom))
            {
                mouseButtomToggleList.Add(buttom);
                return true;
            }
            else
            {
                if (Mouse.GetState().IsButtonUp(buttom))
                {
                    mouseButtomToggleList.Remove(buttom);
                    return false;
                }
                return false;
            }
        }

        internal static bool GetKeyUp(MouseButton buttom)
        {
            if (!Window.Instance.Focused) { return false; }

            if (!mouseButtomToggleList.Contains(buttom) && Mouse.GetState().IsButtonUp(buttom))
            {
                mouseButtomToggleList.Add(buttom);
                return true;
            }
            else
            {
                if (Mouse.GetState().IsButtonDown(buttom))
                {
                    mouseButtomToggleList.Remove(buttom);
                    return false;
                }
                return false;
            }
        }
        internal static bool GetKeyUp(Key key)
        {
            if (!Window.Instance.Focused) { return false; }

            if (!keyToggleList.Contains(key) && keyboardState.IsKeyUp(key))
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

        internal static bool GetKey(Key key)
        {
            if (!Window.Instance.Focused) { return false; }

            return keyboardState.IsKeyDown(key);
        }
        internal static bool GetKey(MouseButton buttom)
        {
            if (!Window.Instance.Focused) { return false; }

            return Mouse.GetState().IsButtonDown(buttom);
        }

        internal static void SetMousePos(Point position)
        {
            _mousePosition = position;
        }
    }
}
