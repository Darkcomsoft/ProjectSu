using OpenTK.Windowing.Desktop;
using Projectsln.darkcomsoft.src;
using Projectsln.darkcomsoft.src.debug;
using Projectsln.darkcomsoft.src.debug.window;
using Projectsln.darkcomsoft.src.server;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Projectsln.darkcomsoft.src
{
    class main
    {

        [MTAThread]
        static void Main(string[] args)
        {
#if Client
            StartClient();
#endif
#if Server
            StartServer();
#endif
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
            nativeWindow.Profile = OpenTK.Windowing.Common.ContextProfile.Any;//LEMBRAR DE SETAR NO SHADER A VERSAO CORE EX:#version 330 core
            nativeWindow.WindowState = OpenTK.Windowing.Common.WindowState.Normal;
            nativeWindow.WindowBorder = OpenTK.Windowing.Common.WindowBorder.Resizable;
            nativeWindow.Flags = OpenTK.Windowing.Common.ContextFlags.ForwardCompatible;


            nativeWindow.Title = Application.AppName + " : " + Application.Version;
            WindowMain game = new WindowMain(gameWindowSettings, nativeWindow);

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

        private static void StartServer()
        {
            ServerMain server = new ServerMain();

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
                server.Exit();
            }
        }

        public void ReloadGame()
        {

        }

        private static void CheckOpenAL()
        {
            if (CheckLibrary("openal32.dll"))
            {
                Debug.Log("You Have OpenAL(Audio Library), you good to go!", "OpenAL");
                //Application.NoSoundMode = false;

            }
            else
            {
                Debug.LogError("You don't Have OpanAL(Audio Library), you need to Download: https://www.openal.org/", "OpenAL");
                Debug.Log("Starting! No-Sound Mode ):", "OpenAL");
                //Application.NoSoundMode = true;
            }
        }

        public static void GoToSite(string url)
        {
            System.Diagnostics.Process.Start(url);
        }

        [DllImport("kernel32", SetLastError = true)]
        static extern IntPtr LoadLibrary(string lpFileName);

        static bool CheckLibrary(string fileName)
        {
            return LoadLibrary(fileName) != IntPtr.Zero;
        }
    }
}
