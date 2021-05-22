using ProjectSLN.darkcomsoft.src.debug;
using ProjectSLN.darkcomsoft.src.engine.render;
using ProjectSLN.darkcomsoft.src.world;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using ProjectSLN.darkcomsoft.src.network;
using ProjectSLN.darkcomsoft.src.entity.managers;
using ProjectSLN.darkcomsoft.src.engine;
using ProjectSLN.darkcomsoft.src.engine.gameobject;

namespace ProjectSLN.darkcomsoft.src.entity
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

        #region OldStuffOfNetwork
        [Obsolete]
        protected int m_regionId { get; private set; }//this the in world region, sync only player inside the region withothers
        [Obsolete]
        private long m_Owner;
        [Obsolete]
        private int m_ViewID = 0;
        [Obsolete]
        private NetDeliveryMethod m_DefaultNetDeliveryMethod = NetDeliveryMethod.Unreliable;
        [Obsolete]
        private Dictionary<string, RPCALL> _methodlist;
        [Obsolete]
        private bool m_IsEntityReady;//if the entity is ready for do send RPC, if the entity is spaned on the network
        [Obsolete]
        public void SetupEntityNetcode(int id, long owner)
        {
            m_ViewID = id;
            m_Owner = owner;

            m_IsEntityReady = true;
            //NetworkManager.AddEntityNet(this);
            OnStart();
        }

        [Obsolete]
        public long getOwner { get { return m_Owner; } }
        [Obsolete]
        public int getViewId { get { return m_ViewID; } }
        [Obsolete]
        public int getRegionID { get { return m_regionId; } }
        [Obsolete]
        public bool isReady { get { return m_IsEntityReady; } }
        [Obsolete]
        public bool isMine
        {
            get
            {
                return NetworkManager.IsMine(m_Owner);
            }
        }
        #endregion
    }
}