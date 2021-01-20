using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using ProjectSuelen.src.Engine.AssetsPipeline;
using ProjectSuelen.src.Engine.PhysicsSystem;
using ProjectSuelen.src.Engine.Spaces;
using ProjectSuelen.src.Engine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.Engine
{
    /// <summary>
    /// All engine logic, is like the Window Class but just for the systems
    /// </summary>
    public class EngineMain : ObjectBase
    {
        private static bool engineReady = false;

        private BasicSystem basicSystem;

        private AssetManager assetManager;
        private Physics physics;
        private SpaceManager spaceManager;

        public EngineMain()
        {
            assetManager = new AssetManager();

            basicSystem = new BasicSystem();

            StartGame();
        }

        public void Tick()
        {
            if (AssetManager.AssetsReady && engineReady)
            {
                
            }
        }

        public void TickRender()
        {
            if (AssetManager.AssetsReady && engineReady)
            {
                GL.Enable(EnableCap.DepthTest);
                //3d draw
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
            spaceManager = new SpaceManager();

            BasicScreen.SetStatus(ScreenType.SplashScreen, "Creating Spaces");
            SpaceManager.AddSpace("GlobalSpace", new GlobalSpace());
            SpaceManager.AddSpace("MainMenu", new MainMenuSpace());

            BasicScreen.SetStatus(ScreenType.SplashScreen, "Finished Loading Good to Go (:");
            engineReady = true;

            BasicScreen.SetStatus();
        }

        public void StartGame()
        {
            engineReady = false;

            assetManager.StartLoadAssets();

            BasicScreen.SetStatus(ScreenType.SplashScreen, "Starting EngineSystem");
            physics = new Physics();
            spaceManager = new SpaceManager();

            BasicScreen.SetStatus(ScreenType.SplashScreen, "Creating Spaces");
            SpaceManager.AddSpace("GlobalSpace", new GlobalSpace());
            SpaceManager.AddSpace("MainMenu", new MainMenuSpace());

            BasicScreen.SetStatus(ScreenType.SplashScreen, "Finished Loading Good to Go (:");
            engineReady = true;
            
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

            if (physics != null)
            {
                physics.Dispose();
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
        }
        #endregion

        public static bool EngineReady { get => engineReady; private set { } }
    }
}
