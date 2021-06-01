using ProjectSLN.darkcomsoft.src.render;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ProjectSLN.darkcomsoft.src.engine.window;
using OpenTK.Graphics.OpenGL;
using ProjectSLN.darkcomsoft.src.resources;
using ProjectSLN.darkcomsoft.src.resources.resourcestype;

namespace ProjectSLN.darkcomsoft.src.gui.guisystem.font
{
    public class Font: ClassBase
    {
        private string m_fontName;

        private float aspectRatio;

        private float verticalPerPixelSize;
        private float horizontalPerPixelSize;
        private float spaceWidth;
        private int[] padding;
        private int paddingWidth;
        private int paddingHeight;

        private string[] lines;
        private int ReadCountLine = 0;

        private const int PAD_TOP = 0;
        private const int PAD_LEFT = 1;
        private const int PAD_BOTTOM = 2;
        private const int PAD_RIGHT = 3;

        private const int DESIRED_PADDING = 3;

        private const string Spliter = " ";
        private const string NumSpliter = ",";

        public const float LINE_HEIGHT = 0.03f;
        public const int SPACE_ASCII = 32;

        private Dictionary<string, string> values = new Dictionary<string, string>();
        private Dictionary<int, Character> metaData = new Dictionary<int, Character>();

        public Texture AtlasTexture;

        public Font(string FontName)
        {
            string fontFilePath = string.Concat(Application.AssetsPath, "/Font/", FontName, ".fnt");

            m_fontName = FontName;
            aspectRatio = (float)WindowMain.Instance.Width / (float)WindowMain.Instance.Height;

            AtlasTexture = new Texture(ImageFile.FontLoadImage("/Font/", FontName), TextureMinFilter.Nearest, TextureMagFilter.Nearest);
            lines = File.ReadAllLines(fontFilePath);

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
                if (!c.Equals(Character.Null))
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
                return new Character();
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
            if (metaData.ContainsKey(ascii))
            {
                return metaData[ascii];
            }
            else
            {
                if (metaData.ContainsKey(63))
                {
                    return metaData[63];
                }
                else
                {
                    throw new Exception("Don't Found this Character: (63) on this Font:("+ m_fontName + ")");
                }
            }
        }

        protected override void OnDispose()
        {
            values.Clear();
            metaData.Clear();
            AtlasTexture.Dispose();

            padding = null;
            lines = null;
            metaData = null;
            values = null;
            AtlasTexture = null;
            base.OnDispose();
        }
    }

    public struct Character
    {
        public static Character Null { get { return new Character(); } }

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
