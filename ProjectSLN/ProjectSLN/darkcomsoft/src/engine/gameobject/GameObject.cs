using Lidgren.Network;
using Projectsln.darkcomsoft.src;
using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.engine.render;
using Projectsln.darkcomsoft.src.enums;
using Projectsln.darkcomsoft.src.network;
using Projectsln.darkcomsoft.src.world;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSLN.darkcomsoft.src.engine.gameobject
{
    /// <summary>
    /// Class Base for all Game "Object", is like a entity but used for all type of object, static, dynamic, etc.
    /// </summary>
    public class GameObject : ClassBase
    {
        protected Transform m_transform { get; private set; }
        protected bool m_removed { get; private set; }
        protected bool m_visible { get; private set; }
        protected bool m_usefrustum { get; private set; }
        protected bool m_IsEntityReady;//if the entity is ready for do send RPC, if the entity is spaned on the network

        protected World m_world { get; private set; }//world of this entity is in

        public void Start(World world)
        {
            m_removed = false;
            m_visible = false;
            m_usefrustum = true;
            m_IsEntityReady = false;

            m_transform = new Transform();
            this.m_world = world;

            OnAwake();
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

            //<start> call tick function anyway if is visible or not, let this to the entity to decide to do what when is visible or not
            OnTick();

            if (Application.AppType.Equals(ApplicationType.Client))
            {
                TickClient();
            }

            if (NetworkManager.IsServer)
            {
                TickServer();
            }
            //<end>
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
            m_IsEntityReady = false;

            transform.Dispose();

            m_transform = null;
            m_world = null;
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
        /// is called very frame if the entity is owned by the player
        /// </summary>
        protected virtual void TickMyEntity() { }
        /// <summary>
        /// This is called everyFrame by the Client
        /// </summary>
        protected virtual void TickClient() { }
        /// <summary>
        /// This is only called everyFrame by the server
        /// </summary>
        protected virtual void TickServer() { }
        /// <summary>
        /// This is called everyFrame by Client and Server, is called anyway
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
    }
}
