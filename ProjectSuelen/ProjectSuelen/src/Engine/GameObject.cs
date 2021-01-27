using ProjectSuelen.src.Engine.Entitys;
using ProjectSuelen.src.Engine.NetCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.Engine
{
    public class GameObject : ClassBase
    {
        private Transform Transform;
        private string spacename;

        public GameObject gameObject;

        public GameObject()
        {
            gameObject = this;

            OnCreate();
            Transform = new Transform();
        }

        public void Tick()
        {
            OnTick();
        }

        protected override void OnDispose()
        {
            TickManager.OnTick -= Tick;
            OnDestroy();

            Transform.Dispose();
            gameObject = null;
            base.OnDispose();
        }

        public virtual void Start()
        {
            TickManager.OnTick += Tick;
        }

        protected virtual void OnTick()
        {

        }

        protected virtual void OnDestroy()
        {
            
        }

        protected virtual void OnCreate()
        {

        }

        public static GameObject Instantiate(string SpaceName, GameObject obj)
        {
            SpaceManager.AddObjectToSpace(SpaceName, obj);
            return obj;
        }

        public static void Destroy(GameObject obj)
        {
            SpaceManager.RemoveObjectToSpace(obj.SpaceName, obj);
            obj.Dispose();
            //QueeSystem.EnqueueObjDestoy(obj);
        }

        public static NEntity Net_Instantiate(string SpaceName, NEntity obj)
        {
            Network.Instantiate(SpaceName, obj);
            return obj;
        }

        public static void Net_Destroy(NEntity obj)
        {
            Network.Destroy(obj);
        }

        public Transform transform { get { return Transform; } }
        public string SpaceName { get { return spacename; }  set { spacename = value; } }
    }
}
