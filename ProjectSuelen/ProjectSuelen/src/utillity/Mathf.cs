using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.utillity
{
    public static class Mathf
    {
        public static int FloorToInt(double value) => (int)Math.Floor(value);
        public static double Floor(double value) => (double)Math.Floor(value);
        public static double Round(double value) => (double)Math.Round(value);
    }
}
