using ProjectSu.src.Engine.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.Engine
{
    public class TickManager : ClassBase
    {
        private static TickManager instance;

        private List<GameObject> tickList;
        private List<GameObject> tickDrawList;
        private List<GameObject> tickDrawTransList;

        private Dictionary<string, InstancedRenderGroup> solidRenderGroup;
        private Dictionary<string, InstancedRenderGroup> transRenderGroup;

        public event Action OnDraw;

        public TickManager()
        {
            instance = this;

            tickList = new List<GameObject>();
            tickDrawList = new List<GameObject>();
            tickDrawTransList = new List<GameObject>();

            solidRenderGroup = new Dictionary<string, InstancedRenderGroup>();
            transRenderGroup = new Dictionary<string, InstancedRenderGroup>();
        }

        public void Tick()
        {
            for (int i = 0; i < tickList.Count; i++)
            {
                tickList[i]?.Tick();
            }
        }

        public void TickDraw()
        {
            OnDraw?.Invoke();

            for (int i = 0; i < tickDrawList.Count; i++)
            {
                tickDrawList[i]?.TickDraw();
            }

            for (int i = 0; i < tickDrawTransList.Count; i++)
            {
                tickDrawTransList[i]?.TickDrawTrans();
            }
        }

        protected override void OnDispose()
        {
            tickList.Clear();
            tickList = null;

            solidRenderGroup.Clear();
            solidRenderGroup = null;

            transRenderGroup.Clear();
            transRenderGroup = null;


            instance = null;
            base.OnDispose();
        }

        public static void AddTickList(GameObject gameObject)
        {
            Instance.tickList.Add(gameObject);
        }

        public static void AddTickDrawList(GameObject gameObject)
        {
            Instance.tickDrawList.Add(gameObject);
        }

        public static void AddTickDrawTraList(GameObject gameObject)
        {
            Instance.tickDrawTransList.Add(gameObject);
        }

        public static void RemoveTickList(GameObject gameObject)
        {
            if (Instance != null)
            {
                if (Instance.tickList.Contains(gameObject))
                {
                    Instance.tickList.Remove(gameObject);
                }
            }
        }

        public static void RemoveTickDrawList(GameObject gameObject)
        {
            if (Instance != null)
            {
                if (Instance.tickDrawList.Contains(gameObject))
                {
                    Instance.tickDrawList.Remove(gameObject);
                }
            }
        }

        public static void RemoveTickDrawTraList(GameObject gameObject)
        {
            if (Instance != null)
            {
                if (Instance.tickDrawTransList.Contains(gameObject))
                {
                    Instance.tickDrawTransList.Remove(gameObject);
                }
            }
        }

        public static TickManager Instance { get { return instance; } }
    }
}
