using Projectsln.darkcomsoft.src.consolecli;
using Projectsln.darkcomsoft.src.consolecli.systemconsole;
using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.engine.render;
using Projectsln.darkcomsoft.src.entity.managers;
using Projectsln.darkcomsoft.src.enums;
using Projectsln.darkcomsoft.src.network;
using Projectsln.darkcomsoft.src.world;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Projectsln.darkcomsoft.src
{
    /// <summary>
    /// Application is the base class for the entity systesm, this is used by the server or client
    /// </summary>
    public class Application : ClassBase
    {
        public const string AppName = "ProjectSLN";
        public const string Version = "PRE-ALPHA 21pa01";

        public static readonly string BinaryPath = GetBinaryPath();
        public static readonly string AssetsPath = GetAssetsPath() + "\\Assets";

        public static ApplicationType AppType { get; private set; }

        public static BuildTypeBase gameInstance;// this is the game instance EX: Client or Server

        public static WorldManager worldManager;
        public static EntityManager entityManager;
        public static NetworkManager networkManager;

        public static WindowsConsole windowsConsole;

        public Application(ApplicationType applicationType)
        {
            AppType = applicationType;

            windowsConsole = new WindowsConsole();

            worldManager = new WorldManager();
            entityManager = new EntityManager();
            networkManager = new NetworkManager();

            for (int i = 0; i < 100; i++)
            {
                ConsoleCLI.Execute<TesteCvar>(i);
            }

            switch (applicationType)
            {
                case ApplicationType.Client:
                    StartGame();
                    break;
                case ApplicationType.Server:
                    StartServer();
                    break;
                default:
                    return;
            }
        }

        public void Tick(double time)
        {
            if (entityManager != null)
            {
                entityManager.Tick();
            }

            if (networkManager != null)
            {
                networkManager.Tick();
            }

            QueeSystem.Tick();
            windowsConsole?.Tick();
        }

        public void TickDraw(double time)
        {

        }

        protected override void OnDispose()
        {
            if (gameInstance != null)
            {
                gameInstance.Dispose();
                gameInstance = null;
            }

            networkManager?.Dispose();
            networkManager = null;

            worldManager?.Dispose();
            worldManager = null;

            entityManager?.Dispose();
            entityManager = null;

            windowsConsole?.Dispose();
            windowsConsole = null;
            base.OnDispose();
        }

        private void ReloadResources()
        {

        }

        private void StartGame()
        {
            gameInstance = new Client();
        }

        private void StartServer()
        {
            //gameInstance = new Server();
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

        /*public static long GetCPUUsage()
        {
            return Process.GetCurrentProcess().;
        }*/
    }
}
