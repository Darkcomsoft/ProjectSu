using System;
using System.Collections.Generic;
using System.Text;
using BEPUphysics;
using BEPUutilities.Threading;

namespace ProjectIND.darkcomsoft.src.engine.physics
{
    public class Physics : ClassBase
    {
        public static Physics instance { get; private set; }

        public const float v_gravityX = 0;
        public const float v_gravityY = -9.81f;
        public const float v_gravityZ = 0;

        private Space v_space;
        private ParallelLooper v_parallelLooper;

        public Physics()
        {
            instance = this;

            v_parallelLooper = new ParallelLooper();

            if (Environment.ProcessorCount > 1)
            {
                for (int i = 0; i < Environment.ProcessorCount; i++)
                {
                    v_parallelLooper.AddThread();
                }
            }

            v_space = new Space(v_parallelLooper);

            v_space.ForceUpdater.Gravity = new BEPUutilities.Vector3(v_gravityX, v_gravityY, v_gravityZ);
        }

        public void Tick(float time)
        {
            v_space?.Update(time);
        }

        protected override void OnDispose()
        {
            v_parallelLooper.Dispose();
            v_parallelLooper = null;
            v_space = null;
            instance = null;
            base.OnDispose();
        }

        public static void Add(ISpaceObject obj)
        {
            instance.v_space.Add(obj);
        }

        public static void Remove(ISpaceObject obj)
        {
            instance.v_space.Remove(obj);
        }
    }
}
