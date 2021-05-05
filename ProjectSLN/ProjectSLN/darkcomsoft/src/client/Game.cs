using Lidgren.Network;
using Projectsln.darkcomsoft.src.debug;
using Projectsln.darkcomsoft.src.network;
using Projectsln.darkcomsoft.src.world;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.client
{
    /// <summary>
    /// <see cref="Game"/> used to controll the game logic, like join a world, etc. <see cref="Client"/> is more for the Client Engine stuff like rendering
    /// </summary>
    public class Game : ClassBase
    {
        public static Game instance { get; private set; }

        private bool m_gameisDisconnected = false;

        public Game()
        {
            instance = this;

            CreateStartWorlds();
        }

        protected override void OnDispose()
        {
            UnLinkNetworkCallBacks();

            if (!m_gameisDisconnected)
            {
                Disconnect();
            }

            WorldManager.DestroyAllWorlds();

            instance = null;
            base.OnDispose();
        }

        private void CreateStartWorlds()
        {
            WorldManager.SpawnWorld<MainMenuWorld>();//Start menu World
        }

        #region NetworkStuff
        private void LinkNetworkCallBacks()
        {
            NetworkCallBacks.OnClientStart += OnClientStart;
            NetworkCallBacks.OnConnect += OnConnect;
            NetworkCallBacks.OnPlayerConnect += OnPlayerConnect;
            NetworkCallBacks.OnPlayerDisconnect += OnPlayerDisconnect;
            NetworkCallBacks.OnServerStart += OnServerStart;
            NetworkCallBacks.PlayerApproval += PlayerApproval;

            NetworkCallBacks.OnDisconnect += OnDisconnect;
            NetworkCallBacks.OnServerStop += OnServerStop;
        }

        private void UnLinkNetworkCallBacks()
        {
            NetworkCallBacks.OnClientStart -= OnClientStart;
            NetworkCallBacks.OnConnect -= OnConnect;
            NetworkCallBacks.OnPlayerConnect -= OnPlayerConnect;
            NetworkCallBacks.OnPlayerDisconnect -= OnPlayerDisconnect;
            NetworkCallBacks.OnServerStart -= OnServerStart;
            NetworkCallBacks.PlayerApproval -= PlayerApproval;

            NetworkCallBacks.OnDisconnect -= OnDisconnect;
            NetworkCallBacks.OnServerStop -= OnServerStop;
        }

        public void StartSinglePlayer()
        {
            LinkNetworkCallBacks();
            Application.SetNetworkType(enums.AppNetworkType.ClientSinglePlayerServer);
            NetworkManager.CreateServer(127001, 25000, 10);
        }

        public void Connect(string ip, int port)
        {
            LinkNetworkCallBacks();
            Application.SetNetworkType(enums.AppNetworkType.Client);
            NetworkManager.Connect(int.Parse(ip), port);
        }

        public void OnPlayerDisconnect(NetConnection netConnection)
        {

        }

        public void OnPlayerConnect(NetConnection netConnection)
        {

        }

        public void PlayerApproval(string naosei, NetConnection netConnection)
        {

        }

        public void OnConnect()
        {
            JoinGameWorld();
            Debug.Log("Client Connect!", "GAME");
        }

        public void OnServerStart()
        {
            JoinGameWorld();
            Debug.Log("Client Server Started!", "GAME");
        }

        public void OnClientStart()
        {
            Debug.Log("Client Started!", "GAME");
        }

        public void OnDisconnect()
        {
            Client.instance.RestartGame();
            Debug.Log("Client Disconnected!", "GAME");
        }

        public void OnServerStop()
        {
            Client.instance.RestartGame();
            Debug.Log("Client Server Stoped!", "GAME");
        }
        #endregion

        #region WorldStuff
        private void JoinGameWorld()
        {
            WorldManager.DestroyWorld(WorldManager.GetWorld<MainMenuWorld>());
            WorldManager.SpawnWorld<SatrillesWorld>();
        }

        #endregion

        public static void Disconnect()
        {
            instance.m_gameisDisconnected = true;
            NetworkManager.Disconnect();
        }
    }
}
