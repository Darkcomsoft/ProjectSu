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
        private static TickManager Instance;

        private List<GameObject> tickList;
        private Dictionary<string, RenderGroup> solidRenderGroup;
        private Dictionary<string, RenderGroup> transRenderGroup;

        public static Action OnDraw;

        public TickManager()
        {
            Instance = this;

            tickList = new List<GameObject>();
            solidRenderGroup = new Dictionary<string, RenderGroup>();
            transRenderGroup = new Dictionary<string, RenderGroup>();
        }

        public void Tick()
        {
            for (int i = 0; i < tickList.Count; i++)
            {
                tickList[i].Tick();
            }
        }

        public void TickDraw()
        {
            OnDraw?.Invoke();
            for (int i = 0; i < tickList.Count; i++)
            {
                tickList[i].TickDraw();
            }

            for (int i = 0; i < tickList.Count; i++)
            {
                tickList[i].TickDrawTrans();
            }
        }

        protected override void OnDispose()
        {
            foreach (var item in tickList)
            {
                item.Dispose();
            }

            foreach (var item in solidRenderGroup)
            {
                item.Value.Dispose();
            }

            foreach (var item in transRenderGroup)
            {
                item.Value.Dispose();
            }

            tickList.Clear();
            tickList = null;

            solidRenderGroup.Clear();
            solidRenderGroup = null;

            transRenderGroup.Clear();
            transRenderGroup = null;


            Instance = null;
            base.OnDispose();
        }

        public static void AddTickList(GameObject gameObject)
        {
            Instance.tickList.Add(gameObject);
        }

        public static void RemoveTickList(GameObject gameObject)
        {
            if (Instance.tickList.Contains(gameObject))
            {
                Instance.tickList.Remove(gameObject);
            }
        }
    }
}
