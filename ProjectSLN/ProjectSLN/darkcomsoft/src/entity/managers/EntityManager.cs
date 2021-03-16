using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.misc;
using Projectsln.darkcomsoft.src.network;
using Projectsln.darkcomsoft.src.world;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.entity.managers
{
    public class EntityManager : ClassBase
    {
        private static EntityManager instance;
        private List<KeyValuePair<World, Entity>> entityList = new List<KeyValuePair<World, Entity>>();

        public EntityManager()
        {
            instance = this;
        }

        protected override void OnDispose()
        {
            for (int i = 0; i < Instance.entityList.Count; i++)
            {
                NetworkManager.DestroyEntity(Instance.entityList[i].Value, true);
            }

            entityList = null;
            instance = null;
            base.OnDispose();
        }

        public void Tick()
        {
            for (int i = 0; i < entityList.Count; i++)
            {
                Entity entityBase = entityList[i].Value;
                entityBase.Tick();

                if (entityBase.isRemoved)
                {
                    entityBase.Dispose();
                    entityList.RemoveAt(--i);
                }
            }
        }

        /// <summary>
        /// Dont use this to Spawn Entitys(USE THIS -> NetworkManager.SpawnEntity ), thi is just to add and create a Entity instance to the list, but only the netcode call this
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="world"></param>
        /// <returns></returns>
        public static Entity AddEntity<T>(World world)
        {
            Entity entityBase = Utilits.CreateInstance<Entity>(typeof(T));
            entityBase.Start(world);

            Instance.entityList.Add(KeyValuePair.Create(entityBase.GetWorld, entityBase));
            return entityBase;
        }

        /// <summary>
        /// Dont use this to Destroy Entitys, "USE THIS -> NetworkManager.DestroyEntity ", thi is just to remove a Entity from the list, and dispose the entity, but only the netcode call this
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="world"></param>
        /// <returns></returns>
        public static void RemoveEntity(Entity entity, bool insta = false)
        {
            if (ContainsEntity(entity))
            {
                if (insta)
                {
                    Instance.entityList.Remove(KeyValuePair.Create(entity.GetWorld, entity));
                    entity.Dispose();
                }
                else
                {
                    entity.DestroyThis();
                }
            }
        }

        public static bool ContainsEntity(Entity entity)
        {
            return Instance.entityList.Contains(KeyValuePair.Create(entity.GetWorld, entity));
        }

        public static void WorldCleared<T>(T world)
        {
            for (int i = 0; i < Instance.entityList.Count; i++)
            {
                if (Instance.entityList[i].Key is T)
                {
                    NetworkManager.DestroyEntity(Instance.entityList[i].Value, true);
                }
            }
        }

        public static EntityManager Instance { get { return instance; } }
        public List<KeyValuePair<World, Entity>> getEntityList { get { return entityList; } }
    }
}
