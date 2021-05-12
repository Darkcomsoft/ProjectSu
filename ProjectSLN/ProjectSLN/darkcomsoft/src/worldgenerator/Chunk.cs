using Projectsln.darkcomsoft.src;
using Projectsln.darkcomsoft.src.engine;
using ProjectSLN.darkcomsoft.src.engine.gameobject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSLN.darkcomsoft.src.worldgenerator
{
    /// <summary>
    /// Base class for all type of voxel Chunks
    /// </summary>
    public abstract class Chunk : GameObject
    {
        protected override void OnAwake()
        {
            base.OnAwake();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
