using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.client
{
    /// <summary>
    /// Game Settings like, graphics, sound, controls etc.
    /// </summary>
    public static class GameSettings
    {
        private static float m_GuiScale = 1;
        public static int TargetFrameRate = 60;

        //Interations
        public static Keys INVENTORY_KEY = Keys.I;

        //Movements
        public static Keys MOVEFRONT_KEY = Keys.W;
        public static Keys MOVEBACK_KEY = Keys.S;
        public static Keys MOVELEFT_KEY = Keys.A;
        public static Keys MOVERIGHT_KEY = Keys.D;


        //Misc
        public static Keys DEVCONSOLE_KEY = Keys.F1;
        public static Keys DEBUGSCREEN_KEY = Keys.F2;


        public static float GuiScale { get { return m_GuiScale; } set { m_GuiScale = value; if (m_GuiScale < 0.1f) { m_GuiScale = 0.1f; } } }
    }
}
