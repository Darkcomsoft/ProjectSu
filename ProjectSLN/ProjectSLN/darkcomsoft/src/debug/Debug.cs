using OpenTK.Graphics.OpenGL;
using Projectsln.darkcomsoft.src.server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.debug
{
    public static class Debug
    {
        public static BeginMode GLBeginMode = BeginMode.Triangles;

        private static string GetPrintTime()
        {
            return string.Format("[{0}:{1}:{2}]", System.DateTime.Now.TimeOfDay.Hours, System.DateTime.Now.TimeOfDay.Minutes, System.DateTime.Now.TimeOfDay.Seconds);
        }

        public static void Log(string menssage, string caller = "")
        {
            string callerName = "";

            if (caller != string.Empty)
            {
                callerName = string.Format("[{0}]", caller);
            }
#if Client
#if Release
            Console.WriteLine( string.Format("{0} {1}LOG: {2}", GetPrintTime(), callerName, menssage));
#else
            System.Diagnostics.Debug.WriteLine( string.Format("{0} {1}LOG: {2}", GetPrintTime(), callerName, menssage));
#endif
#elif Server
            Console.WriteLine(string.Format("{0} {1}LOG: {2}", GetPrintTime(), callerName, menssage));
#endif
        }

        public static void LogWarning(string menssage, string caller = "")
        {
            string callerName = "";

            if (caller != string.Empty)
            {
                callerName = string.Format("[{0}]", caller);
            }
#if Client
#if Release
            Console.WriteLine(string.Format("{0} {1}WARNING: {2}", GetPrintTime(), caller,menssage));
#else
            System.Diagnostics.Debug.WriteLine(string.Format("{0} {1}WARNING: {2}", GetPrintTime(), caller,menssage));
#endif
#elif Server
            Console.WriteLine(string.Format("{0} {1}WARNING: {2}", GetPrintTime(), caller, menssage));
#endif
        }

        public static void LogError(string menssage, string caller = "")
        {
            string callerName = "";

            if (caller != string.Empty)
            {
                callerName = string.Format("[{0}]", caller);
            }
#if Client
#if Release
            Console.WriteLine(string.Format("{0} {1}ERROR: {2}", GetPrintTime(), caller, menssage));
#else
            System.Diagnostics.Debug.WriteLine(string.Format("{0} {1}ERROR: {2}", GetPrintTime(), caller, menssage));
#endif
#elif Server
            Console.WriteLine(string.Format("{0} {1}ERROR: {2}", GetPrintTime(), caller, menssage));
#endif
        }

        public static void LogFail(string menssage)
        {
#if  Client
            System.Diagnostics.Debug.Fail(menssage);

#if Release
            Console.WriteLine("ERROR: " + menssage);
#else
            System.Diagnostics.Debug.Fail(menssage);
#endif
#elif Server
            Console.WriteLine("ERROR: " + menssage);
#endif
        }

        public static void LogException(string menssage)
        {
#if Client
            throw new Exception(menssage);
#elif Server
            Console.WriteLine("Exception: " + menssage);
#endif
        }


        #region cvarDebug
        public static void cvarLog(string menssage)
        {
#if Client
#if Release
            Console.WriteLine( string.Format("{0} [cvar]LOG: {1}", GetPrintTime(), menssage));
#else
            System.Diagnostics.Debug.WriteLine(string.Format("{0} [cvar]LOG: {1}", GetPrintTime(), menssage));
#endif
#elif Server
            System.Diagnostics.Debug.WriteLine( string.Format("{0} [cvar]LOG: {1}", GetPrintTime(), menssage));
#endif
        }

        public static void cvarError(string menssage)
        {
#if Client
#if Release
            Console.WriteLine( string.Format("{0} [cvar]ERROR: {1}", GetPrintTime(), menssage));
#else
            System.Diagnostics.Debug.WriteLine(string.Format("{0} [cvar]ERROR: {1}", GetPrintTime(), menssage));
#endif
#elif  Server
            System.Diagnostics.Debug.WriteLine( string.Format("{0} [cvar]ERROR: {1}", GetPrintTime(), menssage));
#endif
        }
        #endregion
    }
}
