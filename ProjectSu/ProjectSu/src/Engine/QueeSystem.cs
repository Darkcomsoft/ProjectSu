using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.Engine
{
    public class QueeSystem
    {
        private static Queue<GameObject> objectsToDestroy = new Queue<GameObject>();

        public static void Tick()
        {
            while (objectsToDestroy.Count > 0)
            {
                objectsToDestroy.Dequeue().Dispose();
            }
        }

        public static void EnqueueObjDestoy(GameObject obj)
        {
            objectsToDestroy.Enqueue(obj);
        }
    }
}
