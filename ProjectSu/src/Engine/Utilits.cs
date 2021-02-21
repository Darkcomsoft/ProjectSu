using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ProjectSu.src.Engine
{
    public static class Utilits
    {
        public static IntPtr GetIntPtr(Byte[] byteBuf)
        {
            IntPtr ptr = Marshal.AllocHGlobal(byteBuf.Length);
            for (int i = 0; i < byteBuf.Length; i++)
            {
                Marshal.WriteByte(ptr, i, byteBuf[i]);
            }
            return ptr;
        }
    }
}
