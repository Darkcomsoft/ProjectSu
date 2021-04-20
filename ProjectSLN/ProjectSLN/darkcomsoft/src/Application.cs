﻿using OpenTK.Windowing.Common;
using Projectsln.darkcomsoft.src.consolecli;
using Projectsln.darkcomsoft.src.consolecli.systemconsole;
using Projectsln.darkcomsoft.src.debug;
using Projectsln.darkcomsoft.src.debug.render;
using Projectsln.darkcomsoft.src.debug.window;
using Projectsln.darkcomsoft.src.entity.managers;
using Projectsln.darkcomsoft.src.enums;
using Projectsln.darkcomsoft.src.gui.guisystem;
using Projectsln.darkcomsoft.src.network;
using Projectsln.darkcomsoft.src.resources;
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
        public const string Version = "InDev w21m4w2";

        public static readonly string BinaryPath = GetBinaryPath();
        public static readonly string AssetsPath = GetAssetsPath() + "\\Assets";


        public static ApplicationType AppType { get; private set; }
        public static BuildTypeBase gameInstance;// this is the game instance EX: Client or Server
        public static ResourcesManager m_resourceManager;
        public static Input input;
        public static GUI m_gui;
        public static WorldManager m_worldManager;
        public static EntityManager m_entityManager;
        public static NetworkManager m_networkManager;
        public static WindowsConsole m_windowsConsole;

        public Application(ApplicationType applicationType)
        {
            AppType = applicationType;

            m_resourceManager = new ResourcesManager();
            m_windowsConsole = new WindowsConsole();

            m_worldManager = new WorldManager();
            m_entityManager = new EntityManager();
            m_networkManager = new NetworkManager();

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
            m_entityManager?.Tick();
            m_networkManager?.Tick();

            QueeSystem.Tick();
            m_gui?.Tick(time);
            m_windowsConsole?.Tick();
            gameInstance?.Tick();
        }

        public void TickDraw(double time)
        {
            gameInstance?.TickDraw();
            m_gui?.Draw(time);
        }

        protected override void OnDispose()
        {
            if (gameInstance != null)
            {
                gameInstance.Dispose();
                gameInstance = null;
            }

            m_networkManager?.Dispose();
            m_networkManager = null;

            m_worldManager?.Dispose();
            m_worldManager = null;

            m_entityManager?.Dispose();
            m_entityManager = null;

            m_windowsConsole?.Dispose();
            m_windowsConsole = null;

            m_gui?.Dispose();
            m_gui = null;

            m_resourceManager?.Dispose();
            m_resourceManager = null;

            input?.Dispose();
            input = null;
            base.OnDispose();
        }

        private void ReloadResources()
        {

        }

        private void StartGame()
        {
            m_resourceManager.LoadPreResources(AppType);

            input = new Input();
            m_gui = new GUI();

            gameInstance = new Client();
        }

        private void StartServer()
        {
            //gameInstance = new Server();
        }

        public void OnResize()
        {
            m_gui?.OnResize();
        }

        public void OnMouseMove()
        {
            m_gui?.OnMouseMove();
        }

        public void OnMouseDown(MouseButtonEventArgs e)
        {
            m_gui?.OnMousePress(e);
        }

        public void OnMouseUp(MouseButtonEventArgs e)
        {
            m_gui?.OnMouseRelease(e);
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
