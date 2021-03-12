using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.misc;
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
                DestroyEntity(Instance.entityList[i].Value, true);
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
        /// Instantiate a entity set the Type <T>, and set world you are calling the function
        /// </summary>
        /// <typeparam name="T">Type of the entity you want spawn</typeparam>
        /// <param name="world">world you want to spawn a entity</param>
        /// <returns></returns>
        public static Entity SpawnEntity<T>(World world)
        {
            Entity entityBase = Utilits.CreateInstance<Entity>(typeof(T));
            entityBase.Start(world);
            Instance.entityList.Add(KeyValuePair.Create(entityBase.GetWorld, entityBase));
            return entityBase;
        }

        public static void DestroyEntity(Entity entity, bool insta = false)
        {
            if (Instance.entityList.Contains(KeyValuePair.Create(entity.GetWorld, entity)))
            {
                if (insta)
                {
                    destroyEntity(entity);
                }
                else
                {
                    entity.DestroyThis();
                }
            }
        }

        private static void destroyEntity(Entity entityBase)
        {
            Instance.entityList.Remove(KeyValuePair.Create(entityBase.GetWorld, entityBase));
            entityBase.Dispose();
        }

        public static void WorldCleared<T>(T world)
        {
            for (int i = 0; i < Instance.entityList.Count; i++)
            {
                if (Instance.entityList[i].Key is T)
                {
                    DestroyEntity(Instance.entityList[i].Value, true);
                }
            }
        }

        public static EntityManager Instance { get { return instance; } }
    }
}
