using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.Engine
{
    public class Application
    {
        public const string AppName = "ProjectSu";
        public const string Version = "PRE-ALPHA 21pa01";

        public static int TargetFrameRate = 60;

        public static readonly string BinaryPath = GetBinaryPath();
        public static readonly string AssetsPath = GetAssetsPath() + "\\Assets";

        private static string GetBinaryPath()
        {
#if DEBUG
            return Directory.GetCurrentDirectory();
#else
            return Directory.GetCurrentDirectory();
#endif
        }

        private static string GetAssetsPath()
        {
#if DEBUG
#if Client
            return Directory.GetCurrentDirectory().Replace("\\x64\\Debug\\netcoreapp3.1", "");
#elif Server
            return Directory.GetCurrentDirectory().Replace("\\x64\\Debug Server\\netcoreapp3.1", "");
#endif
#else
#if Client
            return Directory.GetCurrentDirectory().Replace("\\x64\\Release\\netcoreapp3.1", "");
#elif Server
            return Directory.GetCurrentDirectory().Replace("\\x64\\Release Server\\netcoreapp3.1", "");
#endif
#endif
        }

        public static long GetPrivateMemory()
        {
            return Process.GetCurrentProcess().PrivateMemorySize64 / (1024*1024);
        }

        public static long GetVirtualMemory()
        {
            return Process.GetCurrentProcess().VirtualMemorySize64 / (1024 * 1024);
        }

        /*public static long GetCPUUsage()
        {
            return Process.GetCurrentProcess().;
        }*/
    }
}
