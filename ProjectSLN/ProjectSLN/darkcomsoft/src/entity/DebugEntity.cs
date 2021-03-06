using Projectsln.darkcomsoft.src.engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.entity
{
    public class DebugEntity : LivingEntity
    {
        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnTick()
        {
            //Debug.Log("PlayerTick");
            base.OnTick();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}