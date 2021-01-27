using ProjectSuelen.src.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.world
{
    public class WorldManager : ClassBase
    {
        private static WorldManager Instance;
        private ElbriumWorld elbriumWorld;

        public WorldManager()
        {
            Instance = this;
        }

        protected override void OnDispose()
        {
            elbriumWorld = null;
            Instance = null;
            base.OnDispose();
        }

        public static void AddWorld(ElbriumWorld worldBase)
        {
            if (WorldManager.instance != null)
            {
                if (instance.elbriumWorld == null)
                {
                    instance.elbriumWorld = worldBase;
                }
            }
        }

        public static void RemoveWorld(ElbriumWorld worldBase)
        {
            if (instance.elbriumWorld != null)
            {
                if (instance.elbriumWorld == worldBase)
                {
                    instance.elbriumWorld = null;
                }
            }
        }

        public static ElbriumWorld GetWorld(string space)
        {
            if (instance != null)
            {
                if (instance.elbriumWorld != null)
                {
                    if (space == instance.elbriumWorld.SpaceName)
                    {
                        return instance.elbriumWorld;
                    }
                }
            }
            return null;
        }

        public static WorldManager instance { get { return Instance; } }
    }
}
