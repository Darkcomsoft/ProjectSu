using ProjectSu.src.Engine;
using ProjectSu.src.Engine.AssetsPipeline;
using ProjectSu.src.Engine.Entitys;
using ProjectSu.src.Engine.NetCode;
using ProjectSu.src.Spaces;
using ProjectSu.src.UserUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src
{
    public class GameManager : EEntity
    {
        private bool isPlaying = false;

        public InGameGUI gameGUI;

        public GameManager()
        {
            Game.GameManager = this;
        }

        public override void Start()
        {
            Debug.Log("GameManager Started!");

            Network.OnServerStart += OnServerStart;
            Network.OnServerStop += OnServerStop;
            base.Start();
        }

        protected override void OnDestroy()
        {
            Game.GameManager = null;
            Debug.Log("GameManager Destroyed!");

            Network.OnServerStart -= OnServerStart;
            Network.OnServerStop -= OnServerStop;
            base.OnDestroy();
        }

        public void PlaySingle()
        {
            Network.CreateServer("127.0.0.1", 25000, 10);
        }

        public void Disconnect()
        {
            Network.Disconnect();
        }

        private void OnServerStart()
        {
            isPlaying = true;
            SpaceManager.RemoveSpace("MainMenuSpace");
            SpaceManager.AddSpace("ElbriumSpace", new ElbriumSpace());
            gameGUI = (InGameGUI)Instantiate("GlobalSpace", new InGameGUI());
        }
        private void OnServerStop()
        {
            isPlaying = false;
            SpaceManager.ClearSpace("GlobalSpace");
            SpaceManager.RemoveSpace("ElbriumSpace");
            gameGUI = null;

            SpaceManager.AddSpace("MainMenuSpace", new MainMenuSpace());
        }
    }
}
