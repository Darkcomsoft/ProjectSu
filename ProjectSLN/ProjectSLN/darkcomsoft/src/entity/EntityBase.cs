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
        private Transform transform;
        private bool removed = false;
        protected World world;

        public EntityBase()
        {
            Debug.Log("Entity Constructor", "ENTITY");
        }

        public void Start(World world)
        {
            OnBeforeStart();

            transform = new Transform();
            this.world = world;

            OnStart();
        }

        public void Tick()
        {
            if (!removed) { OnTick(); }
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


        protected override void OnDispose()
        {
            transform.Dispose();

            transform = null;

            world = null;
            base.OnDispose();
        }

        public World GetWorld { get { return world; } }
        public bool isRemoved { get { return removed; } }
    }
}
