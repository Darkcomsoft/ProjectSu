using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSLN.darkcomsoft.src.entity
{
    /// <summary>
    /// A Entity with life, can die etc.
    /// </summary>
    public class LivingEntity : Entity
    {
        private bool alive = false;

        protected override void OnAwake()
        {
            alive = true;
            base.OnAwake();
        }

        protected virtual void OnDeath() { }
    }
}
