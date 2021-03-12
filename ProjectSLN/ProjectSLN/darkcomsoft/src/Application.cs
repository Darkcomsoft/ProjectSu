using Projectsln.darkcomsoft.src.consolecli;
using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.engine.render;
using Projectsln.darkcomsoft.src.entity.managers;
using Projectsln.darkcomsoft.src.world;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Projectsln.darkcomsoft.src
{
    public class Application : ClassBase
    {
        public const string AppName = "ProjectSLN";
        public const string Version = "PRE-ALPHA 21pa01";

        public static readonly string BinaryPath = GetBinaryPath();
        public static readonly string AssetsPath = GetAssetsPath() + "\\Assets";

        public static bool NoSoundMode = false;

        public static BuildTypeBase gameInstance;// this is the game instance EX: Client or Server

        public static WorldManager worldManager;
        public static EntityManager entityManager;

        public Application()
        {
            worldManager = new WorldManager();
            entityManager = new EntityManager();

            for (int i = 0; i < 100; i++)
            {
                ConsoleCLI.Execute<TesteCvar>(i);
            }

            StartGame();
        }

        public void Tick(double time)
        {
            if (entityManager != null)
            {
                entityManager.Tick();
            }

            QueeSystem.Tick();
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

            worldManager?.Dispose();
            worldManager = null;

            entityManager?.Dispose();
            entityManager = null;
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
