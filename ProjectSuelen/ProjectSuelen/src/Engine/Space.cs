using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.Engine
{
    public class Space : ObjectBase
    {
        public List<Entity> spaceEntityList;

        public Space()
        {
            spaceEntityList = new List<Entity>();
        }

        protected override void OnDispose()
        {
            foreach (var item in spaceEntityList)
            {
                item.Dispose();
            }

            spaceEntityList.Clear();
            spaceEntityList = null;
        }
    }
}
