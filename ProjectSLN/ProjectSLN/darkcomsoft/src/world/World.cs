using Projectsln.darkcomsoft.src.entity.managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.world
{
    public class World : ClassBase
    {
       public virtual void Start()
        {

        }

        protected override void OnDispose()
        {
            EntityManager.WorldCleared(this);
            base.OnDispose();
        }
    }
}
