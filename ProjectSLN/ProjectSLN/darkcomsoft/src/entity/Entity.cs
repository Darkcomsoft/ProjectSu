﻿using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.engine.render;
using Projectsln.darkcomsoft.src.world;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.entity
{
    /// <summary>
    /// Entity is the base entity class for all entity, All entity is sync in network.
    /// </summary>
    public class Entity : ClassBase
    {
        private Transform m_transform;
        private bool removed = false;
        private bool visible = false;
        private bool m_usefrustum = true;
        private bool _IsEntityReady = false;

        protected World world;
        protected int m_regionId = 0;
        protected long _Owner;
        protected int _ViewID = 0;
        protected NetDeliveryMethod _DefaultNetDeliveryMethod = NetDeliveryMethod.Unreliable;

        private Dictionary<string, RPCALL> _methodlist = new Dictionary<string, RPCALL>();

        public Entity() { }

        public void Start(World world)
        {
            OnBeforeStart();

            m_transform = new Transform();
            this.world = world;

            OnStart();
        }

        public void Tick() { if (!removed) { doTick(); } }

        public void DestroyThis()
        {
            removed = true;
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
            visible = false;

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
                    visible = true;
                }
                else
                {
                    visible = false;
                }
            }
            else
            {
                visible = false;
            }
        }

        protected override void OnDispose()
        {
            transform.Dispose();

            m_transform = null;

            world = null;
            base.OnDispose();
        }

        public Transform transform { get { return m_transform; } }
        public World GetWorld { get { return world; } }
        public bool isRemoved { get { return removed; } }
        public bool isVisible { get { return visible; } }
        public bool isReady { get { return _IsEntityReady; } }
        public long getOwner { get { return _Owner; } }
        public int getViewId { get { return _ViewID; } }
        public int getRegionID { get { return m_regionId; } }
        public bool isMine
        {
            get
            {
                if (Network.Runing)
                {
                    if (_Owner == Network.MyPeer.UniqueIdentifier)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
