using OpenTK.Windowing.Desktop;
using ProjectSLN.darkcomsoft.src;
using ProjectSLN.darkcomsoft.src.debug;
using ProjectSLN.darkcomsoft.src.engine.window;
using ProjectSLN.darkcomsoft.src.server;
using System;
using System.Runtime.InteropServices;
using System.Runtime;
using System.Threading;
using ProjectSLN.darkcomsoft.src.misc;
using ProjectSLN.darkcomsoft.src.consolecli.systemconsole;

namespace ProjectSLN.darkcomsoft.src
{
    class main
    {
        public static WindowsConsole m_windowsConsole { get; private set; }

        [MTAThread]
        static void Main(string[] args)
        {
            foreach (var value in args)
            {
                if (value == "-debug")
                {
                    Debug.EnableDebug();
                }
                Debug.Log("Argument: " + value);
            }

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
                    game.Close();
                }
            }
        }

        private static void StartServer()
        {
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

        public void ReloadGame()
        {

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
    }
}
