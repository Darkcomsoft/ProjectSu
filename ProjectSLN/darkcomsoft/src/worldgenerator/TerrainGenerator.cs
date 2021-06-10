using OpenTK.Mathematics;
using ProjectIND.darkcomsoft.src;
using ProjectIND.darkcomsoft.src.debug;
using ProjectIND.darkcomsoft.src.misc;
using ProjectIND.darkcomsoft.src.engine;
using ProjectIND.darkcomsoft.src.engine.gameobject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ProjectIND.darkcomsoft.src.client;

namespace ProjectIND.darkcomsoft.src.worldgenerator
{
    public class TerrainGenerator : GameObject
    {
        public static TerrainGenerator instance { get; private set; }

        private int v_terrainGenDistanceX = 20;
        private int v_terrainGenDistanceY = 10;

        private ThreadLoop v_terrainLoadThreadLoop;
        private ActionQueue v_destrouChunkQueue;

        //VoxelPopulate
        private ThreadLoop v_populateVoxelThreadLoop;
        private Queue<Vector3d> v_populateVoxelQueue;

        //VoxelPopulate
        private ThreadLoop v_populateMeshThreadLoop;
        private Queue<Vector3d> v_populateMeshQueue;

        private Dictionary<Vector3d, Chunk> v_chunkList;

        protected override void OnAwake()
        {
            instance = this;
            v_chunkList = new Dictionary<Vector3d, Chunk>();

            v_destrouChunkQueue = new ActionQueue();
            v_populateVoxelQueue = new Queue<Vector3d>();
            v_populateMeshQueue = new Queue<Vector3d>();

            StartThreads();//start all others threads
            base.OnAwake();
        }

        protected override void OnDispose()
        {
            v_terrainLoadThreadLoop.Dispose();
            v_terrainLoadThreadLoop = null;

            v_populateVoxelThreadLoop.Dispose();
            v_populateVoxelThreadLoop = null;

            v_populateMeshThreadLoop.Dispose();
            v_populateMeshThreadLoop = null;

            v_destrouChunkQueue.Dispose();
            v_destrouChunkQueue = null;

            foreach (var item in v_chunkList)
            {
                DestroyChunk(item.Value);
            }

            v_populateVoxelQueue.Clear();
            v_populateVoxelQueue = null;

            v_populateMeshQueue.Clear();
            v_populateMeshQueue = null;

            v_chunkList = null;
            instance = null;
            base.OnDispose();
        }

        protected override void OnTick()
        {
            v_destrouChunkQueue.Tick();
            base.OnTick();
        }

        private void StartThreads()
        {
            Debug.Log("Starting Threads!");
            v_terrainLoadThreadLoop = new ThreadLoop("Thread_Terrain_Load", ThreadPriority.Lowest, false, 15, new ThreadCallBack(GenTick));
            v_populateVoxelThreadLoop = new ThreadLoop("Thread_Voxel_Populate", ThreadPriority.Lowest,  false, 10, new ThreadCallBack(PopulateVoxelTick));
            v_populateMeshThreadLoop = new ThreadLoop("Thread_Mesh_Populate", ThreadPriority.Lowest, false, 10, new ThreadCallBack(PopulateMeshTick));
        }

