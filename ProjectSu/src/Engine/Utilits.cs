using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ProjectSu.src.Engine
{
    public static class Utilits
    {
        public static IntPtr GetIntPtr(byte[] byteBuf)//isso e um bom codigo? nao sei, nao so muito expert em pointers
        {
            IntPtr ptr = Marshal.AllocHGlobal(byteBuf.Length);
            for (int i = 0; i < byteBuf.Length; i++)
            {
                Marshal.WriteByte(ptr, i, byteBuf[i]);
            }
            return ptr;
        }

        public static IntPtr GetIntPtr(int[] byteBuf)
        {
            IntPtr ptr = Marshal.AllocHGlobal(byteBuf.Length);
            for (int i = 0; i < byteBuf.Length; i++)
            {
                Marshal.WriteByte(ptr, i, (byte)byteBuf[i]);
            }
            return ptr;
        }
    }

    public static class DRandom
    {
        public static System.Random _random = new System.Random("RNGPadraoCy8v4t498y438y9v3498y34v98yv3498y564654jklbvcflnjkvc536487j053e4tr6870.,.,.,,.".GetHashCode());

        public static void SetSeed(string seedString)
        {
            _random = new System.Random(seedString.GetHashCode());
        }

        public static void SetSeed(int seedInt)
        {
            _random = new System.Random(seedInt);
        }

        public static double Range(double MinValue, double MaxValue)
        {
            // Perform arithmetic in double type to avoid overflowing
            double range = (double)MaxValue - (double)MinValue;
            double sample = _random.NextDouble();
            double scaled = (sample * range) + MinValue;
            return (double)scaled;
        }

        public static double Range(double MinValue, double MaxValue, int Seed)
        {
            _random = new System.Random(Seed);

            // Perform arithmetic in double type to avoid overflowing
            double range = (double)MaxValue - (double)MinValue;
            double sample = _random.NextDouble();
            double scaled = (sample * range) + MinValue;
            return (double)scaled;
        }

        public static int Range(int MinValue, int MaxValue)
        {
            return _random.Next(MinValue, MaxValue);
        }

        public static int Rand()
        {
            return _random.Next();
        }

        public static double Randf()
        {
            return (double)_random.NextDouble();
        }
    }
}
