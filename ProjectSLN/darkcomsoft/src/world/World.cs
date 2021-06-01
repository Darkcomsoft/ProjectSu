using ProjectIND.darkcomsoft.src.engine.gameobject;
using ProjectIND.darkcomsoft.src.entity.managers;
using ProjectIND.darkcomsoft.src.network;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.world
{
    /// <summary>
    /// Base class of all Worlds, Worlds is "like" scenes in Unity3D
    /// </summary>
    public abstract class World : ClassBase
    {
        public static World instance { get; private set; }

        protected override void OnDispose()
        {
            ObjectManager.WorldClear(this);
            instance = null;
            base.OnDispose();
        }

        public void Tick()
        {
            OnTick();
        }

        public virtual void Start() { instance = this; }

        /// <summary>
        /// This is called everyFrame by Client and Server, is called anyway
        /// </summary>
        protected virtual void OnTick() { }
    }
}
