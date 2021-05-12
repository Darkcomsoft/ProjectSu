using Projectsln.darkcomsoft.src.debug;
using Projectsln.darkcomsoft.src.engine.render;
using Projectsln.darkcomsoft.src.world;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Projectsln.darkcomsoft.src.network;
using Projectsln.darkcomsoft.src.entity.managers;
using Projectsln.darkcomsoft.src.engine;
using ProjectSLN.darkcomsoft.src.engine.gameobject;

namespace Projectsln.darkcomsoft.src.entity
{
    /// <summary>
    /// Entity is the base entity class for all entity, All entity is sync over network.
    /// </summary>
    public abstract class Entity : GameObject
    {
        protected int m_regionId { get; private set; }//this the in world region, sync only player inside the region withothers
        private long m_Owner;
        private int m_ViewID = 0;
        private NetDeliveryMethod m_DefaultNetDeliveryMethod = NetDeliveryMethod.Unreliable;
        private Dictionary<string, RPCALL> _methodlist;

        private bool m_IsEntityReady;//if the entity is ready for do send RPC, if the entity is spaned on the network

        protected override void OnAwake()
        {
            _methodlist = new Dictionary<string, RPCALL>();
            base.OnAwake();
        }

        public void SetupEntityNetcode(int id, long owner)
        {
            m_ViewID = id;
            m_Owner = owner;

            m_IsEntityReady = true;
            NetworkManager.AddEntityNet(this);
            OnStart();
        }

        protected override void OnDispose()
        {
            m_IsEntityReady = false;

            NetworkManager.RemoveEntityNet(this);
            _methodlist.Clear();

            _methodlist = null;
            base.OnDispose();
        }


        protected override void OnTick()
        {
            if (isMine && m_IsEntityReady)
            {
                TickMyEntity();
            }
            base.OnTick();
        }

        /// <summary>
        /// is called Every frame if the entity is owned by the player
        /// </summary>
        protected virtual void TickMyEntity() { }

        public long getOwner { get { return m_Owner; } }
        public int getViewId { get { return m_ViewID; } }
        public int getRegionID { get { return m_regionId; } }
        public bool isReady { get { return m_IsEntityReady; } }
        public bool isMine
        {
            get
            {
                return NetworkManager.IsMine(m_Owner);
            }
        }
    }
}