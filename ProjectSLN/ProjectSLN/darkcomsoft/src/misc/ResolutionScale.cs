using OpenTK.Mathematics;
using Projectsln.darkcomsoft.src.debug.window;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.misc
{
    /// <summary>
    /// Arquivo retirado do minecraft, subistituir por um meu
    /// </summary>
    public class ResolutionScale
    {
        public static ResolutionScale instance;

        public double scaledWidthD;
        public double scaledHeightD;
        public int scaledWidth;
        public int scaledHeight;
        public int scaleFactor;

        public ResolutionScale()
        {
            instance = this;

            this.scaledWidth = WindowMain.Instance.Width;
            this.scaledHeight = WindowMain.Instance.Height;
            this.scaleFactor = 1;
            bool flag = false;
            int i = (int)GameSettings.GuiScale;

            if (i == 0)
            {
                i = 1000;
            }

            while (this.scaleFactor < i && this.scaledWidth / (this.scaleFactor + 1) >= 320 && this.scaledHeight / (this.scaleFactor + 1) >= 240)
            {
                ++this.scaleFactor;
            }

            if (flag && this.scaleFactor % 2 != 0 && this.scaleFactor != 1)
            {
                --this.scaleFactor;
            }

            this.scaledWidthD = (double)this.scaledWidth / (double)this.scaleFactor;
            this.scaledHeightD = (double)this.scaledHeight / (double)this.scaleFactor;
            this.scaledWidth = (int)MathHelper.Ceiling(this.scaledWidthD);
            this.scaledHeight = (int)MathHelper.Ceiling(this.scaledHeightD);
        }

        public void OnResize()
        {
            this.scaledWidth = WindowMain.Instance.Width;
            this.scaledHeight = WindowMain.Instance.Height;
            this.scaleFactor = 1;
            bool flag = true;
            int i = 0;

            if (i == 0)
            {
                i = 1000;
            }

            while (this.scaleFactor < i && this.scaledWidth / (this.scaleFactor + 1) >= 320 && this.scaledHeight / (this.scaleFactor + 1) >= 240)
            {
                ++this.scaleFactor;
            }

            if (flag && this.scaleFactor % 2 != 0 && this.scaleFactor != 1)
            {
                --this.scaleFactor;
            }

            this.scaledWidthD = (double)this.scaledWidth / (double)this.scaleFactor;
            this.scaledHeightD = (double)this.scaledHeight / (double)this.scaleFactor;
            this.scaledWidth = (int)MathHelper.Ceiling(this.scaledWidthD);
            this.scaledHeight = (int)MathHelper.Ceiling(this.scaledHeightD);
        }
    }
}
