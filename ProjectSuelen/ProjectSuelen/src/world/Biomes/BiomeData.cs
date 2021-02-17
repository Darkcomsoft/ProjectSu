using ProjectSu.src.database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.world.Biomes
{
    public struct BiomeData
    {
        public TypeBlock _typeBlock;
        public BlockVariant _blockVariant;
        public TreeType _treeType;
        public float _Height;

        public BiomeData(float Height,TypeBlock typeBlock, BlockVariant blockVariant, TreeType treeType)
        {
            _typeBlock = typeBlock;
            _blockVariant = blockVariant;
            _treeType = treeType;
            _Height = Height;
        }
    }
}
