using OpenTK;
using ProjectSuelen.src.Engine.AssetsPipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.Engine
{
    public static class HelpClass
    {
        public static void UseDefaultUniforms(string shader, Matrix4 world)
        {
            AssetManager.ShaderSet(shader, "world", world);
            AssetManager.ShaderSet(shader, "view", Camera.main.GetViewMatrix());
            AssetManager.ShaderSet(shader, "projection", Camera.main.GetProjectionMatrix());
        }
    }
}
