using Lidgren.Network;
using OpenTK.Mathematics;
using ProjectIND.darkcomsoft.src.debug;
using ProjectIND.darkcomsoft.src.engine.gameobject;
using ProjectIND.darkcomsoft.src.entity;
using ProjectIND.darkcomsoft.src.misc;
using ProjectIND.darkcomsoft.src.network;
using ProjectIND.darkcomsoft.src.resources;
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
        private bool v_isPlaying = false;

        #region Player
        public PlayerEntity v_playerentity { get; private set; }
        public static bool v_playerAlive { get; private set; }
        public static Vector3d v_playerPosition = Vector3d.Zero;
        #endregion


        public Game()
        {
            instance = this;

            ResourcesManager.instance.LoadResources();

            CreateStartWorlds();
        }

        protected override void OnDispose()
        {
            WorldManager.DestroyAllWorlds();

            v_playerentity = null;

            instance = null;
            base.OnDispose();
        }

        private void CreateStartWorlds()
        {
            LoadMainMenu();
        }

        public void StartSinglePlayer()
        {
            v_isPlaying = true;
            JoinGameWorld();
        }

        public static void Disconnect()
        {
            instance.v_isPlaying = false;
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

        #region PlayerManager
        public static void SpawnPlayer(World worldToSpawn)
        {
            if (instance.v_playerentity != null) { return; }

            PlayerEntity player = (PlayerEntity)GameObject.SpawnObject<PlayerEntity>(worldToSpawn);
            instance.v_playerentity = player;

            v_playerAlive = true;
        }

        public static void KillPlayer(PlayerEntity playerEntity, bool instant = false)
        {
            if (instance.v_playerentity == null) { return; }
            if (playerEntity.v_isAlive) { return; }//if is alive, means the entity of the player is not ready ot die

            GameObject.DestroyObject(playerEntity, instant);

            v_playerAlive = false;
            instance.v_playerentity = null;
        }

        public static void KillPlayer(bool instant = false)
        {
            if (instance.v_playerentity == null) { return; }
            if (instance.v_playerentity.v_isAlive) { return; }
            
            GameObject.DestroyObject(instance.v_playerentity, instant);

            v_playerAlive = false;
            instance.v_playerentity = null;
        }
        #endregion
    }
}
