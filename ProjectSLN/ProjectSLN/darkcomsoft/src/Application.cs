using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using ProjectSLN.darkcomsoft.src.client;
using ProjectSLN.darkcomsoft.src.CLI;
using ProjectSLN.darkcomsoft.src.CLI.systemconsole;
using ProjectSLN.darkcomsoft.src.debug;
using ProjectSLN.darkcomsoft.src.engine;
using ProjectSLN.darkcomsoft.src.engine.gameobject;
using ProjectSLN.darkcomsoft.src.engine.render;
using ProjectSLN.darkcomsoft.src.engine.window;
using ProjectSLN.darkcomsoft.src.entity.managers;
using ProjectSLN.darkcomsoft.src.enums;
using ProjectSLN.darkcomsoft.src.gui.guisystem;
using ProjectSLN.darkcomsoft.src.network;
using ProjectSLN.darkcomsoft.src.resources;
using ProjectSLN.darkcomsoft.src.server;
using ProjectSLN.darkcomsoft.src.world;
using ProjectSLN.darkcomsoft.src.engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace ProjectSLN.darkcomsoft.src
{
    /// <summary>
    /// Application is the base class for the entity systesm, this is used by the server or client
    /// </summary>
    public class Application : ClassBase
    {
        public const string AppName = "ProjectSLN";
        public const string Version = "InDev w21m4w2";

        public static readonly string BinaryPath = GetBinaryPath();
        public static readonly string AssetsPath = GetAssetsPath() + "\\Assets";

        public static Application instance { get; private set; }

        public static ApplicationType AppType { get; private set; }
        public static AppNetworkType NetworkType { get; private set; }
        public static BuildTypeBase gameInstance { get; private set; }// this is the game instance EX: Client or Server
        public static ResourcesManager m_resourceManager { get; private set; }
        public static WorldManager m_worldManager { get; private set; }
        public static ObjectManager m_entityManager { get; private set; }

        private bool m_appIsClosing = false;

        public Application(ApplicationType applicationType)
        {
            instance = this;

            AppType = applicationType;

            PrintUtilsInfos();

            m_resourceManager = new ResourcesManager();
            m_worldManager = new WorldManager();
            m_entityManager = new ObjectManager();

            switch (applicationType)
            {
                case ApplicationType.Client:
                    StartGame();
                    break;
                case ApplicationType.Server:
                    StartServer();
                    break;
                default:
                    ProjectSLN.darkcomsoft.src.debug.Debug.Log("I don't know what you trying to do, but this is not a app, to start-up!");
                    return;
            }
        }

        public void Tick(double time)
        {
            if (!m_appIsClosing)
            {
                FrameQueeSystem.Tick();
                WindowsConsole.instance?.Tick();
                gameInstance?.Tick();

                m_entityManager?.Tick();
                m_worldManager?.Tick();
            }
        }

        public void TickDraw(double time)
        {
            if (!m_appIsClosing)
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

            m_worldManager?.Dispose();
            m_worldManager = null;

            m_entityManager?.Dispose();
            m_entityManager = null;

            m_resourceManager?.Dispose();
            m_resourceManager = null;

            instance = null;
            base.OnDispose();
        }

        private void StartGame()
        {
            m_resourceManager.LoadPreResources(AppType);

            gameInstance = new Client();
        }

        private void StartServer()
        {
            gameInstance = new Server();
        }

        public static void SetNetworkType(AppNetworkType appNetworkType)
        {
            NetworkType = appNetworkType;
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
            instance.m_appIsClosing = true;

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

        /// <summary>
        /// Check if the app is runing a network Server-Dedicated or Client Hosting a Server
        /// </summary>
        public static bool IsServer { get { if (NetworkType == AppNetworkType.Server || NetworkType == AppNetworkType.ClientSinglePlayerServer) { return true; } return false; } }
    }
}
