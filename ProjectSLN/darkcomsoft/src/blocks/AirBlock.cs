using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.blocks
{
    public class AirBlock : Block
    {
        public AirBlock(byte id)
        {
            v_blockName = "block_air";
            v_blockID = id;
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
