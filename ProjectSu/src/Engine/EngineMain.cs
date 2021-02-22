using OpenTK;
using OpenTK.Graphics.OpenGL;
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
                if (Input.GetKeyDown(Keys.F11, 0))
                {
                    Window.Instance.SetFullScreen();
                }

                if (Input.GetKeyDown(Keys.Escape, 1))
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
        /// This is used for reload the game ,engine systems, assets etc.
        /// </summary>
        public void ReloadGame()
        {
            engineReady = false;

            ClearEngine();

            assetManager.ReloadAssets();

            StartEngineStuff();

            BasicScreen.SetStatus(ScreenType.SplashScreen, "Finished Loading Good to Go (:");
            engineReady = true;

            BasicScreen.SetStatus();
        }

        /// <summary>
        /// Only called in the firts time window open
        /// </summary>
        private void StartGame()
        {
            engineReady = false;
            GameLoaded = false;
            assetManager.StartLoadAssets();

            StartEngineStuff();

            BasicScreen.SetStatus(ScreenType.SplashScreen, "Finished Loading Good to Go (:");
            engineReady = true;
            GameLoaded = true;
            BasicScreen.SetStatus();
        }

        /// <summary>
        /// All engine/game systems, and setups for start the game
        /// </summary>
        private void StartEngineStuff()
        {
            BasicScreen.SetStatus(ScreenType.SplashScreen, "Starting EngineSystem");
            physics = new Physics();
            soundSystem = new SoundSystem();
            tickManager = new TickManager();
            spaceManager = new SpaceManager();
            worldManager = new WorldManager();

            BasicScreen.SetStatus(ScreenType.SplashScreen, "Creating Spaces");
            SpaceManager.AddSpace("GlobalSpace", new GlobalSpace());
            SpaceManager.AddSpace("MainMenuSpace", new MainMenuSpace());
        }

        /// <summary>
        /// Used for reload all assets, only assets
        /// </summary>
        public void ReloadAssets()
        {
            throw new NotImplementedException();
        }

        public void ClearSpaces()
        {
            spaceManager.Dispose();
        }

        private void ClearEngine()
        {
            QueeSystem.CleanUp();
            ClearSpaces();

            worldManager?.Dispose();
            tickManager?.Dispose();
            physics?.Dispose();
            soundSystem?.Dispose();
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