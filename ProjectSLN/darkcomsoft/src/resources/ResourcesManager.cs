using ProjectIND.darkcomsoft.src.blocks;
using ProjectIND.darkcomsoft.src.debug;
using ProjectIND.darkcomsoft.src.enums;
using ProjectIND.darkcomsoft.src.gui.guisystem.font;
using ProjectIND.darkcomsoft.src.render;
using ProjectIND.darkcomsoft.src.resources.resourcestype;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProjectIND.darkcomsoft.src.resources
{
    public class ResourcesManager : ClassBase
    {
        private static ResourcesManager v_instance;
        private Dictionary<string, Shader> v_shaderList;
        private Dictionary<string, Texture> v_textureList;
        private Dictionary<string, Font> v_fontList;

        private Dictionary<byte, Block> v_blockDataList;

        public ResourcesManager()
        {
            v_instance = this;
            v_shaderList = new Dictionary<string, Shader>();
            v_textureList = new Dictionary<string, Texture>();
            v_fontList = new Dictionary<string, Font>();
            v_blockDataList = new Dictionary<byte, Block>();
        }

        protected override void OnDispose()
        {
            foreach (var item in v_blockDataList)
            {
                item.Value.Dispose();
            }

            foreach (var item in v_shaderList)
            {
                item.Value.Dispose();
            }

            foreach (var item in v_textureList)
            {
                item.Value.Dispose();
            }

            foreach (var item in v_fontList)
            {
                item.Value.Dispose();
            }

            v_blockDataList.Clear();
            v_textureList.Clear();
            v_shaderList.Clear();
            v_fontList.Clear();

            v_blockDataList = null;
            v_textureList = null;
            v_shaderList = null;
            v_fontList = null;
            v_instance = null;
            base.OnDispose();
        }

        /// <summary>
        /// Used for load resources for the pre systems, Like splashScreen
        /// </summary>
        public void LoadPreResources(ApplicationType apptype)
        {
            switch (apptype)
            {
                case ApplicationType.Client:
                    LoadShader("Gizmo");
                    LoadShader("UI");
                    LoadShader("Font");

                    LoadFont("PixelFont2");
                    break;
                case ApplicationType.Server:

                    break;
            }
        }

        /// <summary>
        /// This is used for load the rest of the game, if you want to load for pre-system use <see cref="LoadPreResources(ApplicationType)"/>
        /// </summary>
        public void LoadResources()
        {
            switch (Application.AppType)
            {
                case ApplicationType.Client:

                    LoadBlocks();
                    break;
                case ApplicationType.Server:

                    break;
            }
        }

        public static Shader GetShader(string ShaderName)
        {
            if (instance.v_shaderList.TryGetValue(ShaderName, out Shader shader))
            {
                return shader;
            }

            return null;
        } 

        public static Texture GetTexture(string TextureName)
        {
            if (instance.v_textureList.TryGetValue(TextureName, out Texture texture))
            {
                return texture;
            }
            return null;
        }

        public static Font GetFont(string FontName)
        {
            if (instance.v_fontList.TryGetValue(FontName, out Font font))
            {
                return font;
            }
            return null;
        }

        public static Block GetBlock(byte id)
        {
            if (instance.v_blockDataList.TryGetValue(id, out Block block))
            {
                return block;
            }

            printError("Don't exist this block: " + id);
            return null;
        }

        #region LoadFunctions
        private void LoadShader(string ShaderName)
        {
            try
            {
                ShaderFile shaderFile = GLSLLoad.LoadShaderFile("/Shaders/", ShaderName);
                Shader shader = new Shader(shaderFile);
                if (v_shaderList.ContainsKey(ShaderName)) { return; }
                v_shaderList.Add(ShaderName, shader);
                Debug.Log("Shader Loaded! : " + ShaderName, "RESOURCES-MANAGER");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void LoadTexture(string TextureName)
        {
            try
            {
                ImageFile imageFile = ImageFile.LoadImage("/Texture/", TextureName);
                Texture texture = new Texture(imageFile);
                if (v_textureList.ContainsKey(TextureName)) { return; }
                v_textureList.Add(TextureName,texture);
                Debug.Log("Texture Loaded! : " + TextureName, "RESOURCES-MANAGER");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void LoadFont(string FontName)
        {
            try
            {
                if (v_fontList.ContainsKey(FontName)) { return; }
                v_fontList.Add(FontName, new Font(FontName));
                Debug.Log("Font Loaded! : " + FontName, "RESOURCES-MANAGER");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region LoadBlocks
        private void LoadBlocks()
        {
            v_blockDataList.Add(0, new AirBlock());
            v_blockDataList.Add(1, new DirtBlock());
        }
        #endregion

        public static ResourcesManager instance { get { return v_instance; } }
    }
}
