using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.gui.guisystem.font
{
    public class FontType: ClassBase
    {
        private float aspectRatio;

        private float verticalPerPixelSize;
        private float horizontalPerPixelSize;
        private float spaceWidth;
        private int[] padding;
        private int paddingWidth;
        private int paddingHeight;

        private Dictionary<string, string> values = new Dictionary<string, string>();
        private Dictionary<int, Character> metaData = new Dictionary<int, Character>();

        private string[] lines;
        private int ReadCountLine = 0;

        public Texture AtlasTexture;

        public FontType()
        {

        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }

    public class Character
    {
        private int id;
        private float xTextureCoord;
        private float yTextureCoord;
        private float xMaxTextureCoord;
        private float yMaxTextureCoord;
        private float xOffset;
        private float yOffset;
        private float sizeX;
        private float sizeY;
        private float xAdvance;


        public Character(int id, float xTextureCoord, float yTextureCoord, float xTexSize, float yTexSize, float xOffset, float yOffset, float sizeX, float sizeY, float xAdvance)
        {
            this.id = id;
            this.xTextureCoord = xTextureCoord;
            this.yTextureCoord = yTextureCoord;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.xMaxTextureCoord = xTexSize + xTextureCoord;
            this.yMaxTextureCoord = yTexSize + yTextureCoord;
            this.xAdvance = xAdvance;
        }

        public int getId()
        {
            return id;
        }

        public float getxTextureCoord()
        {
            return xTextureCoord;
        }

        public float getyTextureCoord()
        {
            return yTextureCoord;
        }

        public float getXMaxTextureCoord()
        {
            return xMaxTextureCoord;
        }

        public float getYMaxTextureCoord()
        {
            return yMaxTextureCoord;
        }

        public float getxOffset()
        {
            return xOffset;
        }

        public float getyOffset()
        {
            return yOffset;
        }

        public float getSizeX()
        {
            return sizeX;
        }

        public float getSizeY()
        {
            return sizeY;
        }

        public float getxAdvance()
        {
            return xAdvance;
        }

    }
}
