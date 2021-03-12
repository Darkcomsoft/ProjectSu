﻿using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.entity;
using Projectsln.darkcomsoft.src.entity.managers;
using Projectsln.darkcomsoft.src.world;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src
{
    /// <summary>
    /// is the client, this is the class start the game stuff, like graphics, and other things
    /// </summary>
    public class Client : BuildTypeBase
    {

        public Client()
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