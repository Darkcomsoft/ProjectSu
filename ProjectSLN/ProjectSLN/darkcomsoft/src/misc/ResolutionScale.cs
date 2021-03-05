using OpenTK.Mathematics;
using Projectsln.darkcomsoft.src.engine.window;
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
        private double scaledWidthD;
        private double scaledHeightD;
        private int scaledWidth;
        private int scaledHeight;
        private int scaleFactor;

        public ResolutionScale()
        {
            this.scaledWidth = WindowClass.Instance.Width;
            this.scaledHeight = WindowClass.Instance.Height;
            this.scaleFactor = 1;
            bool flag = false;
            int i = GameSettings.GuiScale;

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

        public int getScaledWidth()
        {
            return this.scaledWidth;
        }

        public int getScaledHeight()
        {
            return this.scaledHeight;
        }

        public double getScaledWidth_double()
        {
            return this.scaledWidthD;
        }

        public double getScaledHeight_double()
        {
            return this.scaledHeightD;
        }

        public int getScaleFactor()
        {
            return this.scaleFactor;
        }
    }
}
