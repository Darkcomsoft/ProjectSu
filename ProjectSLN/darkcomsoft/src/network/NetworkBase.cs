using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using ProjectIND.darkcomsoft.src.entity;
using ProjectIND.darkcomsoft.src.engine.gameobject;

namespace ProjectIND.darkcomsoft.src.network
{
    public class NetworkBase : ClassBase
    {
        protected NetPeer v_peer;
        protected NetPeerStatistics _peerStatistics;

        public virtual void Tick() { }

        public virtual void Spawn(Entity entity) { }
        public virtual void Destroy(Entity entity) { }

        protected override void OnDispose()
        {
            base.OnDispose();
        }

        public NetPeer getPeer { get { return v_peer; } }
        public NetServer PeerServer { get { return (NetServer)v_peer; } }
        public NetClient PeerClient { get { return (NetClient)v_peer; } }
        public NetPeerStatistics NetworkStatistics { get { return _peerStatistics; } }
    }
}
