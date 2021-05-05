using Projectsln.darkcomsoft.src.debug;
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
        private List<Entity> m_entityList = new List<Entity>();

        public EntityManager()
        {
            instance = this;
        }

        protected override void OnDispose()
        {
            for (int i = 0; i < m_entityList.Count; i++)
            {
                NetworkManager.DestroyEntity(m_entityList[i], true);
            }
            m_entityList.Clear();

            m_entityList = null;
            instance = null;
            base.OnDispose();
        }

        public void Tick()
        {
            for (int i = 0; i < m_entityList.Count; i++)
            {
                Entity entityBase = m_entityList[i];
                entityBase.Tick();

                if (entityBase.isRemoved)
                {
                    entityBase.Dispose();
                    m_entityList.RemoveAt(--i);
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

            Instance.m_entityList.Add(entityBase);
            return entityBase;
        }

        /// <summary>
        /// Dont use this to Spawn Entitys(USE THIS -> NetworkManager.SpawnEntity ), thi is just to add and create a Entity instance to the list, but only the netcode call this
        /// </summary>
        /// <param name="type"></param>
        /// <param name="world"></param>
        /// <returns></returns>
        public static Entity AddEntity(Type type, World world)
        {
            Entity entityBase = Utilits.CreateInstance<Entity>(type);
            entityBase.Start(world);

            Instance.m_entityList.Add(entityBase);
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
                    Instance.m_entityList.Remove(entity);
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
            return Instance.m_entityList.Contains(entity);
        }

        public static void WorldClear<T>(T world)
        {
            for (int i = 0; i < Instance.m_entityList.Count; i++)
            {
                if (Instance.m_entityList[i].GetWorld is T)
                {
                    NetworkManager.DestroyEntity(Instance.m_entityList[i], true);
                }
            }
        }

        public static EntityManager Instance { get { return instance; } }
        public List<Entity> getEntityList { get { return m_entityList; } }
        public Entity[] getEntityArray { get { return m_entityList.ToArray(); } }
    }
}
