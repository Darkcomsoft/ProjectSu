using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.engine
{
    /// <summary>
    /// All input calculation/system, mouse, keyboard, and more in near future...
    /// </summary>
    public static class Input
    {
        private static List<CurrentKey> keyToggleList = new List<CurrentKey>();
        private static List<CurrentButtom> mouseButtomToggleList = new List<CurrentButtom>();

        private static Vector2 _mousePosition;

        public static KeyboardState keyboardState;
        public static MouseState mouseState;

        private static CurrentKey currentKey = new CurrentKey();
        private static CurrentButtom currentButtom = new CurrentButtom();

        internal static float ScrollWheelValue
        {
            get
            {
                if (!Window.Instance.IsFocused) { return 0; }
                return mouseState.Scroll.Y;
            }
        }

        internal static Vector2 GetMousePosition { get { return _mousePosition; } }

        /// <summary>
        /// If KeyPressed is Down Once, Set a Key and a any number you want, any other input check can have the same number
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ID">any number you want, any other input check can have the same number</param>
        /// <returns></returns>
        internal static bool GetKeyDown(Keys key, int ID)
        {
            if (!Window.Instance.IsFocused) { return false; }

            Input.keyboardState = Window.Instance.KeyboardState;
            Input.mouseState = Window.Instance.MouseState;

            currentKey.Index = ID;
            currentKey.key = key;

            if (!keyToggleList.Contains(currentKey) && keyboardState.IsKeyDown(key))
            {
                keyToggleList.Add(currentKey);
                return true;
            }
            else
            {
                if (!keyboardState.IsKeyDown(key))
                {
                    keyToggleList.Remove(currentKey);
                }
                return false;
            }
        }

        /// <summary>
        /// If MouseKeyPressed is Down Once, Set a Key and a any number you want, any other input check can have the same number
        /// </summary>
        /// <param name="buttom"></param>
        /// <param name="ID">any number you want, any other input check can have the same number</param>
        /// <returns></returns>
        internal static bool GetKeyDown(MouseButton buttom, int ID)
        {
            if (!Window.Instance.IsFocused) { return false; }

            Input.keyboardState = Window.Instance.KeyboardState;
            Input.mouseState = Window.Instance.MouseState;

            currentButtom.Index = ID;
            currentButtom.Buttom = buttom;

            if (!mouseButtomToggleList.Contains(currentButtom) && mouseState.IsButtonDown(buttom))
            {
                mouseButtomToggleList.Add(currentButtom);
                return true;
            }
            else
            {
                if (!mouseState.IsButtonDown(buttom))
                {
                    mouseButtomToggleList.Remove(currentButtom);
                    return false;
                }
                return false;
            }
        }

        /// <summary>
        /// If MouseKeyPressed is Up Once, Set a Key and a any number you want, any other input check can have the same number
        /// </summary>
        /// <param name="buttom"></param>
        /// <param name="ID">any number you want, any other input check can have the same number</param>
        /// <returns></returns>
        internal static bool GetKeyUp(MouseButton buttom, int ID)
        {
            if (!Window.Instance.IsFocused) { return false; }

            Input.keyboardState = Window.Instance.KeyboardState;
            Input.mouseState = Window.Instance.MouseState;

            currentButtom.Index = ID;
            currentButtom.Buttom = buttom;

            if (!mouseButtomToggleList.Contains(currentButtom) && !mouseState.IsButtonDown(buttom))
            {
                mouseButtomToggleList.Add(currentButtom);
                return true;
            }
            else
            {
                if (mouseState.IsButtonDown(buttom))
                {
                    mouseButtomToggleList.Remove(currentButtom);
                    return false;
                }
                return false;
            }
        }

        /// <summary>
        /// If KeyPressed is Up Once, Set a Key and a any number you want, any other input check can have the same number
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ID">any number you want, any other input check can have the same number</param>
        /// <returns></returns>
        internal static bool GetKeyUp(Keys key, int ID)
        {
            if (!Window.Instance.IsFocused) { return false; }

            Input.keyboardState = Window.Instance.KeyboardState;
            Input.mouseState = Window.Instance.MouseState;

            currentKey.Index = ID;
            currentKey.key = key;

            if (!keyToggleList.Contains(currentKey) && !keyboardState.IsKeyDown(key))
            {
                keyToggleList.Add(currentKey);
                return true;
            }
            else
            {
                if (keyboardState.IsKeyDown(key))
                {
                    keyToggleList.Remove(currentKey);
                    return false;
                }
                return false;
            }
        }

        /// <summary>
        /// If Key is Pressed
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static bool GetKey(Keys key)
        {
            if (!Window.Instance.IsFocused) { return false; }

            Input.keyboardState = Window.Instance.KeyboardState;
            Input.mouseState = Window.Instance.MouseState;

            return keyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// If MouseButtom is Pressed
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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

    public struct CurrentKey
    {
        public int Index;
        public Keys key;

        public CurrentKey(int teste, Keys key)
        {
            this.Index = teste;
            this.key = key;
        }
    }
    public struct CurrentButtom
    {
        public int Index;
        public MouseButton Buttom;

        public CurrentButtom(int teste, MouseButton Buttom)
        {
            this.Index = teste;
            this.Buttom = Buttom;
        }
    }
}
