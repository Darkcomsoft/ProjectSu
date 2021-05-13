using OpenTK.Mathematics;
using Projectsln.darkcomsoft.src;
using Projectsln.darkcomsoft.src.misc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProjectSLN.darkcomsoft.src.worldgenerator
{
    public class TerrainGenerator : ClassBase
    {
        private Dictionary<Vector3d, Chunk> m_chunkList;

        private Thread m_chunkSpawnThread;
        private Thread m_populateVoxelThread;
        private Thread m_meshDataGenerationThread;

        public TerrainGenerator()
        {
            m_chunkList = new Dictionary<Vector3d, Chunk>();
        }

        protected override void OnDispose()
        {
            foreach (var item in m_chunkList)
            {
                item.Value.Dispose();
            }

            m_chunkList = null;

            base.OnDispose();
        }

        private void StartThreads()
        {
            m_chunkSpawnThread = new Thread(new ThreadStart(LoadingLoop));
            m_chunkSpawnThread.Name = "Chunk-Spawn-Thread";
            m_chunkSpawnThread.Start();
        }

        protected virtual void LoadingLoop()
        {

        }
    }
}
