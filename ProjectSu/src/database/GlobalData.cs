using OpenTK.Graphics;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.database
{
    /// <summary>
    /// this is for setting global data like the SEED of the world generator, and other things like version of the game 
    /// </summary>
    public static class GlobalData
    {
        public static int Seed;

        #region BiomesValues
        public static BiomeType[,] BiomeTable = new BiomeType[6, 6] {   
        //COLDEST        //COLDER          //COLD                  //HOT                          //HOTTER                       //HOTTEST
        { BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland,    BiomeType.Desert,              BiomeType.Desert,              BiomeType.Desert },              //DRYEST
        { BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland,    BiomeType.Desert,              BiomeType.Desert,              BiomeType.Desert },              //DRYER
        { BiomeType.Ice, BiomeType.Tundra, BiomeType.Woodland,     BiomeType.Woodland,            BiomeType.Savanna,             BiomeType.Savanna },             //DRY
        { BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.Woodland,            BiomeType.Savanna,             BiomeType.Savanna },             //WET
        { BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.SeasonalForest,      BiomeType.TropicalRainforest,  BiomeType.TropicalRainforest },  //WETTER
        { BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.TemperateRainforest, BiomeType.TropicalRainforest,  BiomeType.TropicalRainforest }   //WETTEST
        };

        public static float ColdestValue = 0.05f;
        public static float ColderValue = 0.18f;
        public static float ColdValue = 0.4f;
        public static float WarmValue = 0.6f;
        public static float WarmerValue = 0.8f;
        public static float DryerValue = 0.27f;
        public static float DryValue = 0.4f;
        public static float WetValue = 0.6f;
        public static float WetterValue = 0.8f;
        public static float WettestValue = 0.9f;
        #endregion

        public static Color4 TileColors(BiomeType biome)
        {
            switch (biome)
            {
                case BiomeType.Bench:
                    return new Color4(0, 0, 0, 0);
                case BiomeType.Desert:
                    return new Color4(0, 0, 0, 0);
                case BiomeType.Savanna:
                    return new Color4(0, 0, 0, 0);
                case BiomeType.TropicalRainforest:
                    return new Color4(50, 100, 66, 255);
                case BiomeType.Grassland:
                    return new Color4(70, 175, 66, 255);
                case BiomeType.Woodland:
                    return new Color4(70, 150, 66, 255);
                case BiomeType.SeasonalForest:
                    return new Color4(70, 20, 66, 255);
                case BiomeType.TemperateRainforest:
                    return new Color4(70, 200, 66, 255);
                case BiomeType.BorealForest:
                    return new Color4(150, 85, 66, 255);
                case BiomeType.Tundra:
                    return new Color4(226, 118, 68, 255);
                case BiomeType.Ice:
                    return new Color4(0, 0, 0, 0);
                default:
                    return new Color4(0, 0, 0, 0);
            }
        }
    }
}
