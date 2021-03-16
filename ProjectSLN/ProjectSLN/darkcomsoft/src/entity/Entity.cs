using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.engine.render;
using Projectsln.darkcomsoft.src.world;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Projectsln.darkcomsoft.src.network;

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
        private bool m_IsEntityReady = false;
        private bool m_isEntityNetSynce = false;//if the entity is ready for do send RPC, if the entity is spaned on the network

        private World m_world;//world of this entity is in
        protected int m_regionId = 0;//this the in world region, is use to save netbanda, because this sync only player inside the region withouthers
        private long m_Owner;
        private int m_ViewID = 0;
        private NetDeliveryMethod _DefaultNetDeliveryMethod = NetDeliveryMethod.Unreliable;

        private Dictionary<string, RPCALL> _methodlist = new Dictionary<string, RPCALL>();

        public Entity() { }

        public void Start(World world)
        {
            OnBeforeStart();

            m_transform = new Transform();
            this.m_world = world;

            OnStart();
        }

        public void SetupEntityNetcode()
        {


            m_isEntityNetSynce = true;
        }

        public void Tick() { if (m_isEntityNetSynce && !m_removed) { doTick(); } }

        public void DestroyThis()
        {
            m_removed = true;
            m_isEntityNetSynce = false;
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
        /// Called after OnBeforeStart, and all entity system. EX:transform set world var etc.
        /// </summary>
        protected virtual void OnStart() { }
        /// <summary>
        /// Called before OnStart, and before all entity start system, EX:transform set world var etc. use with carefully
        /// </summary>
        protected virtual void OnBeforeStart() { }
        /// <summary>
        /// When become visible to a camera, is visible in camera View port
        /// </summary>
        protected virtual void OnBecomeVisible() { }
        /// <summary>
        /// When become invisible to a camera, is not visible in camera View port anymore
        /// </summary>
        protected virtual void OnBecomeInvisible() { }

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
            if (m_usefrustum)
            {
                if (Camera.main == null) { return; }

                //I DONT KNOW BUT THIS FUNCTIONS FRUSTUM NEED SOME OPTIMIZATION, I DON'T KNOW DO A LOOK
                Frustum.CalculateFrustum(Camera.main.GetProjectionMatrix(), transform.GetTransformWorld);

                if (Frustum.VolumeVsFrustum(transform.Position, transform.VolumeSize))
                {
                    m_visible = true;
                }
                else
                {
                    m_visible = false;
                }
            }
            else
            {
                m_visible = false;
            }
        }

        protected override void OnDispose()
        {
            transform.Dispose();

            _methodlist.Clear();

            m_transform = null;
            m_world = null;
            _methodlist = null;
            base.OnDispose();
        }

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
