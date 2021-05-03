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
            WorldManager.SpawnWorld<SystemWorld>();

        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
