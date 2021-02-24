using OpenTK.Mathematics;
using ProjectSu.src.Engine;
using ProjectSu.src.Entitys;
using ProjectSu.src.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.Spaces
{
    public class ElbriumSpace : Space
    {
        public ElbriumSpace()
        {
           
        }

        public override void OnSpaceStart()
        {
            GameObject.Instantiate("ElbriumSpace", new ElbriumWorld());
            GameObject.Net_Instantiate("ElbriumSpace", new PlayerEntity());
            base.OnSpaceStart();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
