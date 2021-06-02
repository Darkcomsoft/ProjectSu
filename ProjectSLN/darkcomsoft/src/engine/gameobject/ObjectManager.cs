using ProjectIND.darkcomsoft.src.debug;
using ProjectIND.darkcomsoft.src.entity;
using ProjectIND.darkcomsoft.src.misc;
using ProjectIND.darkcomsoft.src.network;
using ProjectIND.darkcomsoft.src.world;
using ProjectIND.darkcomsoft.src.engine.gameobject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.engine.gameobject
{
    public class ObjectManager : ClassBase
    {
        private static ObjectManager instance;
        private List<GameObject> v_objectList = new List<GameObject>();

        public ObjectManager()
        {
            instance = this;
        }

        protected override void OnDispose()
        {
            for (int i = 0; i < v_objectList.Count; i++)
            {
                GameObject.DestroyObject(v_objectList[i]);
            }
            v_objectList.Clear();

            v_objectList = null;
            instance = null;
            base.OnDispose();
        }

        public void Tick()
        {
            for (int i = 0; i < v_objectList.Count; i++)
            {
                GameObject entityBase = v_objectList[i];
                entityBase.Tick();

                if (entityBase.isRemoved)
                {
                    v_objectList.Remove(entityBase);
                    entityBase.Dispose();
                }
            }
        }

        /// <summary>
        ///  THIS IS ONLY FOR THE GAME-ENGINE TO CALL, Use This <see cref="GameObject.SpawnObject{T}(World)"/> instead!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="world"></param>
        /// <returns></returns>
        public static GameObject CreateGameObject<T>(World world)
        {
            GameObject gameObject = Utilits.CreateInstance<GameObject>(typeof(T));
            Instance.v_objectList.Add(gameObject);
            gameObject.Create(world);
            return gameObject;
        }

        /// <summary>
        /// THIS IS ONLY FOR THE GAME-ENGINE TO CALL, Use This <see cref="GameObject.SpawnObject{T}(World)"/> instead!
        /// </summary>
        /// <param name="type"></param>
        /// <param name="world"></param>
        /// <returns></returns>
        public static GameObject CreateGameObject(Type type, World world)
        {
            GameObject gameObject = Utilits.CreateInstance<GameObject>(type);
            Instance.v_objectList.Add(gameObject);
            gameObject.Create(world);
            return gameObject;
        }

        /// <summary>
        /// THIS IS ONLY FOR THE GAME-ENGINE TO CALL, Use This <see cref="GameObject.DestroyObject(GameObject, bool)"/> instead!
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
                    Instance.v_objectList.Remove(entity);
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
            return Instance.v_objectList.Contains(entity);
        }

        public static void WorldClear<T>(T world)
        {
            for (int i = 0; i < Instance.v_objectList.Count; i++)
            {
                if (Instance.v_objectList[i].GetWorld is T)
                {
                    if (Instance.v_objectList[i] != null)
                    {
                        GameObject.DestroyObject(Instance.v_objectList[i]);
                    }
                }
            }
        }

        public static ObjectManager Instance { get { return instance; } }
        public List<GameObject> getEntityList { get { return v_objectList; } }
        public GameObject[] getEntityArray { get { return v_objectList.ToArray(); } }
    }
}
