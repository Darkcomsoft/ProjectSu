using OpenTK.Windowing.Desktop;
using ProjectIND.darkcomsoft.src;
using ProjectIND.darkcomsoft.src.debug;
using ProjectIND.darkcomsoft.src.engine.window;
using ProjectIND.darkcomsoft.src.server;
using System;
using System.Runtime.InteropServices;
using System.Runtime;
using System.Threading;
using ProjectIND.darkcomsoft.src.misc;
using ProjectIND.darkcomsoft.src.CLI.systemconsole;
using ProjectIND.darkcomsoft.src.enums;

namespace ProjectIND.darkcomsoft.src
{
    class main
    {
        private const int m_GcCleanOutOfMemoryException = 2;

        public static WindowsConsole m_windowsConsole { get; private set; }
        private static ApplicationType m_appType;

        [MTAThread]
        static void Main(string[] args)
        {
            LoadLauncheArguments(args);
            CheckDevMode();

            GCSettings.LatencyMode = GCLatencyMode.LowLatency;

            m_windowsConsole = new WindowsConsole();
#if Client
            StartClient();
#endif
#if Server
            StartServer();
#endif

            m_windowsConsole?.Dispose();
            m_windowsConsole = null;
        }

        private static void StartClient()
        {
            CheckOpenAL();

            m_appType = ApplicationType.Client;

            GameWindowSettings gameWindowSettings = GameWindowSettings.Default;
            gameWindowSettings.IsMultiThreaded = false;//ISSO TAMBEM NAO ESTA SENDO USADO, SE PA NUNCA SERA NAO SEI
            gameWindowSettings.RenderFrequency = 60;//POR EM QUANTO SETAR O FRAME RATE ESTA DISABILITADO
            gameWindowSettings.UpdateFrequency = 60;

            NativeWindowSettings nativeWindow = NativeWindowSettings.Default;
            nativeWindow.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            nativeWindow.APIVersion = new Version(3, 3);
            nativeWindow.Profile = OpenTK.Windowing.Common.ContextProfile.Core;//LEMBRAR DE SETAR NO SHADER A VERSAO CORE EX:#version 330 core
            nativeWindow.WindowState = OpenTK.Windowing.Common.WindowState.Normal;
            nativeWindow.WindowBorder = OpenTK.Windowing.Common.WindowBorder.Resizable;
            nativeWindow.Flags = OpenTK.Windowing.Common.ContextFlags.Default;

            nativeWindow.Title = Application.AppName + " : " + Application.Version;

            using (WindowMain game = new WindowMain(gameWindowSettings, nativeWindow))
            {
                try
                {
                    game.Run();
                }
                catch (OutOfMemoryException memoryEx)
                {
                    Debug.LogWarning("GC: " + memoryEx.Message, "Main");
                    for (int i = 0; i < m_GcCleanOutOfMemoryException; i++)
                    {
                        Debug.LogWarning("GC: Collecting Garbage!", "Main");
                        GC.Collect();
                        Debug.LogWarning("GC: Clean-up, waiting to clean again!", "Main");
                        Thread.Sleep(1000);
                    }
                    return;
                }
                catch (Exception ex)
                {
                    Debug.LogFail(ex.Message + " StackTrace: " + ex.StackTrace);
                }
                finally
                {
                    game.Close();
                }
            }
        }

        private static void StartServer()
        {
            m_appType = ApplicationType.Server;

            using (ServerMain server = new ServerMain())
            {
                try
                {
                    server.Run();
                }
                catch (OutOfMemoryException memoryEx)
                {
                    Debug.LogWarning("GC: " + memoryEx.Message, "Main");
                    for (int i = 0; i < 2; i++)
                    {
                        Debug.LogWarning("GC: Collecting Garbage!", "Main");
                        GC.Collect();
                        Debug.LogWarning("GC: Clean-up, waiting to clean again!", "Main");
                        Thread.Sleep(1000);
                    }
                    return;
                }
                catch (Exception ex)
                {
                    Debug.LogFail(ex.Message + " StackTrace: " + ex.StackTrace);
                }
                finally
                {
                    ServerMain.Exit();
                }
            }
        }

        private static void CheckOpenAL()
        {
            if (Utilits.CheckDLLExist("openal32.dll"))
            {
                Debug.Log("You Have OpenAL(Audio Library), you good to go!", "OpenAL");
                //Application.NoSoundMode = false;
            }
            else
            {
                Debug.LogError("You don't Have OpenAL(Audio Library), you need to Download: https://www.openal.org/", "OpenAL");
                Debug.Log("Starting! No-Sound Mode ):", "OpenAL");
                //Application.NoSoundMode = true;
            }
        }

        private static void LoadLauncheArguments(string[] arg)
        {
            foreach (var value in arg)
            {
                if (value == "-debug")
                {
                    Debug.EnableDebug();
                }
                Debug.Log("Argument: " + value);
            }
        }

        private static void CheckDevMode()
        {
#if DEBUG
            //if is runing a debug version, set by default this
            Debug.EnablePrintDebug();
#else
            Debug.DisablePrintDebug();
#endif
        }
    }
}
