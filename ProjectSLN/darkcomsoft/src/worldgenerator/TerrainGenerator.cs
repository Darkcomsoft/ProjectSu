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
        private int m_terrainGenDistanceX = 20;
        private int m_terrainGenDistanceY = 10;

        private ThreadLoop m_terrainLoadThreadLoop;
        private ActionQueue m_destrouChunkQueue;

        private Dictionary<Vector3d, Chunk> m_chunkList;

        protected override void OnAwake()
        {
            m_chunkList = new Dictionary<Vector3d, Chunk>();

            m_destrouChunkQueue = new ActionQueue();

            StartThreads();//start all others threads
            base.OnAwake();
        }

        protected override void OnDispose()
        {
            m_terrainLoadThreadLoop.Dispose();
            m_terrainLoadThreadLoop = null;

            m_destrouChunkQueue.Dispose();
            m_destrouChunkQueue = null;

            foreach (var item in m_chunkList)
            {
                DestroyChunk(item.Value);
            }

            m_chunkList = null;

            base.OnDispose();
        }

        protected override void OnTick()
        {
            m_destrouChunkQueue.Tick();
            base.OnTick();
        }

        private void StartThreads()
        {
            Debug.Log("Starting Threads!");
            m_terrainLoadThreadLoop = new ThreadLoop("ThreadTerrainLoad", ThreadPriority.Lowest, false, 15, new ThreadCallBack(GenTick));
        }

        //Terrain loader tick
        private void GenTick()
        {
            Vector3d PlayerP = new Vector3d((int)(Math.Round(PlayerManager.m_playerPosition.X / Chunk.m_size) * Chunk.m_size), (int)(Math.Round(PlayerManager.m_playerPosition.Y / Chunk.m_size) * Chunk.m_size), (int)(Math.Round(PlayerManager.m_playerPosition.Z / Chunk.m_size) * Chunk.m_size));
            int minX = (int)PlayerP.X - m_terrainGenDistanceX;
            int maxX = (int)PlayerP.X + m_terrainGenDistanceX;

            int minY = (int)PlayerP.Y - m_terrainGenDistanceY;
            int maxY = (int)PlayerP.Y + m_terrainGenDistanceY;

            int minZ = (int)PlayerP.Z - m_terrainGenDistanceX;
            int maxZ = (int)PlayerP.Z + m_terrainGenDistanceX;

            foreach (var item in m_chunkList)
            {
                if (item.Value.transform.Position.X > maxX || item.Value.transform.Position.X < minX || item.Value.transform.Position.Y > maxY || item.Value.transform.Position.Y < minY || item.Value.transform.Position.Z > maxZ || item.Value.transform.Position.Z < minZ)
                {
                    if (m_chunkList.ContainsKey(item.Value.transform.Position))
                    {
                        item.Value.SetState(enums.ChunkState.deleted);
                        m_destrouChunkQueue.Enqueue(() => { DestroyChunk(item.Value); });
                    }
                }
            }

            for (int x = minX; x < maxX; x += Chunk.m_size)
            {
                for (int y = minY; y < maxY; y += Chunk.m_size)
                {
                    for (int z = minZ; z < maxZ; z += Chunk.m_size)
                    {
                        Vector3d vector = new Vector3d(x, y, z);

                        lock (m_chunkList)
                        {
                            if (!m_chunkList.ContainsKey(vector))
                            {
                                Chunk chunk = (Chunk)GameObject.SpawnObject<Chunk>(m_world);
                                chunk.SetUp(vector);
                                m_chunkList.Add(vector, chunk);
                            }
                            else
                            {
                                if (m_chunkList[vector].m_chunkState == enums.ChunkState.deleted)
                                {
                                    m_chunkList[vector].SetState(enums.ChunkState.notready);
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

            lock (m_chunkList)
            {
                m_chunkList.Remove(chunk.transform.Position);
            }
        }
    }
}
