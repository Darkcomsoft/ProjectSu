using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.entity.managers;
using Projectsln.darkcomsoft.src.world;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src
{
    public class Game : ClassBase
    {

        public Game()
        {
            Debug.Log("GameStarted!");

            WorldManager.SpawnWorld<SystemWorld>();
            WorldManager.SpawnWorld<MainMenuWorld>();
        }

        public void Tick(double time)
        {
            
        }

        public void TickDraw(double time)
        {

        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
