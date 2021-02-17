using ProjectSu;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectSu.src.Engine.Render;
using ProjectSu.src.Engine.AssetsPipeline;

namespace ProjectSu.src.Engine.UI.Font
{
    public class FontType : ClassBase
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

        public FontType(string fontFile, string fontAtlas)
        {
            AtlasTexture = new Texture(AssetManager.LoadImage(fontAtlas), TextureMinFilter.Linear, TextureMagFilter.Linear);

            aspectRatio = (float)Window.Instance.Width / (float)Window.Instance.Height;

            lines = File.ReadAllLines(fontFile);

            loadPaddingData();
            loadLineSizes();
            int imageWidth = getValueOfVariable("scaleW");
            loadCharacterData(imageWidth);
        }

        private bool NextLine()
        {
            values.Clear();

            if (ReadCountLine >= lines.Length - 1)
            {
                return false;
            }

            string[] lineSplit = lines[ReadCountLine].Split(" "[0]);
            ReadCountLine++;

            for (int i = 0; i < lineSplit.Length; i++)
            {
               string[] valuePairs = lineSplit[i].Split("="[0]);
                    
                    if (valuePairs.Length == 2)
                    {
                        values.Add(valuePairs[0], valuePairs[1]);
                    }
            }

            return true;
        }

        private void loadPaddingData()
        {
            NextLine();
            this.padding = getValuesOfVariable("padding");
            this.paddingWidth = padding[PAD_LEFT] + padding[PAD_RIGHT];
            this.paddingHeight = padding[PAD_TOP] + padding[PAD_BOTTOM];
        }

        private int[] getValuesOfVariable(string variable)
        {
            string[] numbers = values[variable].Split(","[0]);
            int[] actualValues = new int[numbers.Length];

            for (int i = 0; i < actualValues.Length; i++)
            {
                actualValues[i] = int.Parse(numbers[i]);
            }
            return actualValues;
        }

        private int getValueOfVariable(string variable)
        {
            return int.Parse(values[variable]);
        }

        private void loadLineSizes()
        {
            NextLine();
            int lineHeightPixels = getValueOfVariable("lineHeight") - paddingHeight;
            verticalPerPixelSize = LINE_HEIGHT / (float)lineHeightPixels;
            horizontalPerPixelSize = verticalPerPixelSize / aspectRatio;
        }

        private void loadCharacterData(int imageWidth)
        {
            NextLine();
            NextLine();
            while (NextLine())
            {
                Character c = loadCharacter(imageWidth);
                if (c != null)
                {
                    metaData.Add(c.getId(), c);
                }
            }
        }

        private Character loadCharacter(int imageSize)
        {
            int id = getValueOfVariable("id");
            if (id == SPACE_ASCII)
            {
                this.spaceWidth = (getValueOfVariable("xadvance") - paddingWidth) * horizontalPerPixelSize;
                return null;
            }
            float xTex = ((float)getValueOfVariable("x") + (padding[PAD_LEFT] - DESIRED_PADDING)) / imageSize;
            float yTex = ((float)getValueOfVariable("y") + (padding[PAD_TOP] - DESIRED_PADDING)) / imageSize;
            int width = getValueOfVariable("width") - (paddingWidth - (2 * DESIRED_PADDING));
            int height = getValueOfVariable("height") - ((paddingHeight) - (2 * DESIRED_PADDING));
            float quadWidth = width * horizontalPerPixelSize;
            float quadHeight = height * verticalPerPixelSize;
            float xTexSize = (float)width / imageSize;
            float yTexSize = (float)height / imageSize;
            float xOff = (getValueOfVariable("xoffset") + padding[PAD_LEFT] - DESIRED_PADDING) * horizontalPerPixelSize;
            float yOff = (getValueOfVariable("yoffset") + (padding[PAD_TOP] - DESIRED_PADDING)) * verticalPerPixelSize;
            float xAdvance = (getValueOfVariable("xadvance") - paddingWidth) * horizontalPerPixelSize;
            return new Character(id, xTex, yTex, xTexSize, yTexSize, xOff, yOff, quadWidth, quadHeight, xAdvance);
        }

        public float getSpaceWidth()
        {
            return spaceWidth;
        }

        public Character getCharacter(int ascii)
        {
            return metaData[ascii];
        }

        protected override void OnDispose()
        {
            values.Clear();
            metaData.Clear();

            AtlasTexture.Dispose();
            AtlasTexture = null;

            lines = null;
            values = null;
            metaData = null;
            padding = null;
            base.OnDispose();
        }

        private const int PAD_TOP = 0;
        private const int PAD_LEFT = 1;
        private const int PAD_BOTTOM = 2;
        private const int PAD_RIGHT = 3;

        private const int DESIRED_PADDING = 3;

        private const string Spliter = " ";
        private const string NumSpliter = ",";

        public const float LINE_HEIGHT = 0.03f;
        public const int SPACE_ASCII = 32;
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
