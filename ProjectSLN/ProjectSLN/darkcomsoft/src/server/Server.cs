using Projectsln.darkcomsoft.src.world;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.server
{
    public class Server : BuildTypeBase
    {
        public Server()
        {
            StartGameServer();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }

        private void StartGameServer()
        {
            WorldManager.SpawnWorld<SatrillesWorld>();
        }
    }
}
