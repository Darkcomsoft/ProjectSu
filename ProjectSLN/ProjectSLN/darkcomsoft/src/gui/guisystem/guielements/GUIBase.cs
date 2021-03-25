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
        private Rectangle transform;
        private Shader m_shader;

        private Matrix4 worldPosition;

        public GUIBase()
        {
            transform = new Rectangle(WindowMain.Instance.Width, 0,50,50);
            m_shader = ResourcesManager.GetShader("UI");
        }

        private bool isHover = false;

        public void Tick()
        {
            if (transform.IntersectsWith(new Rectangle((int)Input.mouseState.Position.X / 2, (int)Input.mouseState.Position.Y / 2, 1, 1)))
            {
                isHover = true;
            }
            else {
                isHover = false;
            }
        }

        public void Draw()
        {
            m_shader.Use();

            if (Client.projection == null) { return; }

            m_shader.Set("world", worldPosition);
            m_shader.Set("projection", Matrix4.CreateOrthographic(WindowMain.Instance.Width, WindowMain.Instance.Height, 0f, 5.0f));
            if (isHover)
            {
                m_shader.Set("uicolor", Color4.Blue);
            }
            else
            {
                m_shader.Set("uicolor", Color4.Yellow);
            }
        }

        protected override void OnDispose()
        {
            m_shader = null;
            base.OnDispose();
        }

        public virtual void OnResize()
        {
            transform.X = (WindowMain.Instance.Width / 2) - transform.Width;
            transform.Y = (WindowMain.Instance.Height / 2) - transform.Height;

            worldPosition = Matrix4.CreateScale(transform.Width, transform.Height, 0) * Matrix4.CreateTranslation(transform.X, -transform.Y, 0);
        }
    }
}
