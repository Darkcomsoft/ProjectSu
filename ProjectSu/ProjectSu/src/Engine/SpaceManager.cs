using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.Engine
{
    public class SpaceManager : ClassBase
    {
        private static SpaceManager Instance;
        private Dictionary<string, Space> spacesList;

        public SpaceManager()
        {
            Instance = this;
            spacesList = new Dictionary<string, Space>();
        }

        protected override void OnDispose()
        {
            foreach (var item in spacesList)
            {
                item.Value.Dispose();
            }

            spacesList.Clear();
            spacesList = null;

            Instance = null;

            base.OnDispose();
        }

        public static void AddSpace(string SpaceName, Space space)
        {
            if (!instance.spacesList.ContainsKey(SpaceName))
            {
                instance.spacesList.Add(SpaceName, space);
                space.spaceName = SpaceName;
                space.OnSpaceStart();
            }
        }

        public static void RemoveSpace(string SpaceName)
        {
            if (instance.spacesList.ContainsKey(SpaceName))
            {
                instance.spacesList[SpaceName].Dispose();
                instance.spacesList.Remove(SpaceName);
            }
        }

        public static void ClearSpace(string SpaceName)
        {
            if (instance.spacesList.ContainsKey(SpaceName))
            {
                instance.spacesList[SpaceName].Clear();
            }
        }

        public static Space GetSpace(string spacename)
        {
            if (instance.spacesList.TryGetValue(spacename, out Space space))
            {
                return space;
            }
            return null;
        }

        public static GameObject AddObjectToSpace(string space, GameObject obj)
        {
            if (instance.spacesList.ContainsKey(space))
            {
                instance.spacesList[space].spaceEntityList.Add(obj);
                obj.SpaceName = space;
                obj.Start();
                return obj;
            }
            throw new Exception("Don't exist this space : " + space);
        }

        public static void RemoveObjectToSpace(string space, GameObject obj)
        {
            if (instance.spacesList.ContainsKey(space))
            {
                instance.spacesList[space].spaceEntityList.Remove(obj);
            }
        }

        public static SpaceManager instance { get { return Instance; } private set { } }
    }
}
