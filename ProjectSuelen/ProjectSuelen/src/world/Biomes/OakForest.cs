﻿using ProjectSuelen;
using ProjectSuelen.src.database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.world.Biomes
{
    public static class OakForest
    {
        public static BiomeData GetBiome(int x, int z, Chunk chunk)
        {
            FastNoise noiseFast = new FastNoise(GlobalData.Seed);
            FastNoise noiseFast2 = new FastNoise(GlobalData.Seed);
            System.Random rand = new Random(GlobalData.Seed - (int)chunk.GetSeed * chunk.GetHashCode() * x + z - chunk.transform.Position.GetHashCode());

            noiseFast.SetFrequency(0.009f);
            noiseFast2.SetFrequency(0.009f);

            float noise = noiseFast.GetPerlinFractal(x, z) * 100;

            if (rand.Next(0, 100) <= 3)
            {
                return new BiomeData(noise * noiseFast2.GetPerlinFractal(x, z), TypeBlock.Grass, BlockVariant.none, TreeType.Oak);
            }
            else
            {
                return new BiomeData(noise * noiseFast2.GetPerlinFractal(x, z), TypeBlock.Grass, BlockVariant.none, TreeType.none);
            }
        }
    }
}