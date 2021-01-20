using ProjectSuelen.src.Engine.UI;
using ProjectSuelen.src.Engine.UI.GUIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectSuelen.src.Engine
{
    public class BasicScreen : ObjectBase
    {
        private static BasicScreen instanceThis;

        private GUIImage darkcomsoftLogo;
        private GUILable loadingInfoText;

        public BasicScreen()
        {
            instanceThis = this;

            darkcomsoftLogo = new GUIImage(new System.Drawing.Rectangle(0, 0, 500, 350), UIDock.Cennter, "Darkcomsoft");
            darkcomsoftLogo.Disable();

            loadingInfoText = new GUILable("NULL", new System.Drawing.Rectangle(0, 15, 250, 250), UIDock.CenterBottom);
            loadingInfoText.Disable();
        }

        private void EnableSplash(string msg)
        {
            instance.darkcomsoftLogo.Enable();
            instance.loadingInfoText.Enable();

            instance.loadingInfoText.SetText(msg);
        }

        private void DisbaleSplash()
        {
            instance.darkcomsoftLogo.Disable();
            instance.loadingInfoText.Disable();
        }

        private void EnableLoading(string msg)
        {
            instance.loadingInfoText.Enable();

            instance.loadingInfoText.SetText("Loading: " + msg);
        }

        private void DisbaleLoading()
        {
            instance.loadingInfoText.Disable();
        }

        public static void SetStatus(ScreenType screenType, string status)
        {
            if (instance != null)
            {
                switch (screenType)
                {
                    case ScreenType.none:
                        instance.DisbaleLoading();
                        instance.DisbaleSplash();
                        break;
                    case ScreenType.SplashScreen:
                        instance.EnableSplash(status);
                        break;
                    case ScreenType.LoadingScreen:
                        instance.EnableLoading(status);
                        break;
                }
            }
        }

        public static void SetStatus()
        {
            if (instance != null)
            {
                instance.DisbaleLoading();
                instance.DisbaleSplash();
            }
        }

        protected override void OnDispose()
        {
            darkcomsoftLogo.Dispose();
            loadingInfoText.Dispose();

            darkcomsoftLogo = null;
            loadingInfoText = null;
            instanceThis = null;

            base.OnDispose();
        }

        public static BasicScreen instance { get { return instanceThis; } private set { } }
    }

    public enum ScreenType : byte
    {
        none, SplashScreen, LoadingScreen
    }
}
