using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.Engine
{
    public static class MouseCursor
    {
        private static bool _isLocked;

        public static bool MouseLocked { get { return _isLocked; } }

        public static void LockCursor()
        {
            Window.Instance.CursorVisible = false;
            _isLocked = true;
        }

        public static void UnLockCursor()
        {
            _isLocked = false;
            Window.Instance.CursorVisible = true;
        }

        public static void CursorLockPosition()
        {
            if (_isLocked)
            {
                Mouse.SetPosition(Window.Instance.X + Window.Instance.Width / 2f, Window.Instance.Y + Window.Instance.Height / 2f);
            }
        }
    }
}
