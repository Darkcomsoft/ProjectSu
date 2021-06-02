using OpenTK.Mathematics;
using ProjectIND.darkcomsoft.src.debug;
using ProjectIND.darkcomsoft.src.engine.gameobject;
using ProjectIND.darkcomsoft.src.entity;
using ProjectIND.darkcomsoft.src.misc;
using ProjectIND.darkcomsoft.src.world;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ProjectIND.darkcomsoft.src.game
{
    public class PlayerManager : ClassBase
    {
        public static PlayerManager instance { get; private set; }
        public PlayerEntity v_playerentity { get; private set; }

        public static Vector3d v_playerPosition = Vector3d.Zero;

        public PlayerManager()
        {
            instance = this;
        }

        protected override void OnDispose()
        {
            v_playerentity = null;
            instance = null;
            base.OnDispose();
        }

        public void SpawnPlayer(World worldToSpawn)
        {
            if (v_playerentity != null) { return; }

            PlayerEntity player = (PlayerEntity)GameObject.SpawnObject<PlayerEntity>(worldToSpawn);
            v_playerentity = player;
        }

        [EngineOnly]
        public static void KillPlayer(PlayerEntity playerEntity, [CallerMemberName]string caller = "")
        {
            instance.v_playerentity = null;
            print("Method Caller: " + caller);
        }
    }
}
