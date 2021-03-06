﻿using Projectsln.darkcomsoft.src.engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.entity
{
    public class PlayerEntity : LivingEntity
    {
        private Camera camera;

        protected override void OnStart()
        {
            camera = new Camera(this, 1);

            base.OnStart();
        }

        protected override void OnTick()
        {
            base.OnTick();
        }

        protected override void OnDispose()
        {
            camera.Dispose();
            camera = null;

            base.OnDispose();
        }
    }
}
