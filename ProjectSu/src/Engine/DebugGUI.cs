using OpenTK.Graphics;
using OpenTK.Graphics.ES20;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ProjectSu.src.Engine.UI;
using ProjectSu.src.Engine.UI.Font;
using ProjectSu.src.Engine.UI.GUIElements;
using ProjectSu.src.Entitys;
using ProjectSu.src.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.Engine
{
    public class DebugGUI : ClassBase
    {
        private bool showing = false;

        private GUILable gameInfoLable;
        private GUILable debugText;

        private GUILable PlayerPosition;
        private GUILable BlockInfo;

        public DebugGUI()
        {
            gameInfoLable = new GUILable(Application.AppName + " " + Application.Version +" | "+ string.Format("OS:({0})  OpenGLVersion:({1})  GLSLVersion:({2})  Vendor:({3})", System.Environment.OSVersion.ToString(), GL.GetString(StringName.Version), GL.GetString(StringName.ShadingLanguageVersion), GL.GetString(StringName.Vendor)), new System.Drawing.Rectangle(5, 5, 200, 20), UIDock.TopLeft);
            gameInfoLable.SetColor(Color4.White);
            gameInfoLable.SetTextAling(TextAling.Left);
            gameInfoLable.ShowBackGround = false;

            debugText = new GUILable("NULL VALUES", new System.Drawing.Rectangle(5, 25, 200, 20), UIDock.TopLeft);
            debugText.SetColor(Color4.White);
            debugText.SetTextAling(TextAling.Left);
            debugText.ShowBackGround = false;

            PlayerPosition = new GUILable("No Player Data", new System.Drawing.Rectangle(5, 45, 200, 20), UIDock.TopLeft);
            PlayerPosition.SetColor(Color4.White);
            PlayerPosition.SetTextAling(TextAling.Left);
            PlayerPosition.ShowBackGround = false;

            BlockInfo = new GUILable("No Player Data", new System.Drawing.Rectangle(5, 65, 200, 20), UIDock.TopLeft);
            BlockInfo.SetColor(Color4.White);
            BlockInfo.SetTextAling(TextAling.Left);
            BlockInfo.ShowBackGround = false;

            /*debugText = new GUILable("NULL VALUES", new System.Drawing.Rectangle(5, 30, 200, 20), UIDock.TopRight);
            debugText.SetColor(Color4.White);
            debugText.SetTextAling(TextAling.Right);
            debugText.ShowBackGround = false;*/
            GL.GetString(StringName.Version);

        }

        public void Tick()
        {
            if (Input.GetKeyDown(Keys.F4, 0))
            {
                if (showing)
                {
                    showing = false;
                    HideDebug();
                }
                else
                {
                    showing = true;
                    ShowDebug();
                }
            }

            if (PlayerEntity.my != null)
            {
                PlayerPosition.SetText(string.Format("Position({0})", Transform.PlayerRootPosition));
                BlockInfo.SetText(string.Format("Block:{0}", WorldManager.GetWorld(PlayerEntity.my.SpaceName)?.GetTileAt(Transform.PlayerRootPosition.X, Transform.PlayerRootPosition.Z).ToString()));
            }
            debugText.SetText(string.Format("FPS:({0})  UPS:({1})  Tick%60:({2})  Time:({3})", Time.FPS, Time.UPS, Time._Tick, Time._Time));
        }

        private void ShowDebug()
        {
            gameInfoLable.Enable();
            debugText.Enable();
            PlayerPosition.Enable();
            BlockInfo.Enable();
        }

        private void HideDebug()
        {
            gameInfoLable.Disable();
            debugText.Disable();
            PlayerPosition.Disable();
            BlockInfo.Disable();
        }

        protected override void OnDispose()
        {
            gameInfoLable?.Dispose();
            gameInfoLable = null;

            debugText?.Dispose();
            debugText = null;

            PlayerPosition?.Dispose();
            PlayerPosition = null;

            BlockInfo?.Dispose();
            BlockInfo = null;
            base.OnDispose();
        }
    }
}
