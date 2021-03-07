using Projectsln.darkcomsoft.src.engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.entity
{
    public class PlayerEntity : LivingEntity
    {
        protected override void OnStart()
        {
            UseFrustum(false);

            base.OnStart();
        }

        protected override void OnTick()
        {
            base.OnTick();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
