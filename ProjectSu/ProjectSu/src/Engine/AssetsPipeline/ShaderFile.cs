using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.Engine.AssetsPipeline
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
