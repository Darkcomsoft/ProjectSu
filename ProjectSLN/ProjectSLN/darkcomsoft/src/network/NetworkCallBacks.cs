using System;
using System.Collections.Generic;
using System.Text;
using Lidgren.Network;

namespace ProjectSLN.darkcomsoft.src.network
{
    public class NetworkCallBacks : ClassBase
    {
        private static NetworkCallBacks m_instance;

        public static event Action<NetConnection> OnPlayerDisconnect;
        public static event Action<NetConnection> OnPlayerConnect;
        public static event Action<string, NetConnection> PlayerApproval;
        public static event Action OnDisconnect;
        public static event Action OnConnect;
        public static event Action OnServerStart;
        public static event Action OnServerStop;
        public static event Action OnClientStart;
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
