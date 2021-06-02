using ProjectIND.darkcomsoft.src.engine.window;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.engine
{
    public static class CursorManager//Need fix the cursor is not being stuck on the center of the screen
    {
        private static bool v_isLocked;

        public static void Lock()
        {
            v_isLocked = true;
            WindowMain.Instance.CursorGrabbed = true;
            //WindowMain.Instance.CursorVisible = false;
        }

        public static void UnLock()
        {
            v_isLocked = false;
            WindowMain.Instance.CursorGrabbed = false;
            //WindowMain.Instance.CursorVisible = true;
        }

        public static void Tick()
        {
            if (v_isLocked)
            {
                WindowMain.Instance.CursorGrabbed = true;
                WindowMain.Instance.CursorVisible = false;
            }
            else
            {
                WindowMain.Instance.CursorGrabbed = false;
                WindowMain.Instance.CursorVisible = true;
            }
        }

        public static bool isLocked { get { return v_isLocked; } }
    }
}
