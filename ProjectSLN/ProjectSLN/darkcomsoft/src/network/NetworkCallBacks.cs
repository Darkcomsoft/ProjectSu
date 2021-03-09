using System;
using System.Collections.Generic;
using System.Text;
using Lidgren.Network;

namespace Projectsln.darkcomsoft.src.network
{
    public class NetworkCallBacks : ClassBase
    {
        private static NetworkCallBacks m_instance;

        public Action<NetConnection> OnPlayerDisconnect;
        public Action<NetConnection> OnPlayerConnect;
        public Action<string, NetConnection> PlayerApproval;
        public Action OnDisconnect;
        public Action OnConnect;
        public Action OnServerStart;
        public Action OnServerStop;
        public Action OnClientStart;
        public Action OnReceivedServerData;

        public NetworkCallBacks()
        {
            m_instance = this;
        }

        protected override void OnDispose()
        {
            m_instance = null;
            base.OnDispose();
        }

        public static NetworkCallBacks instance { get { return m_instance; } }
    }
}
