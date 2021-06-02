using OpenTK.Graphics.OpenGL;
using ProjectIND.darkcomsoft.src.CLI.systemconsole;
using ProjectIND.darkcomsoft.src.server;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.debug
{
    public static class Debug
    {
        public static BeginMode GLBeginMode = BeginMode.Triangles;
        private static bool v_debugMode = false;

        private static bool v_printDebugMode = false;

        public static void EnableDebug()
        {
            v_debugMode = true;
        }

        public static void DisableDebug()
        {
            v_debugMode = false;
        }

        public static void EnablePrintDebug()
        {
            v_printDebugMode = true;
        }

        public static void DisablePrintDebug()
        {
            v_printDebugMode = false;
        }

        private static string GetPrintTime()
        {
            return string.Format("[{0}:{1}:{2}]", System.DateTime.Now.TimeOfDay.Hours, System.DateTime.Now.TimeOfDay.Minutes, System.DateTime.Now.TimeOfDay.Seconds);
        }

        private static void WriteConsole(string msg)
        {
            if (WindowsConsole.isOpen)
            {
                WindowsConsole.WriteLine(msg);
            }
        }

        public static void Log(string menssage, string caller = "")
        {
            if (!v_printDebugMode) { return; }

            string callerName = "";

            if (caller != string.Empty)
            {
                callerName = string.Format("[{0}]", caller);
            }
#if Client
#if Release
            WriteConsole( string.Format("{0} {1}LOG: {2}", GetPrintTime(), callerName, menssage));
#else
            System.Diagnostics.Debug.WriteLine( string.Format("{0} {1}LOG: {2}", GetPrintTime(), callerName, menssage));

            WriteConsole(string.Format("{0} {1}LOG: {2}", GetPrintTime(), callerName, menssage));
#endif
#elif Server
           WriteConsole(string.Format("{0} {1}LOG: {2}", GetPrintTime(), callerName, menssage));
#endif
        }

        public static void LogWarning(string menssage, string caller = "")
        {
            if (!v_printDebugMode) { return; }

            string callerName = "";

            if (caller != string.Empty)
            {
                callerName = string.Format("[{0}]", caller);
            }
#if Client
#if Release
            WriteConsole(string.Format("{0} {1}WARNING: {2}", GetPrintTime(), caller,menssage));
#else
            System.Diagnostics.Debug.WriteLine(string.Format("{0} {1}WARNING: {2}", GetPrintTime(), caller,menssage));
            WriteConsole(string.Format("{0} {1}WARNING: {2}", GetPrintTime(), caller, menssage));
#endif
#elif Server
            WriteConsole(string.Format("{0} {1}WARNING: {2}", GetPrintTime(), caller, menssage));
#endif
        }

        public static void LogError(string menssage, string caller = "")
        {
            if (!v_printDebugMode) { return; }

            string callerName = "";

            if (caller != string.Empty)
            {
                callerName = string.Format("[{0}]", caller);
            }
#if Client
#if Release
            WriteConsole(string.Format("{0} {1}ERROR: {2}", GetPrintTime(), caller, menssage));
#else
            System.Diagnostics.Debug.WriteLine(string.Format("{0} {1}ERROR: {2}", GetPrintTime(), caller, menssage));
            WriteConsole(string.Format("{0} {1}ERROR: {2}", GetPrintTime(), caller, menssage));
#endif
#elif Server
            WriteConsole(string.Format("{0} {1}ERROR: {2}", GetPrintTime(), caller, menssage));
#endif
        }

        public static void LogFail(string menssage)
        {
#if  Client
            System.Diagnostics.Debug.Fail(menssage);

#if Release
            WriteConsole("ERROR: " + menssage);
            System.Diagnostics.Debug.Fail(menssage);
#else
            System.Diagnostics.Debug.Fail(menssage);
#endif
#elif Server
            WriteConsole("ERROR: " + menssage);
            System.Diagnostics.Debug.Fail(menssage);
#endif
        }

        public static void LogException(string menssage)
        {
#if Client
            throw new Exception(menssage);
#elif Server
            WriteConsole("Exception: " + menssage);
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

        public static bool isDebugEnabled { get { return v_debugMode; } }
    }
}
