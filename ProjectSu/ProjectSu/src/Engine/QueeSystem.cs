using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.Engine
{
    public class QueeSystem
    {
        /// <summary>
        /// if the queue count pass this number, is claer every thing on it, in one tick, Default: 200
        /// </summary>
        public static int QueueCache = 200;
        private static Queue<GameObject> objectsToDestroy = new Queue<GameObject>();

        public static void Tick()
        {
            if (objectsToDestroy.Count > 0)
            {
                if (objectsToDestroy.Count >= QueueCache)//if is to mutch waiting to be destroyed, clean-up everyone on the queue.
                {
                    while (objectsToDestroy.Count > 0)
                    {
                        GameObject obj = objectsToDestroy.Dequeue();
                        obj.Dispose();
                    }
                }
                else
                {
                    GameObject obj = objectsToDestroy.Dequeue();
                    obj.Dispose();
                }
            }
        }

        public static void CleanUp()
        {
            while (objectsToDestroy.Count > 0)
            {
                GameObject obj = objectsToDestroy.Dequeue();
                obj.Dispose();
            }
        }

        public static void EnqueueObjDestoy(GameObject obj)
        {
            objectsToDestroy.Enqueue(obj);
        }
    }
}
