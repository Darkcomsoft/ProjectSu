using OpenTK.Graphics;
using ProjectSuelen.src.Engine.Entitys;
using ProjectSuelen.src.Engine.UI;
using ProjectSuelen.src.Engine.UI.GUIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.UserUI
{
    public class InGameGUI :EEntity
    {
        private GUIButtom playButtom;

        public InGameGUI()
        {
            playButtom = new GUIButtom("Disconnect", new System.Drawing.Rectangle(0, 0, 200, 100), UIDock.Cennter);
            playButtom.SetBackColors(Color4.DeepSkyBlue, Color4.Gray, Color4.Red, Color4.Black);
            playButtom.OnClick += disconnectOnClick;
        }

        protected override void OnDestroy()
        {
            playButtom.OnClick -= disconnectOnClick;

            playButtom.Dispose();
            playButtom = null;
            base.OnDestroy();
        }

        private void disconnectOnClick()
        {
            Game.GameManager.Disconnect();
        }
    }
}
