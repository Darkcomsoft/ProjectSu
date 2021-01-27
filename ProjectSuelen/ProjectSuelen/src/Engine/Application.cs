using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.Engine
{
    public class Application
    {
        public const string AppName = "ProjectSuelen";
        public const string Version = "DEV 0.0.1";

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
            return Directory.GetCurrentDirectory().Replace("\\x64\\Debug", "");
#elif Server
            return Directory.GetCurrentDirectory().Replace("\\x64\\Debug Server", "");
#endif
#else
#if Client
            return Directory.GetCurrentDirectory().Replace("\\x64\\Release", "");
#elif Server
            return Directory.GetCurrentDirectory().Replace("\\x64\\Release Server", "");
#endif
#endif
        }
    }
}
