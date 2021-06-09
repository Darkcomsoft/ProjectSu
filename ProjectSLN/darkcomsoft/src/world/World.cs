using OpenTK.Mathematics;
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

        #region Ambience
        public bool Enabled = true;
        public float Density = 0.014f;
        public float Distance = 3.5f;

        public Color4 FogColor = new Color4(0, 0.7490196f, 1, 1);
        public Color4 SkyColor = Color4.DeepSkyBlue;
        public Color4 SkyHorizonColor = Color4.SkyBlue;
        public Color4 AmbienceColor = Color4.White;
        #endregion

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
