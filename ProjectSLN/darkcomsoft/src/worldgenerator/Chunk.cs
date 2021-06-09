using OpenTK.Mathematics;
using ProjectIND.darkcomsoft.src;
using ProjectIND.darkcomsoft.src.blocks;
using ProjectIND.darkcomsoft.src.engine;
using ProjectIND.darkcomsoft.src.engine.gameobject;
using ProjectIND.darkcomsoft.src.enums;
using ProjectIND.darkcomsoft.src.render;
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

        private ChunkRender v_chunkRender;
        private ChunkMesh v_chunkMesh;

        public ChunkState v_chunkState { get; private set; }
        public ChunkStage v_chunkStages { get; private set; }

        public void SetUp(Vector3d position)
        {
            transform.v_Position = position;

            v_voxelArray = new BlockVoxel[v_size, v_size, v_size];
            v_chunkMesh = ChunkMesh.Empty;

            v_chunkState = ChunkState.notready;
            v_chunkStages = ChunkStage.empty;

            TerrainGenerator.RequestVoxelPopulate(this);
        }

        protected override void OnAwake()
        {
            base.OnAwake();
        }

        protected override void OnDispose()
        {
            v_voxelArray = null;

            v_chunkRender?.Dispose();
            v_chunkRender = null;
            base.OnDispose();
        }

        protected override void OnTick()
        {
            if (v_chunkStages == ChunkStage.do_Mesh)
            {
                TerrainGenerator.RequestMeshPopulate(this);
            }

            if (v_chunkStages == ChunkStage.do_finals)
            {
                v_chunkRender = new ChunkRender(this, v_chunkMesh);
                v_chunkStages = ChunkStage.ready;
            }
            base.OnTick();
        }

        protected override void OnDraw(DrawStage drawStage)
        {
            switch (drawStage)
            {
                case DrawStage.SolidDraw:
                    v_chunkRender?.TickDraw();
                    break;
                case DrawStage.TransparentDraw:
                    break;
            }
            base.OnDraw(drawStage);
        }

        public void SetState(ChunkState chunkState)
        {
            v_chunkState = chunkState;
        }

        public void PopulateVoxels()
        {
            v_chunkStages = ChunkStage.do_voxel;

            System.Random rand = new Random();

            for (int x = 0; x < v_size; x++)
            {
                for (int y = 0; y < v_size; y++)
                {
                    for (int z = 0; z < v_size; z++)
                    {
                        if (rand.Next(0,1) == 0)
                        {
                            v_voxelArray[x, y, z].v_blockID = 0;
                        }
                        else
                        {
                            v_voxelArray[x, y, z].v_blockID = 1;
                        }
                    }
                }
            }
            v_chunkStages = ChunkStage.do_Mesh;
        }

        public void PopulateMesh()
        {
            int vertexIndex = 0;

            List<Vector3> v_verts = new List<Vector3>();
            List<int> v_indices = new List<int>();
            List<Vector2> v_uvs = new List<Vector2>();

            for (int x = 0; x < v_size; x++)
            {
                for (int y = 0; y < v_size; y++)
                {
                    for (int z = 0; z < v_size; z++)
                    {
                        if (v_voxelArray[x, y, z].v_blockID != 0)
                        {
                            Vector3 blockpos = new Vector3(x, y, z);
                            BlockVoxel[] blockney = v_voxelArray[x, y, z].GetNeighboors();

                            for (int s = 0; s < 6; s++)
                            {
                                if (blockney[s].v_blockID != 0)
                                {
                                    lock (v_verts)
                                    {
                                        v_verts.Add(GetVert(Tris[s, 0]) + blockpos);
                                        v_verts.Add(GetVert(Tris[s, 1]) + blockpos);
                                        v_verts.Add(GetVert(Tris[s, 2]) + blockpos);
                                        v_verts.Add(GetVert(Tris[s, 3]) + blockpos);
                                    }

                                    lock (v_indices)
                                    {
                                        v_indices.Add(vertexIndex);
                                        v_indices.Add(vertexIndex + 1);
                                        v_indices.Add(vertexIndex + 2);
                                        v_indices.Add(vertexIndex + 2);
                                        v_indices.Add(vertexIndex + 1);
                                        v_indices.Add(vertexIndex + 3);
                                    }

                                    lock (v_uvs)
                                    {
                                        v_uvs.AddRange(Block.GetBlockByID(blockney[s].v_blockID).GetUV());
                                    }

                                    #region Hide
                                    /*if (currentBlock.Type == TypeBlock.Grass)
                                    {
                                        if (s == 0 || s == 1 || s == 4 || s == 5)
                                        {
                                            uvs.AddRange(Game.AssetsManager.GetTileUVs(chunk.Blocks[x, y, z]));
                                        }
                                        else if (s == 3)
                                        {
                                            uvs.AddRange(Game.SpriteManager.GetTileUVs("Dirt"));
                                        }
                                        else
                                        {
                                            uvs.AddRange(Game.AssetsManager.GetTileUVs(chunk.Blocks[x, y, z]));
                                        }
                                    }
                                    else
                                    {
                                        uvs.AddRange(Game.SpriteManager.GetTileUVs(chunk.Blocks[x, y, z]));
                                    }*/
                                    #endregion

                                    vertexIndex += 4;
                                }
                            }
                        }
                    }
                }
            }

            v_chunkMesh.v_Vertices = v_verts.ToArray();
            v_chunkMesh.v_indices = v_indices.ToArray();
            v_chunkMesh.v_uvs = v_uvs.ToArray();

            v_chunkStages = ChunkStage.do_finals;
        }

        public static Vector3 GetVert(int index)
        {
            return Verts[index];
        }

        public static readonly Vector3[] Verts = new Vector3[8]
        {
            new Vector3(0.0f,0.0f,0.0f),
            new Vector3(1.0f,0.0f,0.0f),
            new Vector3(1.0f,1.0f,0.0f),
            new Vector3(0.0f,1.0f,0.0f),
            new Vector3(0.0f,0.0f,1.0f),
            new Vector3(1.0f,0.0f,1.0f),
            new Vector3(1.0f,1.0f,1.0f),
            new Vector3(0.0f,1.0f,1.0f)
        };

        public static readonly int[,] Tris = new int[6, 4]
        {
            {0,3,1,2}, //Back Face 0
            {5,6,4,7},  //Front Face 1
            {3,7,2,6},  //Top Face 2
            {1,5,0,4},  //Bottom Face 3 
            {4,7,0,3},  //Left Face 4
            {1,2,5,6}   //Right Face 5
        };
    }
}