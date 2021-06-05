using ProjectIND.darkcomsoft.src.engine;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;
using ProjectIND.darkcomsoft.src.client;
using ProjectIND.darkcomsoft.src.engine.gameobject;
using ProjectIND.darkcomsoft.src.worldgenerator;
using ProjectIND.darkcomsoft.src.entity;

namespace ProjectIND.darkcomsoft.src.world
{
    /// <summary>
    /// Satrilles World is the main world of the games history, the normal world, start world etc.
    /// </summary>
    public class SatrillesWorld : World
    {
        private TerrainGenerator v_terrainGenObject;

        public override void Start()
        {
            v_terrainGenObject = (TerrainGenerator)GameObject.SpawnObject<TerrainGenerator>(this);

            Game.SpawnPlayer(this);

            base.Start();
        }

        protected override void OnDispose()
        {
            GameObject.DestroyObject(v_terrainGenObject, true);
            v_terrainGenObject = null;

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
