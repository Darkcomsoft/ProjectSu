using ProjectIND.darkcomsoft.src.client;
using ProjectIND.darkcomsoft.src.debug;
using ProjectIND.darkcomsoft.src.game;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.entity
{
    public class PlayerEntity : LivingEntity
    {
        protected override void OnStart()
        {
            UseFrustum(false);

            base.OnStart();
        }

        protected override void OnDeath()
        {
            PlayerManager.KillPlayer(this);
            base.OnDeath();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
