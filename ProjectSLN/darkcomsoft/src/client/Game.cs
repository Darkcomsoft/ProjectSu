﻿using Lidgren.Network;
using ProjectIND.darkcomsoft.src.debug;
using ProjectIND.darkcomsoft.src.entity;
using ProjectIND.darkcomsoft.src.game;
using ProjectIND.darkcomsoft.src.network;
using ProjectIND.darkcomsoft.src.world;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.client
{
    /// <summary>
    /// <see cref="Game"/> used to controll the game logic, like join a world, etc. <see cref="Client"/> is more for the Client Engine stuff like rendering
    /// </summary>
    public class Game : ClassBase
    {
        public static Game instance { get; private set; }
        private bool m_isPlaying = false;

        private PlayerManager m_playerManager;

        public Game()
        {
            instance = this;

            m_playerManager = new PlayerManager();

            CreateStartWorlds();
        }

        protected override void OnDispose()
        {
            WorldManager.DestroyAllWorlds();

            m_playerManager.Dispose();
            m_playerManager = null;

            instance = null;
            base.OnDispose();
        }

        private void CreateStartWorlds()
        {
            LoadMainMenu();
        }

        public void StartSinglePlayer()
        {
            m_isPlaying = true;
            JoinGameWorld();
        }

        public static void Disconnect()
        {
            instance.m_isPlaying = false;
            instance.LoadMainMenu();
        }

        #region World's Stuff
        private void JoinGameWorld()
        {
            WorldManager.DestroyAllWorlds();
            WorldManager.SpawnWorld<SatrillesWorld>();
        }

        private void LoadMainMenu()
        {
            WorldManager.DestroyAllWorlds();
            WorldManager.SpawnWorld<MainMenuWorld>();
        }
        #endregion
    }
}
