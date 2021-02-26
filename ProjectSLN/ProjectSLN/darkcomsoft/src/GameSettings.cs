using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src
{
    /// <summary>
    /// Game Settings like, graphics, sound, controls etc.
    /// </summary>
    public static class GameSettings
    {
        public static int GuiScale = 1;
        public static int TargetFrameRate = 60;

        //Interations
        public static Keys INVENTORY_KEY = Keys.I;

        //Movements
        public static Keys MOVEFOWARD_KEY = Keys.W;


        //Misc
        public static Keys DEVCONSOLE_KEY = Keys.F1;
        public static Keys DEBUGSCREEN_KEY = Keys.F2;
    }
}
