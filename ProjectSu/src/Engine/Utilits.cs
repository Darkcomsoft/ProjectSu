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
}
