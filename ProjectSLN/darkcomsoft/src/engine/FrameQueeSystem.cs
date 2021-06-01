using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.engine
{
    /// <summary>
    /// Queue Called Frame by Frame
    /// </summary>
    public static class FrameQueeSystem
    {
        /// <summary>
        /// if the queue count pass this number, is claer every thing on it, in one tick, Default: 200
        /// </summary>
        public const int QueueCache = 300;
        private static Queue<Action> actionList = new Queue<Action>(QueueCache);

        public static void Tick()
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

        public static void CleanUp()
        {
            while (actionList.Count > 0)
            {
                actionList.Dequeue().Invoke();
            }

            actionList.Clear();
        }

        public static void Enqueue(Action action)
        {
            actionList.Enqueue(action);
        }
    }
}
