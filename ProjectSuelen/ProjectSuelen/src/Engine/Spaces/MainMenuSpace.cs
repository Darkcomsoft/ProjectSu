using ProjectSuelen.src.Engine.UI.GUIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.Engine.Spaces
{
    public class MainMenuSpace : Space
    {
        private GUIImage background;
        private GUILable darkcomsoftCopyRight;
        private GUIImage gUIImage;

        public MainMenuSpace()
        {
            background = new GUIImage(new System.Drawing.Rectangle(0, 0, 0, 0), UI.UIDock.ScreenSizeRatio);
            background.TextureName = "BackGround";

            darkcomsoftCopyRight = new GUILable("Copyright (c) 2021 Darkcomsoft - All rights reserved.", new System.Drawing.Rectangle(5,5,100,15), UI.UIDock.BottomLeft);
            darkcomsoftCopyRight.SetTextAling(UI.Font.TextAling.Left);

            gUIImage = new GUIImage(new System.Drawing.Rectangle(0, 135, 350, 50), UI.UIDock.Cennter, "VaKLogoYellow");
        }

        protected override void OnDispose()
        {
            background.Dispose();
            background = null;

            darkcomsoftCopyRight.Dispose();
            darkcomsoftCopyRight = null;

            gUIImage.Dispose();
            gUIImage = null;

            base.OnDispose();
        }
    }
}
