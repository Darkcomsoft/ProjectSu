using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.entity;
using Projectsln.darkcomsoft.src.entity.managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.world
{
    public class MainMenuWorld : World
    {
        public override void Start()
        {
            Debug.Log("MainMenu Started!");

            EntityManager.SpawnEntity<PlayerEntity>(this);
            for (int i = 0; i < 200; i++)
            {
                EntityBase entity = EntityManager.SpawnEntity<DebugEntity>(this);
                entity.transform.Position = new OpenTK.Mathematics.Vector3d(i * 5, 0, i * 5);
                entity.transform.VolumeSize = new OpenTK.Mathematics.Vector3d(1,1,1);
            }

            base.Start();
        }

        protected override void OnDispose()
        {
            Debug.Log("MainMenu Disposed!");
            base.OnDispose();
        }
    }
}
