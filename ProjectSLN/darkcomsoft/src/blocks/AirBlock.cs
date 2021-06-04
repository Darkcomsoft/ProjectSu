using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.blocks
{
    public class AirBlock : Block
    {
        public AirBlock()
        {
            v_blockName = "block_air";
            v_blockID = 0;
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
