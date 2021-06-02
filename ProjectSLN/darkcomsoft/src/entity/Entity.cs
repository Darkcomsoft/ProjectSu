using ProjectIND.darkcomsoft.src.debug;
using ProjectIND.darkcomsoft.src.engine.render;
using ProjectIND.darkcomsoft.src.world;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using ProjectIND.darkcomsoft.src.network;
using ProjectIND.darkcomsoft.src.entity.managers;
using ProjectIND.darkcomsoft.src.engine;
using ProjectIND.darkcomsoft.src.engine.gameobject;

namespace ProjectIND.darkcomsoft.src.entity
{
    /// <summary>
    /// Entity is the base entity class for all entity, All entity is sync over network.
    /// </summary>
    public abstract class Entity : GameObject
    {
        protected override void OnAwake()
        {
            base.OnAwake();
        }

        protected override void OnDispose()
        {
            //NetworkManager.RemoveEntityNet(this);
            base.OnDispose();
        }

        public static void MoveEntityToWorld(Entity entity, World world)
        {
            entity.ChangeWorld(world);
        }

        #region OldStuffOfNetwork
        [Obsolete]
        protected int v_regionId { get; private set; }//this the in world region, sync only player inside the region withothers
        [Obsolete]
        private long v_Owner;
        [Obsolete]
        private int v_ViewID = 0;
        [Obsolete]
        private NetDeliveryMethod v_DefaultNetDeliveryMethod = NetDeliveryMethod.Unreliable;
        [Obsolete]
        private Dictionary<string, RPCALL> _methodlist;
        [Obsolete]
        private bool v_IsEntityReady;//if the entity is ready for do send RPC, if the entity is spaned on the network
        [Obsolete]
        public void SetupEntityNetcode(int id, long owner)
        {
            v_ViewID = id;
            v_Owner = owner;

            v_IsEntityReady = true;
            //NetworkManager.AddEntityNet(this);
            OnStart();
        }

        [Obsolete]
        public long getOwner { get { return v_Owner; } }
        [Obsolete]
        public int getViewId { get { return v_ViewID; } }
        [Obsolete]
        public int getRegionID { get { return v_regionId; } }
        [Obsolete]
        public bool isReady { get { return v_IsEntityReady; } }
        [Obsolete]
        public bool isMine
        {
            get
            {
                return NetworkManager.IsMine(v_Owner);
            }
        }
        #endregion
    }
}