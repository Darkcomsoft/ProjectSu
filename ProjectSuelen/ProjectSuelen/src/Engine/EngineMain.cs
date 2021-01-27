using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using ProjectSuelen.src.Engine.AssetsPipeline;
using ProjectSuelen.src.Engine.NetCode;
using ProjectSuelen.src.Engine.PhysicsSystem;
using ProjectSuelen.src.Engine.UI;
using ProjectSuelen.src.Spaces;
using ProjectSuelen.src.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.Engine
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

        public EngineMain()
        {
            assetManager = new AssetManager();

            basicSystem = new BasicSystem();

            MouseCursor.UnLockCursor();
        }

        public void Tick()
        {
            if (AssetManager.AssetsReady && engineReady)
            {
                QueeSystem.Tick();

                if (Input.GetKeyDown(Key.F11))
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

                if (Input.GetKeyDown(Key.Escape))
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

                MouseCursor.CursorLockPosition();
                Network.NetworkTick();
                basicSystem?.Tick();
                physics?.UpdatePhisics(Time._DeltaTime);
                tickManager?.Tick();
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
                if (tickManager != null)
                {
                    tickManager.TickDraw();
                }
                GL.Disable(EnableCap.DepthTest);
                basicSystem.TickRender();
            }
            else
            {
                GL.Disable(EnableCap.DepthTest);
                basicSystem.TickRender();
            }
        }

        protected override void OnDispose()
        {
            engineReady = false;

            ClearEngine();

            if (basicSystem != null)
            {
                basicSystem.Dispose();
            }

            if (assetManager != null)
            {
                assetManager.Dispose();
            }
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
            ClearSpaces();

            if (worldManager != null)
            {
                worldManager.Dispose();
            }

            if (tickManager != null)
            {
                tickManager.Dispose();
            }

            if (physics != null)
            {
                physics.Dispose();
            }

            if (soundSystem != null)
            {
                soundSystem.Dispose();
            }
        }

        #region InputFunctions
        public void OnResize()
        {
            if (GUI.instance != null)
            {
                GUI.instance.OnResize();
            }
        }

        public void OnKeyPress(KeyPressEventArgs e)
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
