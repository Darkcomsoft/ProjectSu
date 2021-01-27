using ProjectSuelen.src.Engine.UI.GUIElements;
using OpenTK.Graphics;
using ProjectSuelen.src.Engine;
using ProjectSuelen.src.Engine.UI;
using ProjectSuelen.src.Engine.UI.Font;

namespace ProjectSuelen.src.Spaces
{
    public class MainMenuSpace : Space
    {
        private GUIImage background;
        private GUILable darkcomsoftCopyRight;
        private GUIImage gUIImage;

        private GUIButtom playButtom;

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

            gUIImage = new GUIImage(new System.Drawing.Rectangle(0, 135, 350, 50), UIDock.Cennter, "VaKLogoYellow");

            playButtom = new GUIButtom("Play", new System.Drawing.Rectangle(0, 0, 200, 100), UIDock.Cennter);
            playButtom.SetBackColors(Color4.DeepSkyBlue, Color4.Gray, Color4.Red, Color4.Black);
            playButtom.OnClick += playclick;
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

            base.OnDispose();
        }

        void playclick()
        {
            Debug.Log("PlayButtonClick");
            Game.GameManager.PlaySingle();
        }
    }
}
