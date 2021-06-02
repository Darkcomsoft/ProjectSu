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
        public const int v_size = 16;

        private BlockVoxel[,,] v_voxelArray;

        public ChunkState v_chunkState { get; private set; }
        public ChunkStage v_chunkStages { get; private set; }

        public void SetUp(Vector3d position)
        {
            transform.Position = position;

            v_voxelArray = new BlockVoxel[v_size, v_size, v_size];

            v_chunkState = ChunkState.notready;
            v_chunkStages = ChunkStage.empty;
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
            v_chunkState = chunkState;
        }

        public void PopulateVoxels()
        {
            for (int x = 0; x < v_size; x++)
            {
                for (int y = 0; y < v_size; y++)
                {
                    for (int z = 0; z < v_size; z++)
                    {
                        v_voxelArray[x, y, z].v_blockID = 0;
                    }
                }
            }
        }
    }
}
