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
        protected World world;

        public EntityBase()
        {
            Debug.Log("Entity Constructor", "ENTITY");
        }

        public void Start(World world)
        {
            this.world = world;
        }

        protected override void OnDispose()
        {
            world = null;
            base.OnDispose();
        }

        public World GetWorld { get { return world; } }
    }
}
