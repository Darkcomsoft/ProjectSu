using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Projectsln.darkcomsoft.src.entity;

namespace Projectsln.darkcomsoft.src.network
{
    public class NetworkBase : ClassBase
    {
        protected NetPeer m_peer;
        protected NetPeerStatistics _peerStatistics;

        public virtual void Tick() { }

        public virtual void Spawn(Entity entity) { }
        public virtual void Destroy(Entity entity) { }

        protected override void OnDispose()
        {
            base.OnDispose();
        }

        public NetPeer getPeer { get { return m_peer; } }
        public NetServer PeerServer { get { return (NetServer)m_peer; } }
        public NetClient PeerClient { get { return (NetClient)m_peer; } }
        public NetPeerStatistics NetworkStatistics { get { return _peerStatistics; } }
    }
}
