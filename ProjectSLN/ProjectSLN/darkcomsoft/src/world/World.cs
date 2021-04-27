using Projectsln.darkcomsoft.src.entity.managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.world
{
    /// <summary>
    /// Base class of all Worlds, Worlds is "like" scenes in Unity3D
    /// </summary>
    public abstract class World : ClassBase
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
