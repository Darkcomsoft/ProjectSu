using OpenTK.Mathematics;
using ProjectSu.src.devtest;
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
            //GameObject.Instantiate("ElbriumSpace", new ModelTest());
            for (int x = 0; x < 10; x++)
            {
                for (int z = 0; z < 10; z++)
                {
                    GameObject obj = GameObject.Instantiate("ElbriumSpace", new ModelTest());
                    obj.transform.Position = new Vector3d(x * 10,0,z * 10);
                }
            }
            GameObject.Net_Instantiate("ElbriumSpace", new PlayerEntity());
            base.OnSpaceStart();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
