using Projectsln.darkcomsoft.src.debug;
using Projectsln.darkcomsoft.src.enums;
using Projectsln.darkcomsoft.src.gui.guisystem.font;
using Projectsln.darkcomsoft.src.resources;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Projectsln.darkcomsoft.src.engine;

namespace Projectsln.darkcomsoft.src.gui.guisystem.guielements
{
    public class Text : GUIBase
    {
        private FontRender m_fontRender;

        public Text()
        {
            m_fontRender = new FontRender("Text Teste ?!*:;@%#$%R0123456789一種", 3, 100, new RectangleF(0, 0, 100, 100), this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            m_inputEnable = true;
        }

        public Text(RectangleF positionSize) : base(positionSize)
        {
            m_fontRender = new FontRender("Text Teste ?!*:;@%#$%R0123456789一種", 3, 100, new RectangleF(0, 0, 100, 100), this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            m_inputEnable = false;
        }

        public Text(RectangleF positionSize, GUIDock gUIDock) : base(positionSize, gUIDock)
        {
            m_fontRender = new FontRender("Text Teste ?!*:;@%#$%R0123456789一種", 3, 100, new RectangleF(0, 0, 100, 100), this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            m_inputEnable = true;
        }

        public Text(RectangleF positionSize, GUIDock gUIDock, GUIPivot gUIPivot) : base(positionSize, gUIDock, gUIPivot)
        {
            m_fontRender = new FontRender("Text Teste ?!*:;@%#$%R0123456789一種", 3, 100, new RectangleF(0, 0, 100, 100), this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            m_inputEnable = true;
        }

        protected override void OnTick()
        {
            m_fontRender?.SetText(string.Format("FPS:{0}", Time._Time));
            base.OnTick();
        }

        protected override void OnDraw()
        {
            m_fontRender?.Draw();
            Gizmo.DrawRectangle(m_worldPosition, m_projection, Color4.Red,PrimitiveType.LineLoop);
            base.OnDraw();
        }

        protected override void OnResize(bool after)
        {
            if (!after)
            {
                m_fontRender?.OnResize();
            }
            base.OnResize(after);
        }

        protected override void OnDispose()
        {
            m_fontRender?.Dispose();
            m_fontRender = null;
            base.OnDispose();
        }
    }
}
