using ProjectSLN.darkcomsoft.src.debug;
using ProjectSLN.darkcomsoft.src.engine.gameobject;
using ProjectSLN.darkcomsoft.src.entity;
using ProjectSLN.darkcomsoft.src.misc;
using ProjectSLN.darkcomsoft.src.world;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ProjectSLN.darkcomsoft.src.game
{
    public class PlayerManager : ClassBase
    {
        public static PlayerManager instance { get; private set; }
        public PlayerEntity m_playerentity { get; private set; }

        public PlayerManager()
        {
            instance = this;
        }

        protected override void OnDispose()
        {
            m_playerentity = null;
            instance = null;
            base.OnDispose();
        }

        public void SpawnPlayer(World worldToSpawn)
        {
            if (m_playerentity != null) { return; }

            PlayerEntity player = (PlayerEntity)GameObject.SpawnObject<PlayerEntity>(worldToSpawn);
            m_playerentity = player;
        }

        [EngineOnly]
        public static void KillPlayer(PlayerEntity playerEntity, [CallerMemberName]string caller = "")
        {
            instance.m_playerentity = null;
            print("Method Caller: " + caller);
        }
    }
}
