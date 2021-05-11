using Projectsln.darkcomsoft.src;
using Projectsln.darkcomsoft.src.engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSLN.darkcomsoft.src.worldgenerator
{
    /// <summary>
    /// Base class for all type of voxel Chunks
    /// </summary>
    public abstract class Chunk : ClassBase
    {
        private Transform m_transform;

        protected override void OnDispose()
        {
            m_transform.Dispose();
            m_transform = null;

            base.OnDispose();
        }
    }
}
