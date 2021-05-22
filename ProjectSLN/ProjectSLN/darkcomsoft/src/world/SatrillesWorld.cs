using ProjectSLN.darkcomsoft.src.engine;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;
using ProjectSLN.darkcomsoft.src.client;
using ProjectSLN.darkcomsoft.src.engine.gameobject;
using ProjectSLN.darkcomsoft.src.worldgenerator;
using ProjectSLN.darkcomsoft.src.entity;

namespace ProjectSLN.darkcomsoft.src.world
{
    /// <summary>
    /// Satrilles World is the main world of the games history, the normal world, start world etc.
    /// </summary>
    public class SatrillesWorld : World
    {
        private TerrainGenerator m_terrainGenObject;

        public override void Start()
        {
            m_terrainGenObject = (TerrainGenerator)GameObject.SpawnObject<TerrainGenerator>(this);
            base.Start();
        }

        protected override void OnDispose()
        {
            GameObject.DestroyObject(m_terrainGenObject, true);
            m_terrainGenObject = null;

            base.OnDispose();
        }

        protected override void OnTick()
        {
            if (Input.GetKeyDown(Keys.Escape))
            {
                Game.Disconnect();
            }
            base.OnTick();
        }
    }
}
