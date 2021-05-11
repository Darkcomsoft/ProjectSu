using Projectsln.darkcomsoft.src.debug;
using Projectsln.darkcomsoft.src.entity;
using Projectsln.darkcomsoft.src.misc;
using Projectsln.darkcomsoft.src.network;
using Projectsln.darkcomsoft.src.world;
using ProjectSLN.darkcomsoft.src.engine.gameobject;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.engine.gameobject
{
    public class GameObjManager : ClassBase
    {
        private static GameObjManager instance;
        private List<GameObject> m_objectList = new List<GameObject>();

        public GameObjManager()
        {
            instance = this;
        }

        protected override void OnDispose()
        {
            for (int i = 0; i < m_objectList.Count; i++)
            {
                GameObject.DestroyObject(m_objectList[i]);
            }
            m_objectList.Clear();

            m_objectList = null;
            instance = null;
            base.OnDispose();
        }

        public void Tick()
        {
            for (int i = 0; i < m_objectList.Count; i++)
            {
                GameObject entityBase = m_objectList[i];
                entityBase.Tick();

                if (entityBase.isRemoved)
                {
                    entityBase.Dispose();
                    m_objectList.RemoveAt(--i);
                }
            }
        }

        /// <summary>
        /// Dont use this to Spawn Entitys(USE THIS -> NetworkManager.SpawnEntity ), thi is just to add and create a Entity instance to the list, but only the netcode call this
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="world"></param>
        /// <returns></returns>
        public static GameObject CreateGameObject<T>(World world)
        {
            GameObject entityBase = Utilits.CreateInstance<GameObject>(typeof(T));
            entityBase.Start(world);

            Instance.m_objectList.Add(entityBase);
            return entityBase;
        }

        /// <summary>
        /// Dont use this to Spawn Entitys(USE THIS -> NetworkManager.SpawnEntity ), thi is just to add and create a Entity instance to the list, but only the netcode call this
        /// </summary>
        /// <param name="type"></param>
        /// <param name="world"></param>
        /// <returns></returns>
        public static GameObject CreateGameObject(Type type, World world)
        {
            GameObject entityBase = Utilits.CreateInstance<GameObject>(type);
            entityBase.Start(world);

            Instance.m_objectList.Add(entityBase);
            return entityBase;
        }

        /// <summary>
        /// Dont use this to Destroy Entitys, "USE THIS -> NetworkManager.DestroyEntity ", thi is just to remove a Entity from the list, and dispose the entity, but only the netcode call this
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="world"></param>
        /// <returns></returns>
        public static void RemoveGameObject(GameObject entity, bool insta = false)
        {
            if (ContainsEntity(entity))
            {
                if (insta)
                {
                    Instance.m_objectList.Remove(entity);
                    entity.Dispose();
                }
                else
                {
                    entity.DestroyThis();
                }
            }
        }

        public static bool ContainsEntity(GameObject entity)
        {
            return Instance.m_objectList.Contains(entity);
        }

        public static void WorldClear<T>(T world)
        {
            for (int i = 0; i < Instance.m_objectList.Count; i++)
            {
                if (Instance.m_objectList[i].GetWorld is T)
                {
                    GameObject.DestroyObject(Instance.m_objectList[i]);
                }
            }
        }

        public static GameObjManager Instance { get { return instance; } }
        public List<GameObject> getEntityList { get { return m_objectList; } }
        public GameObject[] getEntityArray { get { return m_objectList.ToArray(); } }
    }
}
