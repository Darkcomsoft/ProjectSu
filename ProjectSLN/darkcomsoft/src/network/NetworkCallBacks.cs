using System;
using System.Collections.Generic;
using System.Text;
using Lidgren.Network;

namespace ProjectIND.darkcomsoft.src.network
{
    public class NetworkCallBacks : ClassBase
    {
        private static NetworkCallBacks m_instance;

        public static Action<NetConnection> OnPlayerDisconnect;
        public static Action<NetConnection> OnPlayerConnect;
        public static Action<string, NetConnection> PlayerApproval;
        public static Action OnDisconnect;
        public static Action OnConnect;
        public static Action OnServerStart;
        public static Action OnServerStop;
        public static Action OnClientStart;
        /// <summary>
        /// <see cref="OnReceivedServerData"/> is called when a client receive start data, like the entire list of entitys, this means you cliet have every thing to play
        /// </summary>
        public static Action OnReceivedServerData;

        public NetworkCallBacks()
        {
            m_instance = this;
        }

        protected override void OnDispose()
        {
            OnPlayerDisconnect = null;
            OnPlayerConnect = null;
            PlayerApproval = null;
            OnDisconnect = null;
            OnConnect = null;
            OnServerStart = null;
            OnServerStop = null;
            OnClientStart = null;
            OnReceivedServerData = null;

            m_instance = null;
            base.OnDispose();
        }

        public static NetworkCallBacks instance { get { return m_instance; } }
    }
}
