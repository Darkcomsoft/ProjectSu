﻿using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ProjectSu.src.Engine.AssetsPipeline;
using ProjectSu.src.Engine.NetCode;
using ProjectSu.src.Engine.PhysicsSystem;
using ProjectSu.src.Engine.UI;
using ProjectSu.src.Spaces;
using ProjectSu.src.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.Engine
{
    /// <summary>
    /// All engine logic, is like the Window Class but just for the systems
    /// </summary>
    public class EngineMain : ClassBase
    {
        private static bool engineReady = false;
        private bool GameLoaded = false;

        private BasicSystem basicSystem;

        private AssetManager assetManager;

        private Physics physics;
        private SoundSystem soundSystem;
        private SpaceManager spaceManager;
        private TickManager tickManager;
        private WorldManager worldManager;

        private DebugGUI debugGUI;

        public EngineMain()
        {
            assetManager = new AssetManager();

            basicSystem = new BasicSystem();
            debugGUI = new DebugGUI();

            MouseCursor.UnLockCursor();
        }

        public void Tick()
        {
            basicSystem?.Tick();
            
            if (AssetManager.AssetsReady && engineReady)
            {
                if (Input.GetKeyDown(Keys.F11))
                {
                    if (Window.Instance.WindowState == WindowState.Fullscreen)
                    {
                        Window.Instance.WindowState = WindowState.Normal;
                    }
                    else
                    {
                        Window.Instance.WindowState = WindowState.Fullscreen;
                    }
                }

                if (Input.GetKeyDown(Keys.Escape))
                {
                    if (MouseCursor.MouseLocked)
                    {
                        MouseCursor.UnLockCursor();
                    }
                    else
                    {
                        MouseCursor.LockCursor();
                    }
                }

                QueeSystem.Tick();
                Network.NetworkTick();
                tickManager?.Tick();
                debugGUI?.Tick();
                physics?.UpdatePhisics(Time._DeltaTime);
                MouseCursor.CursorLockPosition();
            }
            else if (!GameLoaded)//first load the window, before load the systems
            {
                StartGame();
            }
        }

        public void TickRender()
        {
            if (AssetManager.AssetsReady && engineReady)
            {
                GL.Enable(EnableCap.DepthTest);
                tickManager?.TickDraw();
                GL.Disable(EnableCap.DepthTest);
                basicSystem?.TickRender();
                GL.Enable(EnableCap.DepthTest);
            }
            else
            {
                GL.Disable(EnableCap.DepthTest);
                basicSystem?.TickRender();
                GL.Enable(EnableCap.DepthTest);
            }
        }

        protected override void OnDispose()
        {
            engineReady = false;

            ClearEngine();
            debugGUI?.Dispose();
            basicSystem?.Dispose();
            assetManager?.Dispose();
            base.OnDispose();
        }

        /// <summary>
        /// This is used for reload the game, like if you first started
        /// </summary>
        public void ReloadGame()
        {
            engineReady = false;

            ClearEngine();

            assetManager.ReloadAssets();

            BasicScreen.SetStatus(ScreenType.SplashScreen, "Starting EngineSystem");
            physics = new Physics();
            soundSystem = new SoundSystem();
            tickManager = new TickManager();
            spaceManager = new SpaceManager();
            worldManager = new WorldManager();

            BasicScreen.SetStatus(ScreenType.SplashScreen, "Creating Spaces");
            SpaceManager.AddSpace("GlobalSpace", new GlobalSpace());
            SpaceManager.AddSpace("MainMenuSpace", new MainMenuSpace());

            BasicScreen.SetStatus(ScreenType.SplashScreen, "Finished Loading Good to Go (:");
            engineReady = true;

            BasicScreen.SetStatus();
        }

        public void StartGame()
        {
            engineReady = false;
            GameLoaded = false;
            assetManager.StartLoadAssets();

            BasicScreen.SetStatus(ScreenType.SplashScreen, "Starting EngineSystem");
            physics = new Physics();
            soundSystem = new SoundSystem();
            tickManager = new TickManager();
            spaceManager = new SpaceManager();
            worldManager = new WorldManager();

            BasicScreen.SetStatus(ScreenType.SplashScreen, "Creating Spaces");
            SpaceManager.AddSpace("GlobalSpace", new GlobalSpace());
            SpaceManager.AddSpace("MainMenuSpace", new MainMenuSpace());

            BasicScreen.SetStatus(ScreenType.SplashScreen, "Finished Loading Good to Go (:");
            engineReady = true;
            GameLoaded = true;
            BasicScreen.SetStatus();
        }

        /// <summary>
        /// Used for reload all assets, only assets
        /// </summary>
        public void ReloadAssets()
        {

        }

        public void ClearSpaces()
        {
            spaceManager.Dispose();
        }

        private void ClearEngine()
        {
            worldManager?.Dispose();
            tickManager?.Dispose();
            physics?.Dispose();
            soundSystem?.Dispose();

            ClearSpaces();

            QueeSystem.CleanUp();
        }

        #region InputFunctions
        public void OnResize()
        {
            if (GUI.instance != null)
            {
                GUI.instance.OnResize();
            }
        }

        public void OnKeyPress(KeyboardKeyEventArgs e)
        {
            if (GUI.instance != null)
            {
                GUI.instance.OnKeyPress(e);
            }
        }

        public void OnMouseMove(MouseMoveEventArgs e)
        {
            if (GUI.instance != null)
            {
                GUI.instance.OnMouseMove(e);
            }
            Input.SetMousePos(e.Position);
        }
        #endregion

        public static bool EngineReady { get => engineReady; private set { } }
    }
}