using Lidgren.Network;
using Projectsln.darkcomsoft.src.debug;
using Projectsln.darkcomsoft.src.network;
using Projectsln.darkcomsoft.src.world;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.server
{
    public class Server : BuildTypeBase
    {
        public static Server instance { get; private set; }
        private bool m_gameisDisconnected = false;

        public Server()
        {
            instance = this;
            StartGameServer();
        }

        protected override void OnDispose()
        {
            UnLinkNetworkCallBacks();

            if (!m_gameisDisconnected)
            {
                StopServer();
            }

            WorldManager.DestroyAllWorlds();

            instance = null;
            base.OnDispose();
        }

        private void StartGameServer()
        {
            LinkNetworkCallBacks();
            Application.SetNetworkType(enums.AppNetworkType.ClientSinglePlayerServer);
            NetworkManager.CreateServer(127001, 25000, 10);
        }

        #region NetWorkStuff
        private void LinkNetworkCallBacks()
        {
            NetworkCallBacks.OnPlayerConnect += OnPlayerConnect;
            NetworkCallBacks.OnPlayerDisconnect += OnPlayerDisconnect;
            NetworkCallBacks.OnServerStart += OnServerStart;
            NetworkCallBacks.PlayerApproval += PlayerApproval;
            NetworkCallBacks.OnServerStop += OnServerStop;
        }

        private void UnLinkNetworkCallBacks()
        {
            NetworkCallBacks.OnPlayerConnect -= OnPlayerConnect;
            NetworkCallBacks.OnPlayerDisconnect -= OnPlayerDisconnect;
            NetworkCallBacks.OnServerStart -= OnServerStart;
            NetworkCallBacks.PlayerApproval -= PlayerApproval;
            NetworkCallBacks.OnServerStop -= OnServerStop;
        }

        public void OnPlayerDisconnect(NetConnection netConnection) { }

        public void OnPlayerConnect(NetConnection netConnection) { }

        public void PlayerApproval(string data, NetConnection netConnection)
        {
            if (data == NetConfig.SecretKey)
            {
                netConnection.Approve();
            }
            else
            {
                netConnection.Deny("Sorry your data isnt equal to server!");
            }
        }

        public void OnServerStart()
        {
            CreateWorlds();
            Debug.Log("GameServer Started!", "GAMESERVER");
        }

        public void OnServerStop()
        {
            ServerMain.Exit();
            Debug.Log("GameServer Stoped!", "GAMESERVER");
        }
        #endregion

        /// <summary>
        /// Load all playable worlds, the server needs to all world be loaded to be able to do some network stuff
        /// </summary>
        private void CreateWorlds()
        {
            WorldManager.SpawnWorld<SatrillesWorld>();
        }

        public static void StopServer()
        {
            instance.m_gameisDisconnected = true;
            NetworkManager.Disconnect();
        }
    }
}
