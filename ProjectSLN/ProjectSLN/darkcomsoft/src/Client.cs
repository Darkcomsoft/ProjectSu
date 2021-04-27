using OpenTK.Mathematics;
using Projectsln.darkcomsoft.src.debug;
using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.entity;
using Projectsln.darkcomsoft.src.entity.managers;
using Projectsln.darkcomsoft.src.world;
using OpenTK.Windowing.GraphicsLibraryFramework;
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
            Debug.Log("GameStarted!", "CLIENT");

            WorldManager.SpawnWorld<SystemWorld>();
            WorldManager.SpawnWorld<MainMenuWorld>();
        }

        public override void Tick()
        {
            if (Input.GetKeyDown(Keys.P))
            {
                if (!CursorManager.isLocked)
                {
                    CursorManager.Lock();
                }
                else
                {
                    CursorManager.UnLock();
                }
            }

            if (Input.GetKeyDown(Keys.F2))
            {
                if (Debug.isDebugEnabled)
                {
                    Debug.DisableDebug();
                }
                else
                {
                    Debug.EnableDebug();
                }
            }
            base.Tick();
        }

        public override void TickDraw()
        {
            base.TickDraw();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
