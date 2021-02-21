using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProjectSu.src;
using System.Windows;
using System.Runtime.InteropServices;
using ProjectSu.src.Engine;
using OpenTK.Windowing.Desktop;

namespace ProjectSu.src
{
    public class main
    {
        [MTAThread]
        public static void Main()
        {
            CheckOpenAL();

            GameWindowSettings gameWindowSettings = new GameWindowSettings();
            gameWindowSettings.IsMultiThreaded = true;
            gameWindowSettings.RenderFrequency = 60;
            gameWindowSettings.UpdateFrequency = 60;

            NativeWindowSettings nativeWindow = new NativeWindowSettings();
            nativeWindow.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            nativeWindow.APIVersion = new Version(3,3);
            nativeWindow.Profile = OpenTK.Windowing.Common.ContextProfile.Core;
            nativeWindow.WindowState = OpenTK.Windowing.Common.WindowState.Normal;
            nativeWindow.WindowBorder = OpenTK.Windowing.Common.WindowBorder.Resizable;
            nativeWindow.Title = Application.AppName + " : " + Application.Version;
            nativeWindow.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            nativeWindow.StartVisible = true;
            nativeWindow.StartFocused = false;

            using (Window game = new Window(gameWindowSettings, nativeWindow))
            {
                try
                {
                    game.Run();
                }
                catch (OutOfMemoryException memoryEx)
                {
                    Debug.LogWarning("GC: " + memoryEx.Message);
                    for (int i = 0; i < 2; i++)
                    {
                        Debug.LogWarning("GC: Collecting Garbage!");
                        GC.Collect();
                        Debug.LogWarning("GC: Clean-up, waiting to clean again!");
                        Thread.Sleep(1000);
                    }
                    return;
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message + " StackTrace: " + ex.StackTrace);
                }
            }

            AppDomain.CurrentDomain.UnhandledException -= new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

#if Server
            using (Server server = new Server())
            {
                server.Run();
            }
#endif
        }

        private static void CheckOpenAL()
        {
            if (CheckLibrary("openal32.dll"))
            {
                Debug.Log("You Have OpenAL(Audio Library), you good to go!");
                Application.NoSoundMode = false;

            }
            else
            {
                Debug.LogError("You don't Have OpanAL(Audio Library), you need to Download: https://www.openal.org/");
                Debug.Log("Starting! No-Sound Mode ):");
                Application.NoSoundMode = true;
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
