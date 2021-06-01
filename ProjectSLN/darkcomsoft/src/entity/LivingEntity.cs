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
        private bool m_alive = false;

        protected override void OnAwake()
        {
            m_alive = true;
            base.OnAwake();
        }

        protected virtual void OnDeath() { }
    }
}
