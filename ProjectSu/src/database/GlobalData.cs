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
        //Temperatura
        /*public static double ColdestValue = 0.0f;
        public static double ColderValue = 0.2f;
        public static double ColdValue = 0.5f;
        public static double WarmValue = 0.8f;
        public static double WarmerValue = 1.0f;*/

        //Umidade
        /*public static double DryerValue = 0.0f;
        public static double DryValue = 0.2f;
        public static double WetValue = 0.5f;
        public static double WetterValue = 0.8f;
        public static double WettestValue = 1.0f;*/

        public static double ColdestValue = 0.05f;
        public static double ColderValue = 0.18f;
        public static double ColdValue = 0.4f;
        public static double WarmValue = 0.6f;
        public static double WarmerValue = 0.8f;
        public static double DryerValue = 0.27f;
        public static double DryValue = 0.4f;
        public static double WetValue = 0.6f;
        public static double WetterValue = 0.8f;
        public static double WettestValue = 0.9f;

        public static BiomeType[,] BiomeTable = new BiomeType[6, 6] {   
        //COLDEST        //COLDER          //COLD                  //HOT                          //HOTTER                       //HOTTEST
        { BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland,    BiomeType.Desert,              BiomeType.Desert,              BiomeType.Desert },              //DRYEST
        { BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland,    BiomeType.Desert,              BiomeType.Desert,              BiomeType.Desert },              //DRYER
        { BiomeType.Ice, BiomeType.Tundra, BiomeType.Woodland,     BiomeType.Woodland,            BiomeType.Savanna,             BiomeType.Savanna },             //DRY
        { BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.Woodland,            BiomeType.Savanna,             BiomeType.Savanna },             //WET
        { BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.SeasonalForest,      BiomeType.TropicalRainforest,  BiomeType.TropicalRainforest },  //WETTER
        { BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.TemperateRainforest, BiomeType.TropicalRainforest,  BiomeType.TropicalRainforest }   //WETTEST
        };
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
                    return new Color4(70, 175, 66, 255);
                case BiomeType.Grassland:
                    return new Color4(70, 175, 66, 255);
                case BiomeType.Woodland:
                    return new Color4(70, 175, 66, 255);
                case BiomeType.SeasonalForest:
                    return new Color4(70, 175, 66, 255);
                case BiomeType.TemperateRainforest:
                    return new Color4(70, 175, 66, 255);
                case BiomeType.BorealForest:
                    return new Color4(70, 175, 66, 255);
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
