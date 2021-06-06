using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using ProjectIND.darkcomsoft.src.client;
using ProjectIND.darkcomsoft.src.CLI;
using ProjectIND.darkcomsoft.src.CLI.systemconsole;
using ProjectIND.darkcomsoft.src.debug;
using ProjectIND.darkcomsoft.src.engine;
using ProjectIND.darkcomsoft.src.engine.gameobject;
using ProjectIND.darkcomsoft.src.engine.render;
using ProjectIND.darkcomsoft.src.engine.window;
using ProjectIND.darkcomsoft.src.entity.managers;
using ProjectIND.darkcomsoft.src.enums;
using ProjectIND.darkcomsoft.src.gui.guisystem;
using ProjectIND.darkcomsoft.src.network;
using ProjectIND.darkcomsoft.src.resources;
using ProjectIND.darkcomsoft.src.server;
using ProjectIND.darkcomsoft.src.world;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using ProjectIND.darkcomsoft.src.engine.physics;

namespace ProjectIND.darkcomsoft.src
{
    /// <summary>
    /// Application is the base class for the engine, define variables for client and server
    /// </summary>
    public class Application : ClassBase
    {
        public const string AppName = "ProjectIndustry";
        public const string Version = "InDev w21m6w1";

        public static readonly string BinaryPath = GetBinaryPath();
        public static readonly string AssetsPath = GetAssetsPath() + "\\Assets";

        public static Application instance { get; private set; }

        public static ApplicationType AppType { get; private set; }
        public static BuildTypeBase gameInstance { get; private set; }// this is the game instance EX: Client or Server
        public static ResourcesManager v_resourceManager { get; private set; }
        public static WorldManager v_worldManager { get; private set; }
        public static ObjectManager v_entityManager { get; private set; }
        public static Physics v_physics;

        private bool v_appIsClosing = false;

        public Application(ApplicationType applicationType)
        {
            instance = this;

            AppType = applicationType;

            PrintUtilsInfos();

            v_resourceManager = new ResourcesManager();
            v_worldManager = new WorldManager();
            v_entityManager = new ObjectManager();
            v_physics = new Physics();

            switch (applicationType)
            {
                case ApplicationType.Client:
                    StartGame();
                    break;
                case ApplicationType.Server:
                    StartServer();
                    break;
                default:
                    print("I don't know what you trying to do, but this is not a app, to start-up!");
                    return;
            }
        }

        public void Tick(double time)
        {
            if (!v_appIsClosing)
            {
                FrameQueeSystem.Tick();
                WindowsConsole.instance?.Tick();
                gameInstance?.Tick();

                v_entityManager?.Tick();
                v_worldManager?.Tick();
                v_physics?.Tick((float)time);
            }
        }

        public void TickDraw(double time)
        {
            if (!v_appIsClosing)
                gameInstance?.TickDraw();
        }

        //DAR UMA OLHADA NA ORDEM DE DISPOSE, POR QUE ESTOU FAZENDO DE QUALQUER GEITO
        protected override void OnDispose()
        {
            if (gameInstance != null)
            {
                gameInstance.Dispose();
                gameInstance = null;
            }

            v_worldManager?.Dispose();
            v_worldManager = null;

            v_entityManager?.Dispose();
            v_entityManager = null;

            v_resourceManager?.Dispose();
            v_resourceManager = null;

            v_physics.Dispose();
            v_physics = null;

            instance = null;
            base.OnDispose();
        }

        private void StartGame()
        {
            v_resourceManager.LoadPreResources(AppType);

            gameInstance = new Client();
        }

        private void StartServer()
        {
            gameInstance = new Server();
        }

        #region CLIENT-Input for GUI
        public void OnResize()
        {
            gameInstance?.OnResize();
        }

        public void OnMouseMove()
        {
            gameInstance?.OnMouseMove();
        }

        public void OnMouseDown(MouseButtonEventArgs e)
        {
            gameInstance?.OnMouseDown(e);
        }

        public void OnMouseUp(MouseButtonEventArgs e)
        {
            gameInstance?.OnMouseUp(e);
        }
        #endregion

        private void PrintUtilsInfos()
        {
            debug.Debug.Log("Game Version: " + Version);

            debug.Debug.Log(".NET Version: " + RuntimeInformation.FrameworkDescription);
            debug.Debug.Log("OpenTK Version: 4.6.3");
            debug.Debug.Log("OpenGL Version: " + GL.GetString(StringName.Version));
            debug.Debug.Log("Shader Version: " + GL.GetString(StringName.ShadingLanguageVersion));
        }

        public static void CloseApp()
        {
            instance.v_appIsClosing = true;

            switch (AppType)
            {
                case ApplicationType.Client:
                    WindowMain.Instance.Close();
                    break;
                case ApplicationType.Server:
                    ServerMain.Exit();
                    break;
            }
        }

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
            return Process.GetCurrentProcess().PrivateMemorySize64 / (1024 * 1024);
        }

        public static long GetVirtualMemory()
        {
            return Process.GetCurrentProcess().VirtualMemorySize64 / (1024 * 1024);
        }
    }
}
