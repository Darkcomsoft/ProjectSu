using Projectsln.darkcomsoft.src.engine;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;
using Projectsln.darkcomsoft.src.client;

namespace Projectsln.darkcomsoft.src.world
{
    /// <summary>
    /// Satrilles World is the main world of the games history, the normal world, start world etc.
    /// </summary>
    public class SatrillesWorld : World
    {
        public override void Start()
        {

            base.Start();
        }

        public override void Tick()
        {
            if (Input.GetKeyDown(Keys.Escape))
            {
                Game.Disconnect();
            }
            base.Tick();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
