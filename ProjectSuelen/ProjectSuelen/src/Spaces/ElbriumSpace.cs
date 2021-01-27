using ProjectSuelen.src.Engine;
using ProjectSuelen.src.Entitys;
using ProjectSuelen.src.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.Spaces
{
    public class ElbriumSpace : Space
    {
        public ElbriumSpace()
        {
           
        }

        public override void OnSpaceStart()
        {
            GameManager.Instantiate("ElbriumSpace", new ElbriumWorld());
            GameManager.Net_Instantiate("ElbriumSpace", new PlayerEntity());
            base.OnSpaceStart();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
