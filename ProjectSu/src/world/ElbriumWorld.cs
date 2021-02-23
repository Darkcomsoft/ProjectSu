using OpenTK;
using OpenTK.Mathematics;
using ProjectSu.src.database;
using ProjectSu.src.Engine;
using ProjectSu.src.Engine.Entitys;
using ProjectSu.src.utillity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectSu.src.world
{
    public class ElbriumWorld : WorldBase
    {
        public static int ChunkSize = 10;
        public int renderDistanceXZ = 100;
        public int renderDistanceY = 30;
        public bool WorldRuning { get; private set; }
        private bool CanDestroyWorld = false;
        public Vector3d PlayerPos;

        private object LockChunkMap;
        private object LockToUpdate;

        private Dictionary<Vector3d, Chunk> chunkMap = new Dictionary<Vector3d, Chunk>();
        private Queue<Vector3d> ToRemove = new Queue<Vector3d>();
        private Queue<Vector3d> ToUpdate = new Queue<Vector3d>();

        private Thread WorldGeneratorThread;

        public static FastNoise globalNoise;
        public static FastNoise globalNoise2;
        public static FastNoise heatNoise;
        public static FastNoise moistureNoise;
        public static FastNoise roadsNoise;

#if Client
#elif Server
#endif

        protected override void OnStart()
        {
            AddTick();

            GlobalData.Seed = System.DateTime.Now.GetHashCode() * System.DateTime.UtcNow.GetHashCode();

            LockChunkMap = new object();
            LockToUpdate = new object();

            globalNoise = new FastNoise(GlobalData.Seed);
            globalNoise.SetFrequency(0.0005f);

            globalNoise.SetFractalLacunarity(1.0f);
            globalNoise.SetFractalGain(4.0f);

            globalNoise.SetGradientPerturbAmp(9.0f);
            globalNoise.SetFrequencygrad(0.05f);

            globalNoise2 = new FastNoise(GlobalData.Seed * 15);
            globalNoise2.SetFrequency(0.005f);


            heatNoise = new FastNoise(GlobalData.Seed);
            heatNoise.SetFrequency(0.05f);
            heatNoise.SetGradientPerturbAmp(30f);
            heatNoise.SetCellularNoiseLookup(new FastNoise(GlobalData.Seed));
            heatNoise.SetCellularDistanceFunction(FastNoise.CellularDistanceFunction.Natural);
            heatNoise.SetCellularReturnType(FastNoise.CellularReturnType.NoiseLookup);


            moistureNoise = new FastNoise(GlobalData.Seed * GlobalData.Seed.GetHashCode());
            moistureNoise.SetFrequency(0.05f);
            moistureNoise.SetGradientPerturbAmp(30f);
            moistureNoise.SetCellularNoiseLookup(new FastNoise(GlobalData.Seed * GlobalData.Seed.GetHashCode()));
            moistureNoise.SetCellularDistanceFunction(FastNoise.CellularDistanceFunction.Natural);
            moistureNoise.SetCellularReturnType(FastNoise.CellularReturnType.NoiseLookup);

            roadsNoise = new FastNoise(GlobalData.Seed);

            roadsNoise.SetFrequency(0.005f);
            roadsNoise.SetGradientPerturbAmp(30f);
            roadsNoise.SetCellularDistanceFunction(FastNoise.CellularDistanceFunction.Natural);
            roadsNoise.SetCellularReturnType(FastNoise.CellularReturnType.Distance2Div);

            //LoadTheWorld if has a save
            /*if (SaveManager.LoadWorld())//Have a Save
            {

            }
            else//Dont have a save
            {

            }*/
            WorldManager.AddWorld(this);
#if Client
            WorldRuning = true;
            WorldGeneratorThread = new Thread(new ThreadStart(WorldLooping));
            WorldGeneratorThread.Name = "WorldGeneratorLoop";
            WorldGeneratorThread.IsBackground = true;
            WorldGeneratorThread.Start();
#elif Server

#endif
            base.OnStart();
        }


        protected override void OnTick()
        {
#if Client
#elif Server
#endif
            while (ToRemove.Count > 0)
            {
                lock (LockChunkMap)
                {
                    Vector3d vec = ToRemove.Dequeue();
                    if (chunkMap.ContainsKey(vec))
                    {
                        GameObject.Destroy(chunkMap[vec]);
                        chunkMap.Remove(vec);
                    }
                }
            }
            base.OnTick();
        }

        private void WorldLooping()//this is a other thread looping
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            while (WorldRuning)
            {
                CheckViewDistance();
                Thread.Sleep(100);
                /*try
                {
                    CheckViewDistance();
                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error On WorldLoop: (" + ex.Message + ") --- (StackTrace: " + ex.StackTrace + ")");
                }*/
            }

            AppDomain.CurrentDomain.UnhandledException -= new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Debug.Log("WorldThread is Finished!");

            CanDestroyWorld = true;
        }

        private static void CurrentDomain_UnhandledException(Object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;
                Debug.LogError("Unhadled domain exception:\n\n" + ex.Message);
            }
            catch (Exception exc)
            {
                try
                {
                    Debug.LogError("Fatal exception happend inside UnhadledExceptionHandler: \n\n" + exc.Message);
                }
                finally
                {
                    Environment.Exit(1);
                }
            }

            // It should terminate our main thread so Application.Exit() is unnecessary here
        }

        public void CheckViewDistance()
        {
            Vector3d PlayerP = new Vector3d((int)(Mathf.Round(PlayerPos.X / ChunkSize) * ChunkSize), (int)(Mathf.Round(PlayerPos.Y / ChunkSize) * ChunkSize), (int)(Mathf.Round(PlayerPos.Z / ChunkSize) * ChunkSize));
            int minX = (int)PlayerP.X - renderDistanceXZ;
            int maxX = (int)PlayerP.X + renderDistanceXZ;

            /*int minY = (int)PlayerP.Y - renderDistanceY;
            int maxY = (int)PlayerP.Y + renderDistanceY;*/

            int minZ = (int)PlayerP.Z - renderDistanceXZ;
            int maxZ = (int)PlayerP.Z + renderDistanceXZ;

            lock (LockToUpdate)
            {
                while (ToUpdate.Count > 0)
                {
                    lock (LockChunkMap)
                    {
                        Vector3d chunk = ToUpdate.Dequeue();
                        if (chunkMap.ContainsKey(chunk))
                        {
                            chunkMap[chunk].UpdateMesh();
                        }
                    }
                }
            }

            lock (LockChunkMap)
            {
                foreach (var item in chunkMap)
                {
                    if (item.Value.transform.Position.X > maxX || item.Value.transform.Position.X < minX || item.Value.transform.Position.Z > maxZ || item.Value.transform.Position.Z < minZ)
                    {
                        if (chunkMap.ContainsKey(item.Value.transform.Position))
                        {
                            ToRemove.Enqueue(item.Value.transform.Position);
                        }
                    }
                }
            }

            for (int x = minX; x < maxX; x += ChunkSize)
            {
                for (int z = minZ; z < maxZ; z += ChunkSize)
                {
                    Vector3d vector = new Vector3d(x, 0, z);
                    lock (LockChunkMap)
                    {
                        if (!chunkMap.ContainsKey(vector))
                        {
                            Chunk chunk = (Chunk)GameObject.Instantiate(SpaceName, new Chunk(vector));
                            chunkMap.Add(vector, chunk);
                        }
                        else
                        {
                            /*switch (chunkMap[vector].GetStatus)
                            {
                                case ChunkState.noload:
                                    break;
                                case ChunkState.nogameLogic:
                                    break;
                                case ChunkState.noEntity:
                                    break;
                                case ChunkState.AllGameLogic:
                                    break;
                                default:
                                    break;
                            }*/
                        }
                    }
                }
            }
        }

        protected override void OnDestroy()
        {
            WorldRuning = false;

            foreach (var item in chunkMap.Values)
            {
                GameObject.Destroy(chunkMap[item.transform.Position]);
            }

            chunkMap.Clear();
            ToRemove.Clear();
            WorldManager.RemoveWorld(this);
            base.OnDestroy();
        }

        public void UpDateChunk(Vector3d chunk)
        {
            lock (LockToUpdate)
            {
                ToUpdate.Enqueue(chunk);
            }
        }

        public Vector2d GetChunkCoordFromVector3(Vector3d pos)
        {
            int x = (int)Mathf.FloorToInt(pos.X / ChunkSize);
            int z = (int)Mathf.FloorToInt(pos.Z / ChunkSize);
            return new Vector2d(x, z);
        }

        public Block GetTileAt(int x, int z)
        {
            Chunk chunk = GetChunkAt(x, z);

            if (chunk != null)
            {
                return chunk.GetBlocksMap[x - (int)chunk.transform.Position.X, z - (int)chunk.transform.Position.Z];
            }
            return new Block();
        }

        public Block GetTileAt(Vector3d pos)
        {
            Chunk chunk = GetChunkAt((int)pos.X, (int)pos.Z);

            if (chunk != null)
            {
                return chunk.GetBlocksMap[(int)pos.X - (int)chunk.transform.Position.X, (int)pos.Z - (int)chunk.transform.Position.Z];
            }
            return new Block();
        }

        public Block GetTileAt(double x, double z)
        {
            int mx = Mathf.FloorToInt(x);
            int mz = Mathf.FloorToInt(z);

            Chunk chunk = GetChunkAt(mx, mz);

            if (chunk != null)
            {
                return chunk.GetBlocksMap[mx - (int)chunk.transform.Position.X, mz - (int)chunk.transform.Position.Z];
            }
            return new Block();
        }

        public Chunk GetChunkAt(int xx, int zz)
        {
            lock (LockChunkMap)
            {
                Vector3d chunkpos = new Vector3d(Mathf.FloorToInt(xx / (double)ChunkSize) * ChunkSize, 0, Mathf.FloorToInt(zz / (double)ChunkSize) * ChunkSize);

                if (chunkMap.ContainsKey(chunkpos))
                {
                    return chunkMap[chunkpos];
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
