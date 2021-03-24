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
            transform.Position = new Vector3d(0, 0, 0);
            transform.Size = Vector3d.One;
            m_shader = ResourcesManager.GetShader("UI");
        }

        public void Tick()
        {
            transform.Tick();
        }

        public void Draw()
        {
            m_shader.StartUsingShader();

            if (Client.projection == null) { return; }

            m_shader.Set("world", transform.GetTransformWorld);
            m_shader.Set("projection", Matrix4.CreateOrthographicOffCenter(WindowMain.Instance.WindowRectangle.Left, WindowMain.Instance.WindowRectangle.Right, WindowMain.Instance.WindowRectangle.Bottom, WindowMain.Instance.WindowRectangle.Top, 0f, 5.0f));
            m_shader.Set("uicolor", Color4.Yellow);

            //m_shader.StopUsingShader();
        }

        //REMOVER ISSO DEPOIS APENAS PARA USO TEMPORARIO, USO PARA O DESENVOLVIMENTO APENAS
        public void StopUseShader()
        {
            m_shader.StopUsingShader();
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
