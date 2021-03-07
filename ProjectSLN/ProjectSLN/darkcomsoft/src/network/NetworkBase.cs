using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.network
{
    public class NetworkBase : ClassBase
    {
        private NetPeer m_peer;

        public virtual void Tick() { }

        protected override void OnDispose()
        {
            base.OnDispose();
        }

        public NetPeer MyNetPeer { get { return m_peer; } }
    }
}
