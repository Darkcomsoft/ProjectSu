using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace ProjectSu.src.Engine
{
    public static class Debug
    {
        public static BeginMode GLBeginMode = BeginMode.Triangles;

        public static void Log(string menssage)
        {
#if Client
#if Release
            Console.WriteLine("LOG: " + menssage);
#else
            System.Diagnostics.Debug.WriteLine("LOG: " + menssage);
#endif
#elif Server
            Server.Instance.WriteLine("LLOG: " + menssage);
#endif
        }

        public static void LogWarning(string menssage)
        {
#if Client
#if Release
            Console.WriteLine("WARNING: " + menssage);
#else
           System.Diagnostics.Debug.WriteLine("WARNING: " + menssage);
#endif
#elif  Server
            Server.Instance.WriteLine("WARNING: " + menssage);
#endif
        }

        public static void LogError(string menssage)
        {
#if  Client
            System.Diagnostics.Debug.Fail(menssage);

#if Release
            Console.WriteLine("ERROR: " + menssage);
#else
           System.Diagnostics.Debug.Fail(menssage);
#endif
#elif Server
            Server.Instance.WriteLine("ERROR: " + menssage);
#endif
        }

        public static void LogException(string menssage)
        {
#if Client
            throw new Exception(menssage);
#elif Server
            Server.Instance.WriteLine("Exception: " + menssage);
#endif
        }
    }
}