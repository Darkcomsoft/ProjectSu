using ProjectSLN.darkcomsoft.src.client;
using ProjectSLN.darkcomsoft.src.debug;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSLN.darkcomsoft.src.entity
{
    public class PlayerEntity : LivingEntity
    {
        protected override void OnStart()
        {
            UseFrustum(false);

            base.OnStart();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
