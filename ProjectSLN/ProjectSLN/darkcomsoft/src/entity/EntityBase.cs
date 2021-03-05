using Projectsln.darkcomsoft.src.engine;
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
        protected World world;

        public EntityBase()
        {

        }

        public void Start(World world)
        {
            OnBeforeStart();

            m_transform = new Transform();
            this.world = world;

            OnStart();
        }

        public void Tick()
        {
            if (!removed) { doTick(); }
        }

        public void DestroyThis()
        {
            removed = true;
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
            //Application.frustum.CalculateFrustum();

            OnTick();
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