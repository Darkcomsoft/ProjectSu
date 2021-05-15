using Projectsln.darkcomsoft.src.engine;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;
using Projectsln.darkcomsoft.src.client;
using ProjectSLN.darkcomsoft.src.engine.gameobject;
using ProjectSLN.darkcomsoft.src.worldgenerator;

namespace Projectsln.darkcomsoft.src.world
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

            /*if (Application.AppType == enums.ApplicationType.Client)
            {
                debug.Debug.Log("SatrillesWorld Is Created!", "CLIENT");
            }
            if (Application.IsServer)
            {
                debug.Debug.Log("SatrillesWorld Is Created!", "SERVER");
            }*/
            base.Start();
        }

        protected override void OnDispose()
        {
            GameObject.DestroyObject(m_terrainGenObject, true);
            m_terrainGenObject = null;

            base.OnDispose();
        }

        protected override void TickClient()
        {
            if (Input.GetKeyDown(Keys.Escape))
            {
                Game.Disconnect();
            }
            base.TickClient();
        }
    }
}
