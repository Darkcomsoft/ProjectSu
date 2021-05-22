using Lidgren.Network;
using ProjectSLN.darkcomsoft.src.debug;
using ProjectSLN.darkcomsoft.src.network;
using ProjectSLN.darkcomsoft.src.world;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSLN.darkcomsoft.src.client
{
    /// <summary>
    /// <see cref="Game"/> used to controll the game logic, like join a world, etc. <see cref="Client"/> is more for the Client Engine stuff like rendering
    /// </summary>
    public class Game : ClassBase
    {
        public static Game instance { get; private set; }
        private bool m_isPlaying = false;

        public Game()
        {
            instance = this;

            CreateStartWorlds();
        }

        protected override void OnDispose()
        {
            WorldManager.DestroyAllWorlds();

            instance = null;
            base.OnDispose();
        }

        private void CreateStartWorlds()
        {
            WorldManager.SpawnWorld<MainMenuWorld>();//Start menu World
        }

        public void StartSinglePlayer()
        {
            m_isPlaying = true;
            JoinGameWorld();
        }

        #region WorldStuff
        private void JoinGameWorld()
        {
            WorldManager.DestroyWorld(WorldManager.GetWorld<MainMenuWorld>());
            WorldManager.SpawnWorld<SatrillesWorld>();
        }

        #endregion

        public static void Disconnect()
        {
            instance.m_isPlaying = false;
            WorldManager.DestroyAllWorlds();
            WorldManager.SpawnWorld<MainMenuWorld>();
        }
    }
}
