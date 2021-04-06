using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using Projectsln.darkcomsoft.src.resources.resourcestype;

namespace Projectsln.darkcomsoft.src.render
{
    public class Texture : ClassBase
    {
        private int m_handler;
        private int m_width;
        private int m_height;

        private TextureTarget _TextureTarget = TextureTarget.Texture2D;
        private PixelInternalFormat _PixelInternalFormat = PixelInternalFormat.Rgba;
        private PixelFormat _PixelFormat = PixelFormat.Bgra;

        public Texture(ImageFile imageData, TextureUnit textureUnit = TextureUnit.Texture0)
        {
            m_width = imageData._width;
            m_height = imageData._height;

            m_handler = GL.GenTexture();

            Use(textureUnit);

            GL.TexImage2D(_TextureTarget, 0, _PixelInternalFormat, m_width, m_height, 0, _PixelFormat, PixelType.UnsignedByte, imageData._ImgData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            //GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public Texture(ImageFile imageData, TextureMinFilter textureMinFilter, TextureMagFilter textureMagFilter)
        {
            m_width = imageData._width;
            m_height = imageData._height;

            m_handler = GL.GenTexture();

            Use();

            GL.TexImage2D(_TextureTarget, 0, _PixelInternalFormat, m_width, m_height, 0, _PixelFormat, PixelType.UnsignedByte, imageData._ImgData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)textureMinFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)textureMagFilter);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            //GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Use(TextureUnit textunit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(textunit);
            GL.BindTexture(_TextureTarget, m_handler);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }

        public int GetWidth { get { return m_width; } }
        public int GetHeight { get { return m_height; } }
    }
}
