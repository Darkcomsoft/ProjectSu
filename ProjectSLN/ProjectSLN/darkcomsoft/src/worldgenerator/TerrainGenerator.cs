using OpenTK.Mathematics;
using Projectsln.darkcomsoft.src;
using Projectsln.darkcomsoft.src.debug;
using Projectsln.darkcomsoft.src.misc;
using ProjectSLN.darkcomsoft.src.engine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProjectSLN.darkcomsoft.src.worldgenerator
{
    public class TerrainGenerator : ClassBase
    {
        private Dictionary<Vector3d, Chunk> m_chunkList;

        private ThreadLoop m_threadLoop;

        public TerrainGenerator()
        {
            m_chunkList = new Dictionary<Vector3d, Chunk>();
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
            m_threadLoop = new ThreadLoop(new DeleTeste(Tickk));
        }

        protected virtual void LoadingLoop()
        {

        }

        private void Tickk()
        {
            Debug.Log("Tick Terrain");
        }
    }
}
