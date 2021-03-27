using Projectsln.darkcomsoft.src.engine.window;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.engine
{
    public static class CursorManager
    {
        private static bool m_isLocked;

        public static void Lock()
        {
            m_isLocked = true;
            WindowMain.Instance.CursorGrabbed = true;
            //WindowMain.Instance.CursorVisible = false;
        }

        public static void UnLock()
        {
            m_isLocked = false;
            WindowMain.Instance.CursorGrabbed = false;
            //WindowMain.Instance.CursorVisible = true;
        }

        public static void Tick()
        {
            if (m_isLocked)
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

        public static bool isLocked { get { return m_isLocked; } }
    }
}
