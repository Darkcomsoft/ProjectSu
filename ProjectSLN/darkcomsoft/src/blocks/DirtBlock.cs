using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Mathematics;

namespace ProjectIND.darkcomsoft.src.blocks
{
    public class DirtBlock : Block
    {
        public DirtBlock()
        {
            v_blockName = "block_dirt";
            v_blockID = 1;

            AddUV(0, 0, 0);
            AddUV(1, 0, 0);
            AddUV(2, 0, 0);
            AddUV(3, 0, 0);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