        //Terrain loader tick
        private void GenTick()
        {
            Vector3d PlayerP = new Vector3d((int)(Math.Round(Game.v_playerPosition.X / Chunk.v_size) * Chunk.v_size), (int)(Math.Round(Game.v_playerPosition.Y / Chunk.v_size) * Chunk.v_size), (int)(Math.Round(Game.v_playerPosition.Z / Chunk.v_size) * Chunk.v_size));
            int minX = (int)PlayerP.X - v_terrainGenDistanceX;
            int maxX = (int)PlayerP.X + v_terrainGenDistanceX;

            int minY = (int)PlayerP.Y - v_terrainGenDistanceY;
            int maxY = (int)PlayerP.Y + v_terrainGenDistanceY;

            int minZ = (int)PlayerP.Z - v_terrainGenDistanceX;
            int maxZ = (int)PlayerP.Z + v_terrainGenDistanceX;

            foreach (var item in v_chunkList)
            {
                if (item.Value.transform.v_Position.X > maxX || item.Value.transform.v_Position.X < minX || item.Value.transform.v_Position.Y > maxY || item.Value.transform.v_Position.Y < minY || item.Value.transform.v_Position.Z > maxZ || item.Value.transform.v_Position.Z < minZ)
                {
                    if (v_chunkList.ContainsKey(item.Value.transform.v_Position))
                    {
                        item.Value.SetState(enums.ChunkState.deleted);
                        v_destrouChunkQueue.Enqueue(() => { DestroyChunk(item.Value); });
                    }
                }
            }

            for (int x = minX; x < maxX; x += Chunk.v_size)
            {
                for (int y = minY; y < maxY; y += Chunk.v_size)
                {
                    for (int z = minZ; z < maxZ; z += Chunk.v_size)
                    {
                        Vector3d vector = new Vector3d(x, y, z);

                        lock (v_chunkList)
                        {
                            if (!v_chunkList.ContainsKey(vector))
                            {
                                Chunk chunk = (Chunk)GameObject.SpawnObject<Chunk>(v_world);
                                chunk.SetUp(vector);
                                v_chunkList.Add(vector, chunk);
                            }
                            else
                            {
                                if (v_chunkList[vector].v_chunkState == enums.ChunkState.deleted)
                                {
                                    v_chunkList[vector].SetState(enums.ChunkState.notready);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void PopulateVoxelTick()
        {
            lock (v_populateVoxelQueue)
            {
                while (v_populateVoxelQueue.Count > 0)
                {
                    Vector3d pos = v_populateVoxelQueue.Dequeue();

                    if (v_chunkList.ContainsKey(pos))
                    {
                        v_chunkList[pos].PopulateVoxels();
                    }
                }
            }
        }

        private void PopulateMeshTick()
        {
            lock (v_populateMeshQueue)
            {
                while (v_populateMeshQueue.Count > 0)
                {
                    Vector3d pos = v_populateMeshQueue.Dequeue();

                    if (v_chunkList.ContainsKey(pos))
                    {
                        v_chunkList[pos].PopulateMesh();
                    }
                }
            }
        }

        private void DestroyChunk(Chunk chunk)
        {
            GameObject.DestroyObject(chunk);

            lock (v_chunkList)
            {
                v_chunkList.Remove(chunk.transform.v_Position);
            }
        }

        public static void RequestVoxelPopulate(Chunk chunk)
        {
            instance.v_populateVoxelQueue.Enqueue(chunk.transform.v_Position);
        }

        public static void RequestMeshPopulate(Chunk chunk)
        {
            instance.v_populateMeshQueue.Enqueue(chunk.transform.v_Position);
        }

        #region GetFunctions
        public BlockVoxel GetTileAt(int x, int y,int z)
        {
            Chunk chunk = GetChunkAt(x, y,z);

            if (chunk != null)
            {
                return chunk.v_voxelArray[x - (int)chunk.transform.v_Position.X, y - (int)chunk.transform.v_Position.Y, z - (int)chunk.transform.v_Position.Z];
            }
            return new BlockVoxel();
        }

        public BlockVoxel GetTileAt(Vector3d pos)
        {
            Chunk chunk = GetChunkAt((int)pos.X, (int)pos.Y, (int)pos.Z);

            if (chunk != null)
            {
                return chunk.v_voxelArray[(int)pos.X - (int)chunk.transform.v_Position.X, (int)pos.Y - (int)chunk.transform.v_Position.Y, (int)pos.Z - (int)chunk.transform.v_Position.Z];
            }
            return new BlockVoxel();
        }

        public BlockVoxel GetTileAt(double x, double y,double z)
        {
            int mx = MathUtils.FloorToInt(x);
            int my = MathUtils.FloorToInt(y);
            int mz = MathUtils.FloorToInt(z);

            Chunk chunk = GetChunkAt(mx, my, mz);

            if (chunk != null)
            {
                return chunk.v_voxelArray[mx - (int)chunk.transform.v_Position.X, my - (int)chunk.transform.v_Position.Y, mz - (int)chunk.transform.v_Position.Z];
            }
            return new BlockVoxel();
        }

        public Chunk GetChunkAt(int xx, int yy,int zz)
        {
            Vector3d chunkpos = new Vector3d(MathUtils.FloorToInt(xx / (double)Chunk.v_size) * Chunk.v_size, MathUtils.FloorToInt(yy / (double)Chunk.v_size) * Chunk.v_size, MathUtils.FloorToInt(zz / (double)Chunk.v_size) * Chunk.v_size);

            if (v_chunkList.ContainsKey(chunkpos))
            {
                return v_chunkList[chunkpos];
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
