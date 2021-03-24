using OpenTK.Mathematics;
using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.engine.window;
using Projectsln.darkcomsoft.src.render;
using Projectsln.darkcomsoft.src.resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Projectsln.darkcomsoft.src.gui.guisystem.guielements
{
    public class GUIBase : ClassBase
    {
        private Transform transform;
        private Shader m_shader;

        public GUIBase()
        {
            transform = new Transform();
            transform.Position = new Vector3d(WindowMain.Instance.Width / 2, WindowMain.Instance.Height / 2, 0);
            transform.Size = Vector3d.One;
            m_shader = ResourcesManager.GetShader("UI");
        }

        public void Tick()
        {
            transform.Tick();
        }

        public void Draw()
        {
            m_shader.Use();

            if (Client.projection == null) { return; }

            m_shader.Set("world", transform.GetTransformWorld);
            m_shader.Set("projection", Client.projection);
            m_shader.Set("uicolor", Color4.Yellow);
        }

        protected override void OnDispose()
        {
            transform?.Dispose();
            transform = null;
            m_shader = null;
            base.OnDispose();
        }
    }
}
