using Projectsln.darkcomsoft.src.debug;
using Projectsln.darkcomsoft.src.misc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.world
{
    public class WorldManager : ClassBase
    {
        private static WorldManager instance;
        private List<World> worldList;

        public WorldManager()
        {
            instance = this;
            worldList = new List<World>();
        }

        protected override void OnDispose()
        {
            foreach (var item in worldList)
            {
                item.Dispose();
            }
            base.OnDispose();
        }

        public void Tick()
        {
            for (int i = 0; i < worldList.Count; i++)
            {
                worldList[i].Tick();
            }
        }

        public static World SpawnWorld<T>()
        {
            World world = Utilits.CreateInstance<World>(typeof(T));
            Instance.worldList.Add(world);
            world.Start();
            return world;
        }

        /// <summary>
        /// This destroy all worlds is corrent Spawned
        /// </summary>
        /// <param name="except">if you define a world, this world is not be destroyed</param>
        public static void DestroyAllWorlds(World except = null)
        {
            for (int i = 0; i < instance.worldList.Count; i++)
            {
                World world = instance.worldList[i];

                if (except != null)
                {
                    if (world != except)
                    {
                        world.Dispose();
                        Instance.worldList.Remove(world);
                    }
                }
                else
                {
                    world.Dispose();
                    Instance.worldList.Remove(world);
                }
            }
        }

        public static void DestroyWorld(World world)
        {
            if (Instance.worldList.Contains(world))
            {
                world.Dispose();
                Instance.worldList.Remove(world);
            }
        }

        public static World GetWorld(Type type)
        {
            foreach (var item in instance.worldList)
            {
                if (item.GetType().Equals(type))
                {
                    return item;
                }
            }

            throw new Exception("Don't found this world, is not loaded or have some error");
        }

        public static World GetWorld<T>()
        {
            foreach (var item in instance.worldList)
            {
                if (item.GetType().Equals(typeof(T)))
                {
                    return item;
                }
            }

            throw new Exception("Don't found this world, is not loaded or have some error");
        }

        public static WorldManager Instance { get { return instance; } }
    }
}
