using OpenTK.Mathematics;
using Projectsln.darkcomsoft.src;
using Projectsln.darkcomsoft.src.misc;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSLN.darkcomsoft.src.worldgenerator
{
    public class TerrainGenerator : ClassBase
    {
        private Dictionary<Vector3d, Chunk> m_chunkList;

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

        /*public Chunk SpawnChunk<T>()
        {
            Chunk entityBase = Utilits.CreateInstance<Chunk>(typeof(T));
        }*/

        public void DestroyChunk(Chunk chunk)
        {

        }
    }
}
