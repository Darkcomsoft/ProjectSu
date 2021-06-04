using OpenTK.Mathematics;
using ProjectIND.darkcomsoft.src.resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.blocks
{
    /// <summary>
    /// Base class for all Blocks, this is only for database of the blocks
    /// </summary>
    public class Block : ClassBase
    {
        public byte v_blockID { get; protected set; }
        public string v_blockName { get; protected set; }

        public bool v_transparent { get; protected set; }

        public bool v_Lit { get; protected set; }
        public int v_LitAmount { get; protected set; }

        private Vector2[] v_uvArray = new Vector2[4];

        protected override void OnDispose()
        {
            v_uvArray = null;
            base.OnDispose();
        }

        public static Block GetBlockByID(byte id)
        {
            return ResourcesManager.GetBlock(id);
        }

        public Vector2[] GetUV()
        {
            return v_uvArray;
        }

        public void AddUV(int index,float x, float y)
        {
            v_uvArray[index] = new Vector2(x,y);
        }
    }
}
