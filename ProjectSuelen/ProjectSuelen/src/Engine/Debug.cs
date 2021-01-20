using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.Engine
{
    public static class Debug
    {
        public static void Log(string menssage)
        {
#if Client
            System.Diagnostics.Debug.WriteLine("LOG: " + menssage);
#elif Server
            Server.Instance.WriteLine("LLOG: " + menssage);
#endif
        }

        public static void LogWarning(string menssage)
        {
#if  Client
            System.Diagnostics.Debug.WriteLine("WARNING: " + menssage);
#elif  Server
            Server.Instance.WriteLine("WARNING: " + menssage);
#endif
        }

        public static void LogError(string menssage)
        {
#if  Client
            System.Diagnostics.Debug.Fail(menssage);
            //Console.Error.WriteLine("Unhandled exception: " + args.ExceptionObject);
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