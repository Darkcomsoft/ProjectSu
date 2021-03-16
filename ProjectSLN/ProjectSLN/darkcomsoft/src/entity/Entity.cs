using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.engine.render;
using Projectsln.darkcomsoft.src.world;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Projectsln.darkcomsoft.src.network;
using Projectsln.darkcomsoft.src.entity.managers;

namespace Projectsln.darkcomsoft.src.entity
{
    /// <summary>
    /// Entity is the base entity class for all entity, All entity is sync in network.
    /// </summary>
    public class Entity : ClassBase
    {
        private Transform m_transform;
        private bool m_removed = false;
        private bool m_visible = false;
        private bool m_usefrustum = true;
        private bool m_IsEntityReady = false;//if the entity is ready for do send RPC, if the entity is spaned on the network

        private World m_world;//world of this entity is in
        protected int m_regionId = 0;//this the in world region, is use to save netbanda, because this sync only player inside the region withouthers
        private long m_Owner;
        private int m_ViewID = 0;
        private NetDeliveryMethod m_DefaultNetDeliveryMethod = NetDeliveryMethod.Unreliable;
        private Dictionary<string, RPCALL> _methodlist;

        public Entity() { }

        public void Start(World world)
        {
            m_transform = new Transform();
            this.m_world = world;

            _methodlist = new Dictionary<string, RPCALL>();

            OnAwake();
        }

        public void SetupEntityNetcode(int id, long owner)
        {
            m_ViewID = id;
            m_Owner = owner;

            m_IsEntityReady = true;
            NetworkManager.AddEntityNet(this);
            OnStart();
        }

        public void Tick() { if (m_IsEntityReady && !m_removed) { doTick(); } }

        public void DestroyThis()
        {
            m_removed = true;
            m_IsEntityReady = false;
        }

        private void doTick()
        {
            m_visible = false;

            if (m_usefrustum)
            {
                DoCheckIfVisible();
            }

            OnTick();//call tick function anyway if is visible or not, let this to the entity to decide do what when is visible or not
        }

        private void DoCheckIfVisible()
        {
            if (Camera.main == null) { return; }

            //I DONT KNOW BUT THIS FUNCTIONS FRUSTUM NEED SOME OPTIMIZATION, I DON'T KNOW DO A LOOK
            Frustum.CalculateFrustum(Camera.main.GetProjectionMatrix(), transform.GetTransformWorld);

            if (Frustum.VolumeVsFrustum(transform.Position, transform.VolumeSize))
            {
                m_visible = true;
            }
        }

        protected override void OnDispose()
        {
            NetworkManager.RemoveEntityNet(this);

            m_IsEntityReady = false;

            transform.Dispose();

            _methodlist.Clear();

            m_transform = null;
            m_world = null;
            _methodlist = null;
            base.OnDispose();
        }

        /// <summary>
        /// If is true, is gone tick only if is visible by the camera, if is false, dont need to tick if is visible by the camera, true only recomended in static no render entity
        /// </summary>
        /// <param name="value"></param>
        public void UseFrustum(bool value)
        {
            m_usefrustum = value;
        }

        /// <summary>
        /// Called every frame
        /// </summary>
        protected virtual void OnTick() { }
        /// <summary>
        ///  Called when entity is setup in network, this set up the Network and Call this
        /// </summary>
        protected virtual void OnStart() { }
        /// <summary>
        /// Called when class instance is created, before this the net is setup and Call the OnStart, Use this if need to do before network entity start
        /// </summary>
        protected virtual void OnAwake() { }
        /// <summary>
        /// When become visible to a camera, is visible in camera View port
        /// </summary>
        protected virtual void OnBecomeVisible() { }
        /// <summary>
        /// When become invisible to a camera, is not visible in camera View port anymore
        /// </summary>
        protected virtual void OnBecomeInvisible() { }

        public Transform transform { get { return m_transform; } }
        public World GetWorld { get { return m_world; } }
        public bool isRemoved { get { return m_removed; } }
        public bool isVisible { get { return m_visible; } }
        public bool isReady { get { return m_IsEntityReady; } }
        public long getOwner { get { return m_Owner; } }
        public int getViewId { get { return m_ViewID; } }
        public int getRegionID { get { return m_regionId; } }
        public bool isMine
        {
            get
            {
                return NetworkManager.IsMine(m_Owner);
            }
        }
    }
}
