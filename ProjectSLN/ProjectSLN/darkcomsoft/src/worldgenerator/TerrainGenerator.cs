using OpenTK.Mathematics;
using Projectsln.darkcomsoft.src;
using Projectsln.darkcomsoft.src.debug;
using Projectsln.darkcomsoft.src.misc;
using ProjectSLN.darkcomsoft.src.engine;
using ProjectSLN.darkcomsoft.src.engine.gameobject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProjectSLN.darkcomsoft.src.worldgenerator
{
    public class TerrainGenerator : GameObject
    {
        private Dictionary<Vector3d, Chunk> m_chunkList;

        private ThreadLoop m_threadLoop;

        protected override void OnAwake()
        {
            m_chunkList = new Dictionary<Vector3d, Chunk>();
            StartThreads();//start all others threads
            base.OnAwake();
        }

        protected override void OnDispose()
        {
            m_threadLoop.Dispose();
            m_threadLoop = null;

            foreach (var item in m_chunkList)
            {
                item.Value.Dispose();
            }

            m_chunkList = null;

            base.OnDispose();
        }

        private void StartThreads()
        {
            Debug.Log("Starting Threads!");
            m_threadLoop = new ThreadLoop("ThreadTerrainLoad", ThreadPriority.Lowest, false, 15, new ThreadCallBack(GenTick));
        }

        //Terrain loader tick
        private void GenTick()
        {
            Debug.Log("Tick Terrain");
        }
    }
}
