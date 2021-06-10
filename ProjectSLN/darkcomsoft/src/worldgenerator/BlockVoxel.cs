using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.worldgenerator
{
    public struct BlockVoxel
    {
        public int x, y, z;

        public byte v_blockID;

        public BlockVoxel[] GetNeighboors()
        {
            BlockVoxel[] neighbors = new BlockVoxel[6];

            neighbors[0] = TerrainGenerator.instance.GetTileAt(x + 1, y, z);//Direita
            neighbors[1] = TerrainGenerator.instance.GetTileAt(x - 1, y, z);//Esquerda
            neighbors[2] = TerrainGenerator.instance.GetTileAt(x, y + 1, z);//Cima
            neighbors[3] = TerrainGenerator.instance.GetTileAt(x, y - 1, z);//Bixo
            neighbors[4] = TerrainGenerator.instance.GetTileAt(x, y, z + 1);//Frente
            neighbors[5] = TerrainGenerator.instance.GetTileAt(x, y, z - 1);//Atras


            return neighbors;
        }
    }
}