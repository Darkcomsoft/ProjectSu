using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.enums;
using Projectsln.darkcomsoft.src.gui.guisystem.font;
using Projectsln.darkcomsoft.src.render;
using Projectsln.darkcomsoft.src.resources.resourcestype;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Projectsln.darkcomsoft.src.resources
{
    public class ResourcesManager : ClassBase
    {
        private static ResourcesManager m_instance;
        private Dictionary<string, Shader> m_shaderList;
        private Dictionary<string, Texture> m_textureList;
        private Dictionary<string, FontType> m_fontList;

        public ResourcesManager()
        {
            m_instance = this;
            m_shaderList = new Dictionary<string, Shader>();
            m_textureList = new Dictionary<string, Texture>();
            m_fontList = new Dictionary<string, FontType>();
        }

        protected override void OnDispose()
        {
            foreach (var item in m_shaderList)
            {
                item.Value.Dispose();
            }

            foreach (var item in m_textureList)
            {
                item.Value.Dispose();
            }

            foreach (var item in m_fontList)
            {
                item.Value.Dispose();
            }

            m_textureList.Clear();
            m_shaderList.Clear();
            m_fontList.Clear();

            m_textureList = null;
            m_shaderList = null;
            m_fontList = null;
            m_instance = null;
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
                    LoadShader("UI");
                    LoadFont("PixelFont");
                    LoadFont("PixelFont2");
                    break;
                case ApplicationType.Server:

                    break;
            }
        }

        /// <summary>
        /// Used for client load resources, all. Shader,Textures, Sounds etc.
        /// </summary>
        public void ClientLoadResources()
        {

        }

        /// <summary>
        /// This is for server loading some resources EX: 3d models to do the collider stuff
        /// </summary>
        public void ServerLoadResources()
        {

        }

        /// <summary>
        /// Used to Client Reload resources at runtime, for debug or to load a diferent resource pack
        /// </summary>
        public void ClientReloadResources()
        {

        }

        /// <summary>
        /// Used to Server Reload resources at runtime, for debug or to load a diferent resource pack
        /// </summary>
        public void ServerReloadResources()
        {

        }

        public static Shader GetShader(string ShaderName)
        {
            if (instance.m_shaderList.TryGetValue(ShaderName, out Shader shader))
            {
                return shader;
            }

            return null;
        } 

        #region LoadFunctions
        private void LoadShader(string ShaderName)
        {
            try
            {
                ShaderFile shaderFile = GLSLLoad.LoadShaderFile("/Shaders/", ShaderName);
                Shader shader = new Shader(shaderFile);
                if (m_shaderList.ContainsKey(ShaderName)) { return; }
                m_shaderList.Add(ShaderName, shader);
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
                if (m_textureList.ContainsKey(TextureName)) { return; }
                m_textureList.Add(TextureName,texture);
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
                if (m_fontList.ContainsKey(FontName)) { return; }
                m_fontList.Add(FontName, new FontType(FontName));
                Debug.Log("Font Loaded! : " + FontName, "RESOURCES-MANAGER");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        public static ResourcesManager instance { get { return m_instance; } }
    }
}
