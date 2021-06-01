using ProjectSLN.darkcomsoft.src.debug;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace ProjectSLN.darkcomsoft.src.resources.resourcestype
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

        public static ImageFile LoadImage(string path, string filename)
        {
            try
            {
                string filePath = string.Concat(Application.AssetsPath, path, filename, ".png");

                if (!File.Exists(filePath))
                {
                    Debug.LogError("Texture Files Can't be found At: " + filePath);
                    throw new Exception("Texture Files Can't be found At: " + filePath);
                }

                using (var image = new Bitmap(filePath))
                {
                    image.RotateFlip(RotateFlipType.RotateNoneFlipY);

                    var data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);

                    return new ImageFile(data.Scan0, data.Width, data.Height);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static ImageFile FontLoadImage(string path, string filename)
        {
            try
            {
                string filePath = string.Concat(Application.AssetsPath, path, filename, ".png");

                if (!File.Exists(filePath))
                {
                    Debug.LogError("Texture Files Can't be found At: " + filePath);
                    throw new Exception("Texture Files Can't be found At: " + filePath);
                }

                using (var image = new Bitmap(filePath))
                {
                    //image.RotateFlip(RotateFlipType.RotateNoneFlipY);

                    var data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);

                    return new ImageFile(data.Scan0, data.Width, data.Height);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
