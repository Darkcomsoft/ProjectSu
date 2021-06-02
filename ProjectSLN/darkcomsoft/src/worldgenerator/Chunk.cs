using OpenTK.Mathematics;
using ProjectIND.darkcomsoft.src;
using ProjectIND.darkcomsoft.src.engine;
using ProjectIND.darkcomsoft.src.engine.gameobject;
using ProjectIND.darkcomsoft.src.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.worldgenerator
{
    /// <summary>
    /// Base class for all type of voxel Chunks
    /// </summary>
    public class Chunk : GameObject
    {
        public const int m_size = 16;

        private Block[,,] m_voxelArray;

        public ChunkState m_chunkState { get; private set; }
        public ChunkStage m_chunkStages { get; private set; }

        public void SetUp(Vector3d position)
        {
            transform.Position = position;

            m_voxelArray = new Block[m_size, m_size, m_size];

            m_chunkState = ChunkState.notready;
            m_chunkStages = ChunkStage.empty;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }

        public void SetState(ChunkState chunkState)
        {
            m_chunkState = chunkState;
        }

        public void PopulateVoxels()
        {
            for (int x = 0; x < m_size; x++)
            {
                for (int y = 0; y < m_size; y++)
                {
                    for (int z = 0; z < m_size; z++)
                    {
                        m_voxelArray[x, y, z].m_blockID = 0;
                    }
                }
            }
        }
    }
}
