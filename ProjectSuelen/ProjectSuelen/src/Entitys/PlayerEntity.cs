using OpenTK;
using OpenTK.Graphics;
using ProjectSuelen.src.Engine;
using ProjectSuelen.src.Engine.UI;
using ProjectSuelen.src.Engine.UI.Font;
using ProjectSuelen.src.Engine.UI.GUIElements;
using ProjectSuelen.src.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.Entitys
{
    public class PlayerEntity : Entity
    {
        private PlayerController _playerController;

        private GUILable PlayerPosition;
        private GUILable BlockInfo;

        public PlayerEntity()
        {
            
        }

        protected override void NetStart()
        {
            transform.Position = new Vector3d(0, 30, 0);
            
#if Client
            if (isMine)
            {
                PlayerPosition = new GUILable("No Player Data", new System.Drawing.Rectangle(5, 50, 200, 20), UIDock.TopLeft);
                PlayerPosition.SetColor(Color4.White);
                PlayerPosition.SetTextAling(TextAling.Left);
                PlayerPosition.ShowBackGround = false;

                BlockInfo = new GUILable("No Player Data", new System.Drawing.Rectangle(5, 70, 200, 20), UIDock.TopLeft);
                BlockInfo.SetColor(Color4.White);
                BlockInfo.SetTextAling(TextAling.Left);
                BlockInfo.ShowBackGround = false;

                _playerController = new PlayerController(this, 0.5f);//Start the player controller, only if you own this entity(if you spawened this)
            }
#elif Server

#endif
            base.NetStart();
        }

        protected override void OnTick()
        {
#if Client
            Transform.PlayerRootPosition = transform.Position;
            WorldManager.GetWorld(SpaceName).PlayerPos = transform.Position;
            if (_playerController != null)
            {
                _playerController.UpdateController();
            }

            if (isMine)//if this is my
            {
                //RPC("RPC_SyncPosition", ProjectSuelen.src.Net.RPCMode.AllNoOwner, transform.Position, transform.Rotation);

                //CurrentBlock = Game.GetWorld.GetTileAt(transform.Position.X, transform.Position.Z);
                PlayerPosition.SetText(string.Format("Position({0}) Memory({1})", transform.Position.ToString(), ConvertBytesToMegabytes(1024)));
                BlockInfo.SetText(string.Format("Block:{0}", "NULL"));
            }
            else
            {

            }
#elif Server

#endif
            base.OnTick();
        }
        static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
        protected override void OnDestroy()
        {
#if Client
            if (PlayerPosition != null)
            {
                PlayerPosition.Dispose();
                PlayerPosition = null;
            }

            if (BlockInfo != null)
            {
                BlockInfo.Dispose();
                BlockInfo = null;
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
