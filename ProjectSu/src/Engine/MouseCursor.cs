using OpenTK.Input;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
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
            Window.Instance.CursorGrabbed = true;
            _isLocked = true;
        }

        public static void UnLockCursor()
        {
            _isLocked = false;
            Window.Instance.CursorVisible = true;
            Window.Instance.CursorGrabbed = false;
        }
    }
}
