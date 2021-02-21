using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using ProjectSu.src.Engine;
using ProjectSu.src.Engine.AssetsPipeline;
using ProjectSu.src.Engine.UI;
using ProjectSu.src.Engine.UI.Font;
using ProjectSu.src.Engine.UI.GUIElements;
using ProjectSu.src.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.Entitys
{
    public class PlayerEntity : Entity
    {
        public static PlayerEntity my;
        private PlayerController _playerController;

        public PlayerEntity()
        {
            
        }

        protected override void NetStart()
        {
#if Client
            if (isMine)
            {
                my = this;

                transform.Position = new Vector3d(0, 30, 0);

                _playerController = new PlayerController(this, 0.5f);//Start the player controller, only if you own this entity(if you spawened this)
            }
#elif Server

#endif
            base.NetStart();
        }

        protected override void OnTick()
        {
#if Client
            
            if (isMine)//if this is my
            {
                Transform.PlayerRootPosition = transform.Position;
                WorldManager.GetWorld(SpaceName).PlayerPos = transform.Position;

                if (_playerController != null)
                {
                    _playerController.UpdateController();
                }

                //RPC("RPC_SyncPosition", ProjectSu.src.Net.RPCMode.AllNoOwner, transform.Position, transform.Rotation);
            }
            else
            {

            }
#elif Server

#endif
            base.OnTick();
        }

        protected override void OnDestroy()
        {
#if Client
            if (my == this)
            {
                my = null;
            }

            if (_playerController != null)
            {
                _playerController.Dispose();
                _playerController = null;
            }
#endif
            base.OnDestroy();
        }
    }
}
