using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.network
{
    public class NetworkServer : NetworkBase
    {
        public NetworkServer(long ip, int port, int maxplayers)
        {

        }

        public override void Tick()
        {
            base.Tick();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
