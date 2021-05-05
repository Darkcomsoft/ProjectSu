using Projectsln.darkcomsoft.src.entity.managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.world
{
    /// <summary>
    /// Base class of all Worlds, Worlds is "like" scenes in Unity3D
    /// </summary>
    public abstract class World : ClassBase
    {
        public static World instance { get; private set; }

        protected override void OnDispose()
        {
            EntityManager.WorldClear(this);
            instance = null;
            base.OnDispose();
        }

        public void Tick()
        {
            if (!Application.IsServer)
            {
                TickClient();
            }
            else if (Application.IsServer)
            {
                TickServer();
            }
            else
            {
                if (Application.AppType == enums.ApplicationType.Client)
                {
                    TickClient();
                }
                else if (Application.AppType == enums.ApplicationType.Server)
                {
                    TickServer();
                }
            }

            TickServerClient();
        }

        public virtual void Start() { instance = this; }

        /// <summary>
        /// This is called everyFrame by the Client
        /// </summary>
        protected virtual void TickClient() { }
        /// <summary>
        /// This is only called everyFrame by the server
        /// </summary>
        protected virtual void TickServer() { }
        /// <summary>
        /// This is called everyFrame by Client and Server
        /// </summary>
        protected virtual void TickServerClient() { }
    }
}
