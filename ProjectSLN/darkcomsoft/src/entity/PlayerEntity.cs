using ProjectIND.darkcomsoft.src.client;
using ProjectIND.darkcomsoft.src.debug;
using ProjectIND.darkcomsoft.src.engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.entity
{
    public class PlayerEntity : LivingEntity
    {

        private Camera v_playerCamera;
        private double v_moveSpeed = 0.3f;
        private double v_sensitivity = 0.1f;

        private double Yaw { get; set; }
        private double Pitch { get; set; }

        private const double v_characterHeight = 1;
        private const double v_characterWidth = 0.5f;
        private const double v_jumpSpeed = 10f;
        private const double v_stepHeight = 5f;

        protected override void OnStart()
        {
            UseFrustum(false);

            base.OnStart();
        }

        protected override void OnDeath()
        {
            Game.KillPlayer(this);
            base.OnDeath();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
