﻿using OpenTK;
using OpenTK.Graphics;
using ProjectSuelen.src.Engine.Render;
using ProjectSuelen.src.Engine.Sound;
using ProjectSuelen.src.Engine.UI.Font;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.Engine.AssetsPipeline
{
    public class AssetManager : ObjectBase
    {
        private static AssetManager thisinstance;
        private static bool assetsReady = false;
        private Dictionary<string, Shader> _shaderList;
        private Dictionary<string, Texture> _textureList;
        private Dictionary<string, Model> _Models;
        private Dictionary<string, FontType> _FontList;
        private Dictionary<string, AudioClip> _soundList;

        private string AssetsPath = "NULL_PATH";

        public AssetManager()
        {
            thisinstance = this;

            AssetsPath = Application.AssetsPath;

            Debug.Log("Assets Folder Path: " + AssetsPath);

            _shaderList = new Dictionary<string, Shader>();
            _textureList = new Dictionary<string, Texture>();
            _Models = new Dictionary<string, Model>();
            _FontList = new Dictionary<string, FontType>();
            _soundList = new Dictionary<string, AudioClip>();

            LoadPreAssets();
        }

        protected override void OnDispose()
        {
            UnloadAssets();
            thisinstance = null;
            base.OnDispose();
        }

        private void LoadPreAssets()
        {
            _textureList.Add("Darkcomsoft", new Texture(LoadImage("/Images/", "Darkcomsoft", "png")));

            _shaderList.Add("Font", new Shader(LoadShader("/Shaders/", "Font")));
            _shaderList.Add("GUI", new Shader(LoadShader("/Shaders/", "GUI")));

            _FontList.Add("PixelFont", LoadFont("/UI/Fonts/", "PixelFont"));
            _FontList.Add("PixelFont2", LoadFont("/UI/Fonts/", "PixelFont2"));
        }

        private void LoadAssets()
        {
            BasicScreen.SetStatus(ScreenType.SplashScreen, "Loading Assests");
            //Assets aqui
            BasicScreen.SetStatus(ScreenType.SplashScreen, "Loading Images");
            _textureList.Add("VaKLogoYellow", new Texture(LoadImage("/Images/", "VaKLogoYellow", "png")));
            _textureList.Add("BackGround", new Texture(LoadImage("/Images/", "BackGround", "png")));
            
            //
                        assetsReady = true;
            BasicScreen.SetStatus();
        }

        private void UnloadAssets()
        {
            assetsReady = false;

            foreach (var item in _shaderList)
            {
                item.Value.Dispose();
            }

            foreach (var item in _textureList)
            {
                item.Value.Dispose();
            }

            foreach (var item in _Models)
            {
                item.Value.Dispose();
            }

            foreach (var item in _FontList)
            {
                item.Value.Dispose();
            }

            foreach (var item in _soundList)
            {
                item.Value.Dispose();
            }

            _shaderList.Clear();
            _textureList.Clear();
            _Models.Clear();
            _FontList.Clear();
            _soundList.Clear();

            _shaderList = null;
            _textureList = null;
            _Models = null;
            _FontList = null;
            _soundList = null;
        }

        public void ReloadAssets()
        {
            BasicScreen.SetStatus(ScreenType.SplashScreen, "Reloading Assests");
            UnloadAssets();
            LoadPreAssets();
            LoadAssets();
        }
        public void StartLoadAssets()
        {
            LoadAssets();
        }

        #region GetAssets
        public static int GetSound(string soundName)
        {
            if (AssetManager.instance._soundList.TryGetValue(soundName, out AudioClip clip))
            {
                return clip.GetHandler();
            }
            else
            {
                return 0;
            }
        }
        public static Texture GetTexture(string TextureName)
        {
            if (AssetManager.instance._textureList.TryGetValue(TextureName, out Texture texture))
            {
                return texture;
            }
            else
            {
                throw new Exception("Dont Exist this Assets: " + TextureName);
            }
        }
        /*public static CubeMapTexture GetCubeMap(string TextureName)
        {
            if (AssetManager.instance._CubeTextures.TryGetValue(TextureName, out CubeMapTexture texture))
            {
                return texture;
            }
            else
            {
                throw new Exception("Dont Exist this Assets: " + TextureName);
            }
        }*/
        public static Model GetMesh(string MeshName)
        {
            if (AssetManager.instance._Models.TryGetValue(MeshName, out Model _mesh))
            {
                return _mesh;
            }
            else
            {
                throw new Exception("Dont Exist this Assets: " + MeshName);
            }
        }
        public static Shader GetShader(string ShaderName)
        {
            if (AssetManager.instance._shaderList.TryGetValue(ShaderName, out Shader texture))
            {
                return texture;
            }
            else
            {
                throw new Exception("Dont Exist this Assets: " + ShaderName);
            }
        }
        public static FontType GetFont(string FontName)
        {
            if (AssetManager.instance._FontList.TryGetValue(FontName, out FontType font))
            {
                return font;
            }
            else
            {
                throw new Exception("Dont Exist this Assets: " + FontName);
            }
        }
        #endregion

        #region UseAssetsFunctions
        public static void UseTexture(string TextureName)
        {
            if (AssetManager.instance._textureList.TryGetValue(TextureName, out Texture texture))
            {
                texture.Use();
            }
        }

        public static void UseShader(string ShaderName)
        {
            if (AssetManager.instance._shaderList.TryGetValue(ShaderName, out Shader shader))
            {
                shader.Use();
            }
        }

        public static void ShaderSet(string ShaderName, string name, Matrix4 value)
        {
            if (AssetManager.instance._shaderList.TryGetValue(ShaderName, out Shader shader))
            {
                shader.SetMatrix4(name, value);
            }
        }

        public static void ShaderSet(string ShaderName, string name, int value)
        {
            if (AssetManager.instance._shaderList.TryGetValue(ShaderName, out Shader shader))
            {
                shader.SetInt(name, value);
            }
        }

        public static void ShaderSet(string ShaderName, string name, float value)
        {
            if (AssetManager.instance._shaderList.TryGetValue(ShaderName, out Shader shader))
            {
                shader.SetFloat(name, value);
            }
        }

        public static void ShaderSet(string ShaderName, string name, bool value)
        {
            if (AssetManager.instance._shaderList.TryGetValue(ShaderName, out Shader shader))
            {
                shader.Setbool(name, value);
            }
        }

        public static void ShaderSet(string ShaderName, string name, Vector3 value)
        {
            if (AssetManager.instance._shaderList.TryGetValue(ShaderName, out Shader shader))
            {
                shader.SetVector3(name, value);
            }
        }

        public static void ShaderSet(string ShaderName, string name, Vector4 value)
        {
            if (AssetManager.instance._shaderList.TryGetValue(ShaderName, out Shader shader))
            {
                shader.SetVector4(name, value);
            }
        }

        public static void ShaderSet(string ShaderName, string name, Color4 value)
        {
            if (AssetManager.instance._shaderList.TryGetValue(ShaderName, out Shader shader))
            {
                shader.SetColor(name, value);
            }
        }
        #endregion

        #region LoadAssetsSystem
        public static ShaderFile LoadShader(string path, string file)
        {
            string vertshader = string.Concat(Application.AssetsPath,path, file, ".vert");
            string fragshader = string.Concat(Application.AssetsPath,path, file, ".frag");

            if (!File.Exists(vertshader) || !File.Exists(fragshader))
            {
                Debug.LogError("Shader Files Can't be found!");
                throw new Exception("Shader Files Can't be found!");
            }

            return new ShaderFile(File.ReadAllText(vertshader), File.ReadAllText(fragshader));
        }
        public static ImageFile LoadImage(string path, string file, string extensio)
        {
            if (!File.Exists(string.Concat(Application.AssetsPath, path, file, "." + extensio)))
            {
                Debug.LogError("Texture Files Can't be found At: " + string.Concat(Application.AssetsPath, path, file, "." + extensio));
                throw new Exception("Texture Files Can't be found At: " + string.Concat(Application.AssetsPath, path, file, "." + extensio));
            }

            using (var image = new Bitmap(string.Concat(Application.AssetsPath, path, file, "." + extensio)))
            {
                image.RotateFlip(RotateFlipType.RotateNoneFlipY);

                //image.MakeTransparent();
                var data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                return new ImageFile(data.Scan0, data.Width, data.Height);
            }
        }
        public static ImageFile[] LoadCubeImages(string path, string file, string extensio)
        {
            List<ImageFile> files = new List<ImageFile>();

            string finalPath = Application.AssetsPath + path + file + "/";

            for (int i = 0; i < 6; i++)
            {
                string finalFileName = file;
                RotateFlipType rotate = RotateFlipType.RotateNoneFlipNone;
                if (i == 0)
                {
                    finalFileName = file + "_rt";
                }
                else if (i == 1)
                {
                    finalFileName = file + "_dn";
                }
                else if (i == 2)
                {
                    finalFileName = file + "_bk";
                }
                else if (i == 3)
                {
                    finalFileName = file + "_lf";
                }
                else if (i == 4)
                {
                    finalFileName = file + "_up";
                    rotate = RotateFlipType.Rotate90FlipNone;
                }
                else if (i == 5)
                {
                    finalFileName = file + "_ft";
                }

                if (!File.Exists(string.Concat(finalPath, finalFileName, "." + extensio)))
                {
                    Debug.LogError("Texture Files Can't be found At: " + string.Concat(finalPath, finalFileName, "." + extensio));
                    throw new Exception("Texture Files Can't be found At: " + string.Concat(finalPath, finalFileName, "." + extensio));
                }

                using (var image = new Bitmap(string.Concat(finalPath, finalFileName, "." + extensio)))
                {
                    image.RotateFlip(rotate);

                    //image.MakeTransparent();
                    var data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                    files.Add(new ImageFile(data.Scan0, data.Width, data.Height));
                }
            }

            return files.ToArray();
        }
        public FontType LoadFont(string path, string file)
        {
            string fontFile = string.Concat(Application.AssetsPath,path, file, ".fnt");
            string fontAtlas = string.Concat(Application.AssetsPath, path, file, ".png");

            if (!File.Exists(fontFile) || !File.Exists(fontAtlas))
            {
                Debug.LogError("Shader Files Can't be found!");
                throw new Exception("Shader Files Can't be found!");
            }

            return new FontType(fontFile, fontAtlas);
        }
        public static ImageFile LoadImage(string path)
        {
            using (var image = new Bitmap(path))
            {
                //image.RotateFlip(RotateFlipType.RotateNoneFlipY);

                //image.MakeTransparent();
                var data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                return new ImageFile(data.Scan0, data.Width, data.Height);
            }
        }
        /*public static Mesh LoadModel(string path, string file)
{
    if (!File.Exists(string.Concat(path, file, ".dae")))
    {
        Debug.LogError("Model(COLLADA) Files Can't be found At: " + string.Concat(path, file, ".dae"));
        throw new Exception("Model(COLLADA) Files Can't be found At: " + string.Concat(path, file, ".dae"));
    }

    ColladaProcessor processor = new ColladaProcessor(string.Concat(path, file, ".dae"));

    return processor.Load();
}*/
        #endregion

        public static AssetManager instance { get => thisinstance; private set { } }
        public static bool AssetsReady { get => assetsReady; private set { } }
    }
}
