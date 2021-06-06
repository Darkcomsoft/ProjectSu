using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.entity
{
    /// <summary>
    /// A Entity with life, can die etc.
    /// </summary>
    public class LivingEntity : Entity
    {
        public bool v_isAlive { get; private set; }

        protected override void OnAwake()
        {
            v_isAlive = true;
            base.OnAwake();
        }

        protected virtual void OnDeath() { }
    }
}
