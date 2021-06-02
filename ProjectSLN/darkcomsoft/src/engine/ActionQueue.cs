using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.engine
{
    /// <summary>
    /// Used to create a ActionQueue, to do somthing frame by frame, or if pass the max Cache Count do all in one tick
    /// </summary>
    public class ActionQueue : ClassBase
    {
        /// <summary>
        /// if the queue count pass this number, is claer every thing on it, in one tick, Default: 200
        /// </summary>
        public const int QueueCache = 300;
        private Queue<Action> actionList;

        public ActionQueue()
        {
            actionList = new Queue<Action>(QueueCache);
        }

        public void Tick()
        {
            if (actionList.Count > 0)
            {
                if (actionList.Count >= QueueCache)//if is to mutch waiting to be destroyed, clean-up everyone on the queue.
                {
                    while (actionList.Count > 0)
                    {
                        actionList.Dequeue().Invoke();
                    }
                }
                else
                {
                    actionList.Dequeue().Invoke();
                }
            }
        }

        protected override void OnDispose()
        {
            while (actionList.Count > 0)
            {
                actionList.Dequeue().Invoke();
            }

            actionList.Clear();
            base.OnDispose();
        }

        public void Enqueue(Action action)
        {
            actionList.Enqueue(action);
        }
    }
}
