using System;
using System.Collections.Generic;
using System.Text;
using Lidgren.Network;
using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.misc;

namespace Projectsln.darkcomsoft.src.network
{
    public class NetworkManager : ClassBase
    {
        /// <summary>
        /// This i the default NetDeliveryMethod, used for the netcode systems, all entitys have ther one NetDeliveryMethod
        /// </summary>
        public const NetDeliveryMethod NetBaseDeliveryMethod = NetDeliveryMethod.ReliableOrdered;

        private static NetworkManager m_instance;
        private NetworkType m_netType;
        private NetworkBase network;

        public NetworkManager()
        {
            m_instance = this;
        }

        public static void CreateServer(long ip, int port, int maxplayers)
        {
            instance.doCreateServer(ip, port, maxplayers);
        }

        /// <summary>
        /// Connect Using Numeric ip
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="password"></param>
        public static void Connect(long ip, int port, string password)
        {
            instance.doConnectClient(ip, port, password);
        }

        /// <summary>
        /// Connect Using Web URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="port"></param>
        /// <param name="password"></param>
        public static void Connect(string url, int port, string password)
        {
            instance.doConnectClient(long.Parse(Utilits.GetipfromURL(url)), port, password);
        }

        public static void Disconnect()
        {
            instance.doDisconnect();
        }

        public void Tick()
        {
            if (network != null)
            {
                network.Tick();
            }
        }

        private void doDisconnect()
        {
            m_netType = NetworkType.none;

            if (network != null)
            {
                network.Dispose();
                network = null;
            }
        }

        private void doCreateServer(long ip, int port, int maxplayers)
        {
            if (m_netType != NetworkType.none) { Debug.Log("Is allready runing a network instance: " + m_netType, "NETWORK");  return; }

            m_netType = NetworkType.Server;
            network = new NetworkServer(ip, port, maxplayers);
        }

        private void doConnectClient(long ip, int port, string pass)
        {
            if (m_netType != NetworkType.none) { Debug.Log("Is allready runing a network instance: " + m_netType, "NETWORK"); return; }

            m_netType = NetworkType.Client;
            network = new NetworkClient(ip, port, pass);
        }

        protected override void OnDispose()
        {
            Disconnect();//Call disconnect any way if the network is finished.

            m_instance = null;
            base.OnDispose();
        }

        public static NetworkManager instance { get { return m_instance; } }
    }

    public enum NetworkType
    {
        none, Server, Client
    }

    public static class NetConfig
    {
        public static string SecretKey = "secret";
        public static int DefaultOutgoingMessageCapacity = 99999;
        public static int SendBufferSize = 131071;
        public static float ConnectionTimeout = 50;
        public static bool AcceptConnection = true;
        public static bool UseMessageRecycling = true;
        public static string AppIdentifier = Application.AppName + Application.Version;
        /// <summary>
        /// TiketRate is the milliseconds to Thread stop and continue. 15 is Recommended
        /// </summary>
        public static int TickRate = 15;
        /// <summary>
        /// If is true Unat is false, if is false Unat is true.
        /// </summary>
        public static bool DedicatedServer = false;
    }
}
