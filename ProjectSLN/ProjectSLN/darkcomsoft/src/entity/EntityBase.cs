﻿using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.world;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.entity
{
    /// <summary>
    /// Entity Base for all entity static and dynamic
    /// </summary>
    public class EntityBase : ClassBase
    {
        private Transform m_transform;
        private bool removed = false;
        private bool visible = false;
        private bool m_tickOlyVisible = true;
        protected World world;

        public EntityBase() { }

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
        /// If is true, is gone tick only if is visible by the camera, if is false, dont need to tick if is visible by the camera
        /// </summary>
        /// <param name="value"></param>
        public void TickOnlyVisible(bool value)
        {
            m_tickOlyVisible = value;
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
            visible = false;//set this every frame to false, just to make sure the entity can't tick, if want the entity tick any way, use TickOnlyVisible(bool value), maybe this is a stupid implementation but (: fuck-it
            DoCheckIfVisible();

            if (m_tickOlyVisible)
            {
                if (visible)
                {
                    OnTick();
                }
            }
            else
            {
                OnTick();
            }
        }

        private void DoCheckIfVisible()
        {
            if (m_tickOlyVisible)
            {
                if (Camera.main == null) { return; }

                Application.frustum.CalculateFrustum(Camera.main.GetProjectionMatrix(), transform.GetTransformWorld);

                if (Application.frustum.VolumeVsFrustum(transform.Position, transform.VolumeSize))
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
    }
}