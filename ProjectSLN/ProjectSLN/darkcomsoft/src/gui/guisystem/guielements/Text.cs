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
            m_fontRender = new FontRender("Text Teste ?!*:;@%#$%R0123456789一種", 25, 1,  this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            m_inputEnable = true;
        }

        public Text(RectangleF positionSize, float fontSize) : base(positionSize)
        {
            m_fontRender = new FontRender("Text Teste ?!*:;@%#$%R0123456789一種", fontSize, 1,  this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            m_inputEnable = false;
        }

        public Text(RectangleF positionSize, GUIDock gUIDock, float fontSize) : base(positionSize, gUIDock)
        {
            m_fontRender = new FontRender("Text Teste ?!*:;@%#$%R0123456789一種", fontSize, 1,  this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            m_inputEnable = true;
        }

        public Text(RectangleF positionSize, GUIDock gUIDock, GUIPivot gUIPivot, float fontSize) : base(positionSize, gUIDock, gUIPivot)
        {

            m_fontRender = new FontRender("Text Teste ?!*:;@%#$%R0123456789一種", fontSize, 1,  this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            m_inputEnable = true;
        }

        public void SetText(string text)
        {
            m_fontRender?.SetText(text);
        }

        public void SetTextAling(TextAling textAling)
        {
            m_fontRender?.SetTextPivot(textAling);
        }

        public void SetTextColor(Color4 color)
        {
            m_fontRender?.SetColor(color);
        }

        protected override void OnDraw()
        {
            m_fontRender?.Draw();
            Gizmo.DrawRectangle(m_worldPosition, m_projection, Color4.Red,PrimitiveType.LineLoop);
            base.OnDraw();
        }

        protected override void OnResize(bool after)
        {
            m_fontRender?.OnResize();
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
