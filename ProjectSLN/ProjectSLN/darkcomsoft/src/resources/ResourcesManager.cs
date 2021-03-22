using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.resources
{
    public class ResourcesManager : ClassBase
    {
        private Dictionary<string, Shader> m_shaderList;

        public ResourcesManager()
        {

        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
