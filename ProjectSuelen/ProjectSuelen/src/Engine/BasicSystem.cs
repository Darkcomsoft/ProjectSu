using ProjectSuelen.src.Engine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.Engine
{
    /// <summary>
    /// BasicSystem is the most basic systems, EX: GUI, OS operations, is gona still load until engine(Window) are open, and runing
    /// </summary>
    public class BasicSystem : ClassBase
    {
        private GUI guiManager;
        private BasicScreen basicScreen;

        public BasicSystem()
        {
            guiManager = new GUI();
            basicScreen = new BasicScreen();
        }

        public void Tick()
        {
            guiManager.Tick();
        }

        public void TickRender()
        {
            guiManager.TickRender();
        }

        protected override void OnDispose()
        {
            if (basicScreen != null)
            {
                basicScreen.Dispose();
            }

            if (guiManager != null)
            {
                guiManager.Dispose();
            }
            base.OnDispose();
        }
    }
}
