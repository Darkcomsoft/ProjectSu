using ProjectSu.src.Engine.Entitys;
using ProjectSu.src.Engine.NetCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.Engine
{
    public class GameObject : ClassBase
    {
        private Transform Transform;
        private string spacename;

        private bool IsDetroying;

        public GameObject gameObject;

        public GameObject()
        {
            gameObject = this;

            OnCreate();
            Transform = new Transform();
        }

        public void Tick()
        {
            if (!IsDetroying)
            {
                OnTick();
            }
        }

        public void TickDraw()
        {
            if (!IsDetroying)
            {
                OnTickDraw();
            }
        }

        public void TickDrawTrans()
        {
            if (!IsDetroying)
                OnTickDrawTrans();
        }

        protected override void OnDispose()
        {
            if (!IsDetroying)
            {
                RemoveTick();
                RemoveDraw();
                RemoveDrawTrans();
            }

            OnDestroy();

            Transform.Dispose();
            gameObject = null;
            base.OnDispose();
        }

        public virtual void Start()
        {
            OnStart();
        }

        protected virtual void OnStart()
        {

        }

        /// <summary>
        /// Used for all update logics
        /// </summary>
        protected virtual void OnTick()
        {

        }

        /// <summary>
        /// used to reder solid obejcts
        /// </summary>
        protected virtual void OnTickDraw()
        {

        }

        /// <summary>
        /// Used to render transparent objects
        /// </summary>
        protected virtual void OnTickDrawTrans()
        {

        }

        protected virtual void OnDestroy()
        {
            
        }

        protected virtual void OnCreate()
        {

        }

        #region TickFunctions

        public void AddTick()
        {
            TickManager.AddTickList(this);
        }

        public void AddDraw()
        {
            TickManager.AddTickDrawList(this);
        }

        public void AddDrawTrans()
        {
            TickManager.AddTickDrawTraList(this);
        }

        //

        public void RemoveTick()
        {
            TickManager.RemoveTickList(this);
        }

        public void RemoveDraw()
        {
            TickManager.RemoveTickDrawList(this);
        }

        public void RemoveDrawTrans()
        {
            TickManager.RemoveTickDrawTraList(this);
        }
        #endregion

        private void StartDestroyProcess()
        {
            IsDetroying = true;
            RemoveTick();
            RemoveDraw();
            RemoveDrawTrans();
        }

        public static GameObject Instantiate(string SpaceName, GameObject obj)
        {
            SpaceManager.AddObjectToSpace(SpaceName, obj);
            return obj;
        }

        public static void Destroy(GameObject obj, bool imediato = false)
        {
            SpaceManager.RemoveObjectToSpace(obj.SpaceName, obj);

            if (imediato)
            {
                obj.StartDestroyProcess();
                obj.Dispose();
            }
            else
            {
                obj.StartDestroyProcess();
                QueeSystem.EnqueueObjDestoy(obj);
            }
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
