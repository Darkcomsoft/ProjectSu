using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.resources.resourcestype
{
    public struct ShaderFile
    {
        public string _vertshader;
        public string _fragshader;

        public ShaderFile(string vertshader, string fragshader)
        {
            _vertshader = vertshader;
            _fragshader = fragshader;
        }
    }
}
