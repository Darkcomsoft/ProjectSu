using Projectsln.darkcomsoft.src.engine.gameobject;
using Projectsln.darkcomsoft.src.entity.managers;
using Projectsln.darkcomsoft.src.network;
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
            GameObjManager.WorldClear(this);
            instance = null;
            base.OnDispose();
        }

        public void Tick()
        {
            if (Application.AppType.Equals(enums.ApplicationType.Client))
            {
                TickClient();
            }

            if (NetworkManager.IsServer)
            {
                TickServer();
            }

            OnTick();
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
        /// This is called everyFrame by Client and Server, is called anyway
        /// </summary>
        protected virtual void OnTick() { }
    }
}
