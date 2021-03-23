using Projectsln.darkcomsoft.src.enums;
using Projectsln.darkcomsoft.src.render;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.resources
{
    public class ResourcesManager : ClassBase
    {
        private Dictionary<string, Shader> m_shaderList;

        public ResourcesManager(ApplicationType apptype)
        {
            m_shaderList = new Dictionary<string, Shader>();

            LoadPreResources(apptype);
        }

        protected override void OnDispose()
        {
            foreach (var item in m_shaderList)
            {
                item.Value.Dispose();
            }

            m_shaderList.Clear();
            m_shaderList = null;
            base.OnDispose();
        }

        /// <summary>
        /// Used for load resources for the pre systems, Like splashScreen
        /// </summary>
        private void LoadPreResources(ApplicationType apptype)
        {
            switch (apptype)
            {
                case ApplicationType.Client:

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

        private void LoadShader()
        {

        }
    }
}
