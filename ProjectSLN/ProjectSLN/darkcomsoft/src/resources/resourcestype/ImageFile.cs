using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.resources.resourcestype
{
    public struct ImageFile
    {
        public int _width;
        public int _height;
        public IntPtr _ImgData;

        public ImageFile(IntPtr scan0, int width, int height) : this()
        {
            this._ImgData = scan0;
            this._width = width;
            this._height = height;
        }
    }
}
