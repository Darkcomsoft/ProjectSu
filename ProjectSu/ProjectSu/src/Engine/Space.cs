using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.Engine
{
    public class Space : ClassBase
    {
        public string spaceName;

        public List<GameObject> spaceEntityList;
        public Ambience ambience;

        public Space()
        {
            spaceEntityList = new List<GameObject>();
        }

        public virtual void OnSpaceStart()
        {
            ambience = new Ambience(spaceName);
        }

        public void Clear()
        {
            for (int i = 0; i < spaceEntityList.Count; i++)
            {
                spaceEntityList[i].Dispose();
            }

            spaceEntityList.Clear();
        }

        protected override void OnDispose()
        {
            for (int i = 0; i < spaceEntityList.Count; i++)
            {
                spaceEntityList[i].Dispose();
            }

            spaceEntityList.Clear();
            spaceEntityList = null;

            if (ambience != null)
            {
                ambience.Dispose();
                ambience = null;
            }
            base.OnDispose();
        }
    }
}
