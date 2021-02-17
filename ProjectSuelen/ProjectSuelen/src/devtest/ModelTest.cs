using ProjectSu.src.Engine.AssetsPipeline;
using ProjectSu.src.Engine.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.devtest
{
    public class ModelTest : EEntity
    {
        protected override void OnTickDraw()
        {
            AssetManager.GetModel("oak").DrawModel(this);
            base.OnTickDraw();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
