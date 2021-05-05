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
            NetworkCallBacks.instance.OnClientStart += OnClientStart;
            NetworkCallBacks.instance.OnConnect += OnConnect;
            NetworkCallBacks.instance.OnPlayerConnect += OnPlayerConnect;
            NetworkCallBacks.instance.OnPlayerDisconnect += OnPlayerDisconnect;
            NetworkCallBacks.instance.OnServerStart += OnServerStart;
            NetworkCallBacks.instance.PlayerApproval += PlayerApproval;

            NetworkCallBacks.instance.OnDisconnect += OnDisconnect;
            NetworkCallBacks.instance.OnServerStop += OnServerStop;
        }

        private void UnLinkNetworkCallBacks()
        {
            NetworkCallBacks.instance.OnClientStart -= OnClientStart;
            NetworkCallBacks.instance.OnConnect -= OnConnect;
            NetworkCallBacks.instance.OnPlayerConnect -= OnPlayerConnect;
            NetworkCallBacks.instance.OnPlayerDisconnect -= OnPlayerDisconnect;
            NetworkCallBacks.instance.OnServerStart -= OnServerStart;
            NetworkCallBacks.instance.PlayerApproval -= PlayerApproval;

            NetworkCallBacks.instance.OnDisconnect -= OnDisconnect;
            NetworkCallBacks.instance.OnServerStop -= OnServerStop;
        }

        public void StartSinglePlayer()
        {
            NetworkManager.CreateServer(127001, 25000, 10);

            LinkNetworkCallBacks();
        }

        public void Connect(string ip, int port)
        {
            NetworkManager.Connect(int.Parse(ip), port);

            LinkNetworkCallBacks();
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
            Debug.Log("Client Connect!", "GAME");
            JoinGameWorld();
        }

        public void OnServerStart()
        {
            Debug.Log("Client Server Started!", "GAME");
            JoinGameWorld();
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
