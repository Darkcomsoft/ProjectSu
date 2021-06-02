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
using ProjectIND.darkcomsoft.src.game;

namespace ProjectIND.darkcomsoft.src.worldgenerator
{
    public class TerrainGenerator : GameObject
    {
        private int v_terrainGenDistanceX = 20;
        private int v_terrainGenDistanceY = 10;

        private ThreadLoop v_terrainLoadThreadLoop;
        private ActionQueue v_destrouChunkQueue;

        private Dictionary<Vector3d, Chunk> v_chunkList;

        protected override void OnAwake()
        {
            v_chunkList = new Dictionary<Vector3d, Chunk>();

            v_destrouChunkQueue = new ActionQueue();

            StartThreads();//start all others threads
            base.OnAwake();
        }

        protected override void OnDispose()
        {
            v_terrainLoadThreadLoop.Dispose();
            v_terrainLoadThreadLoop = null;

            v_destrouChunkQueue.Dispose();
            v_destrouChunkQueue = null;

            foreach (var item in v_chunkList)
            {
                DestroyChunk(item.Value);
            }

            v_chunkList = null;

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
            v_terrainLoadThreadLoop = new ThreadLoop("ThreadTerrainLoad", ThreadPriority.Lowest, false, 15, new ThreadCallBack(GenTick));
        }

        //Terrain loader tick
        private void GenTick()
        {
            Vector3d PlayerP = new Vector3d((int)(Math.Round(PlayerManager.v_playerPosition.X / Chunk.v_size) * Chunk.v_size), (int)(Math.Round(PlayerManager.v_playerPosition.Y / Chunk.v_size) * Chunk.v_size), (int)(Math.Round(PlayerManager.v_playerPosition.Z / Chunk.v_size) * Chunk.v_size));
            int minX = (int)PlayerP.X - v_terrainGenDistanceX;
            int maxX = (int)PlayerP.X + v_terrainGenDistanceX;

            int minY = (int)PlayerP.Y - v_terrainGenDistanceY;
            int maxY = (int)PlayerP.Y + v_terrainGenDistanceY;

            int minZ = (int)PlayerP.Z - v_terrainGenDistanceX;
            int maxZ = (int)PlayerP.Z + v_terrainGenDistanceX;

            foreach (var item in v_chunkList)
            {
                if (item.Value.transform.Position.X > maxX || item.Value.transform.Position.X < minX || item.Value.transform.Position.Y > maxY || item.Value.transform.Position.Y < minY || item.Value.transform.Position.Z > maxZ || item.Value.transform.Position.Z < minZ)
                {
                    if (v_chunkList.ContainsKey(item.Value.transform.Position))
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

        private void DestroyChunk(Chunk chunk)
        {
            GameObject.DestroyObject(chunk);

            lock (v_chunkList)
            {
                v_chunkList.Remove(chunk.transform.Position);
            }
        }
    }
}
