using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.Engine.AssetsPipeline
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
