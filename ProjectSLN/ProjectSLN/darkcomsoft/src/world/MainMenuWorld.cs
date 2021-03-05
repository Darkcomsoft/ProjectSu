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


            for (int i = 0; i < 9000; i++)
            {
                EntityManager.SpawnEntity<PlayerEntity>(this);
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
