using OpenTK.Mathematics;
using Projectsln.darkcomsoft.src.debug;
using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.entity;
using Projectsln.darkcomsoft.src.entity.managers;
using Projectsln.darkcomsoft.src.world;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;
using Projectsln.darkcomsoft.src.gui.guisystem;
using OpenTK.Windowing.Common;
using Projectsln.darkcomsoft.src.network;
using Lidgren.Network;

namespace Projectsln.darkcomsoft.src.client
{
    /// <summary>
    /// is the client, this is the class start the game stuff, like graphics, and other things
    /// </summary>
    public class Client : BuildTypeBase
    {
        private Client m_instance;

        public static Input m_input { get; private set; }
        public static Gizmo m_gizmos { get; private set; }
        public static GUI m_gui { get; private set; }

        public Client()
        {
            m_instance = this;

            m_gizmos = new Gizmo();//This is only for debug
            m_input = new Input();
            m_gui = new GUI();

            NetworkCallBacks.instance.OnClientStart += OnClientStart;
            NetworkCallBacks.instance.OnConnect += OnConnect;
            NetworkCallBacks.instance.OnDisconnect += OnDisconnect;
            NetworkCallBacks.instance.OnPlayerConnect += OnPlayerConnect;
            NetworkCallBacks.instance.OnPlayerDisconnect += OnPlayerDisconnect;
            NetworkCallBacks.instance.OnReceivedServerData += OnReceivedServerData;
            NetworkCallBacks.instance.OnServerStart += OnServerStart;
            NetworkCallBacks.instance.OnServerStop += OnServerStop;
            NetworkCallBacks.instance.PlayerApproval += PlayerApproval;

            Debug.Log("GameStarted!", "CLIENT");

            WorldManager.SpawnWorld<SystemWorld>();
            WorldManager.SpawnWorld<MainMenuWorld>();
        }

        public override void Tick()
        {
            if (Input.GetKeyDown(Keys.P))
            {
                if (!CursorManager.isLocked)
                {
                    CursorManager.Lock();
                }
                else
                {
                    CursorManager.UnLock();
                }
            }

            if (Input.GetKeyDown(GameSettings.DEBUGSCREEN_KEY))
            {
                if (Debug.isDebugEnabled)
                {
                    Debug.DisableDebug();
                }
                else
                {
                    Debug.EnableDebug();
                }
            }

            m_gui?.Tick();
            base.Tick();
        }

        public override void TickDraw()
        {
            //DrawStuuff Like 3d

            //DrawGUI
            m_gui?.Draw();
            base.TickDraw();
        }

        protected override void OnDispose()
        {
            NetworkCallBacks.instance.OnClientStart -= OnClientStart;
            NetworkCallBacks.instance.OnConnect -= OnConnect;
            NetworkCallBacks.instance.OnDisconnect -= OnDisconnect;
            NetworkCallBacks.instance.OnPlayerConnect -= OnPlayerConnect;
            NetworkCallBacks.instance.OnPlayerDisconnect -= OnPlayerDisconnect;
            NetworkCallBacks.instance.OnReceivedServerData -= OnReceivedServerData;
            NetworkCallBacks.instance.OnServerStart -= OnServerStart;
            NetworkCallBacks.instance.OnServerStop -= OnServerStop;
            NetworkCallBacks.instance.PlayerApproval -= PlayerApproval;

            m_gui?.Dispose();
            m_gui = null;

            m_gizmos?.Dispose();
            m_gizmos = null;

            m_input?.Dispose();
            m_input = null;

            m_instance = null;
            base.OnDispose();
        }

        public override void OnResize()
        {
            m_gui?.OnResize();
            base.OnResize();
        }

        public override void OnMouseMove()
        {
            m_gui?.OnMouseMove();
            base.OnMouseMove();
        }

        public override void OnMouseDown(MouseButtonEventArgs e)
        {
            m_gui?.OnMousePress(e);
            base.OnMouseDown(e);
        }

        public override void OnMouseUp(MouseButtonEventArgs e)
        {
            m_gui?.OnMouseRelease(e);
            base.OnMouseUp(e);
        }

        #region NetWork-Stuff
        public void StartSinglePlayer()
        {
            NetworkManager.CreateServer(long.Parse("127.0.0.1"), 25000, 10);
        }

        public void Connect(string ip, int port)
        {
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
        public void OnDisconnect()
        {

        }
        public void OnConnect()
        {
            JoinGameWorld();
        }
        public void OnServerStart()
        {
            JoinGameWorld();
        }
        public void OnServerStop()
        {

        }
        public void OnClientStart()
        {

        }
        public void OnReceivedServerData()
        {

        }
        #endregion

        private void JoinGameWorld()
        {
            WorldManager.DestroyWorld(WorldManager.GetWorld<MainMenuWorld>());
            WorldManager.SpawnWorld<SatrillesWorld>();
        }

        private void JoinMainMenu()
        {
            WorldManager.DestroyWorld(WorldManager.GetWorld<SatrillesWorld>());
            WorldManager.SpawnWorld<MainMenuWorld>();
        }

        public Client instance { get { return m_instance; } }
    }
}
