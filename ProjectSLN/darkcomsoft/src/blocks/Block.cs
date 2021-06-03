using ProjectIND.darkcomsoft.src.resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.blocks
{
    public class Block : ClassBase
    {
        public byte v_blockID { get; protected set; }
        public string v_blockName { get; protected set; }

        public static Block GetBlockByID(byte id)
        {
            return ResourcesManager.GetBlock(id);
        }
    }
}
