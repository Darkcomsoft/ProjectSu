﻿using ProjectSLN.darkcomsoft.src.debug;
using ProjectSLN.darkcomsoft.src.enums;
using ProjectSLN.darkcomsoft.src.gui.guisystem.font;
using ProjectSLN.darkcomsoft.src.render;
using ProjectSLN.darkcomsoft.src.resources.resourcestype;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProjectSLN.darkcomsoft.src.resources
{
    public class ResourcesManager : ClassBase
    {
        private static ResourcesManager m_instance;
        private Dictionary<string, Shader> m_shaderList;
        private Dictionary<string, Texture> m_textureList;
        private Dictionary<string, Font> m_fontList;

        public ResourcesManager()
        {
            m_instance = this;
            m_shaderList = new Dictionary<string, Shader>();
            m_textureList = new Dictionary<string, Texture>();
            m_fontList = new Dictionary<string, Font>();
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

        public static Texture GetTexture(string TextureName)
        {
            if (instance.m_textureList.TryGetValue(TextureName, out Texture texture))
            {
                return texture;
            }
            return null;
        }

        public static Font GetFont(string FontName)
        {
            if (instance.m_fontList.TryGetValue(FontName, out Font font))
            {
                return font;
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
                m_fontList.Add(FontName, new Font(FontName));
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
