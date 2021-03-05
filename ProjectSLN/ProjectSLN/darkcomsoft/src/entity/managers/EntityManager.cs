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
        private List<KeyValuePair<World, EntityBase>> entityList = new List<KeyValuePair<World, EntityBase>>();

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
                EntityBase entityBase = entityList[i].Value;
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
        public static EntityBase SpawnEntity<T>(World world)
        {
            EntityBase entityBase = Utilits.CreateInstance<EntityBase>(typeof(T));
            entityBase.Start(world);
            Instance.entityList.Add(KeyValuePair.Create(entityBase.GetWorld, entityBase));
            return entityBase;
        }

        public static void DestroyEntity(EntityBase entityBase, bool insta = false)
        {
            if (Instance.entityList.Contains(KeyValuePair.Create(entityBase.GetWorld, entityBase)))
            {
                if (insta)
                {
                    destroyEntity(entityBase);
                }
                else
                {
                    entityBase.DestroyThis();
                }
            }
        }

        private static void destroyEntity(EntityBase entityBase)
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
