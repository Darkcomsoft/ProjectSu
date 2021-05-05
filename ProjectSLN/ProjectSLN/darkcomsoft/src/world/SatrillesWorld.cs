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
            if (!Application.IsServer)
            {
                debug.Debug.Log("SatrillesWorld Is Created!", "CLIENT");
            }
            else
            {
                debug.Debug.Log("SatrillesWorld Is Created!", "SERVER");
            }
            base.Start();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }

        protected override void TickClient()
        {
            if (Input.GetKeyDown(Keys.Escape))
            {
                Game.Disconnect();
            }
            base.TickClient();
        }
    }
}
