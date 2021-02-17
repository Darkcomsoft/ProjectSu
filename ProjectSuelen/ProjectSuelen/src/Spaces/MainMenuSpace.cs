using ProjectSu.src.Engine.UI.GUIElements;
using OpenTK.Graphics;
using ProjectSu.src.Engine;
using ProjectSu.src.Engine.UI;
using ProjectSu.src.Engine.UI.Font;

namespace ProjectSu.src.Spaces
{
    public class MainMenuSpace : Space
    {
        private GUIImage background;
        private GUILable darkcomsoftCopyRight;
        private GUIImage gUIImage;

        private GUIButtom playButtom;
        private GUIButtom quitButtom;

        public MainMenuSpace()
        {
           
        }

        public override void OnSpaceStart()
        {
            GameObject.Instantiate("GlobalSpace", new GameManager());

            background = new GUIImage(new System.Drawing.Rectangle(0, 0, 0, 0), UIDock.ScreenSizeRatio);
            background.TextureName = "BackGround";

            darkcomsoftCopyRight = new GUILable("Copyright (c) 2021 Darkcomsoft - All rights reserved.", new System.Drawing.Rectangle(5, 5, 100, 15), UIDock.BottomLeft);
            darkcomsoftCopyRight.SetTextAling(TextAling.Left);

            gUIImage = new GUIImage(new System.Drawing.Rectangle(0, 100, 350, 50), UIDock.Cennter, "VaKLogoYellow");

            playButtom = new GUIButtom("Play", new System.Drawing.Rectangle(0, 0, 200, 50), UIDock.Cennter);
            playButtom.SetBackColors(Color4.DeepSkyBlue, Color4.Gray, Color4.Red, Color4.Black);
            playButtom.OnClick += playclick;

            quitButtom = new GUIButtom("Quit", new System.Drawing.Rectangle(0, -55, 200, 50), UIDock.Cennter);
            quitButtom.SetBackColors(Color4.DeepSkyBlue, Color4.Gray, Color4.Red, Color4.Black);
            quitButtom.OnClick += quitclick;
            base.OnSpaceStart();
        }

        protected override void OnDispose()
        {
            background.Dispose();
            background = null;

            darkcomsoftCopyRight.Dispose();
            darkcomsoftCopyRight = null;

            gUIImage.Dispose();
            gUIImage = null;

            playButtom.OnClick -= playclick;

            playButtom.Dispose();
            playButtom = null;

            quitButtom.OnClick -= quitclick;

            quitButtom.Dispose();
            quitButtom = null;

            base.OnDispose();
        }

        void playclick()
        {
            Debug.Log("PlayButtonClick");
            Game.GameManager.PlaySingle();
        }

        void quitclick()
        {
            Window.Instance.Exit();
        }
    }
}
