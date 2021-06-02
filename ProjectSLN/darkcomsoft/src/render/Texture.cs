using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using ProjectIND.darkcomsoft.src.resources.resourcestype;

namespace ProjectIND.darkcomsoft.src.render
{
    public class Texture : ClassBase
    {
        private int v_handler;
        private int v_width;
        private int v_height;

        private TextureTarget _TextureTarget = TextureTarget.Texture2D;
        private PixelInternalFormat _PixelInternalFormat = PixelInternalFormat.Rgba;
        private PixelFormat _PixelFormat = PixelFormat.Bgra;

        public Texture(ImageFile imageData, TextureUnit textureUnit = TextureUnit.Texture0)
        {
            v_width = imageData._width;
            v_height = imageData._height;

            v_handler = GL.GenTexture();

            Use(textureUnit);

            GL.TexImage2D(_TextureTarget, 0, _PixelInternalFormat, v_width, v_height, 0, _PixelFormat, PixelType.UnsignedByte, imageData._ImgData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        }

        public Texture(ImageFile imageData, TextureMinFilter textureMinFilter, TextureMagFilter textureMagFilter)
        {
            v_width = imageData._width;
            v_height = imageData._height;

            v_handler = GL.GenTexture();

            Use();

            GL.TexImage2D(_TextureTarget, 0, _PixelInternalFormat, v_width, v_height, 0, _PixelFormat, PixelType.UnsignedByte, imageData._ImgData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)textureMinFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)textureMagFilter);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        }

        public void Use(TextureUnit textunit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(textunit);
            GL.BindTexture(_TextureTarget, v_handler);
        }

        protected override void OnDispose()
        {
            GL.DeleteTexture(v_handler);
            base.OnDispose();
        }

        public int GetWidth { get { return v_width; } }
        public int GetHeight { get { return v_height; } }
    }
}
