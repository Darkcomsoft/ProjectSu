using ProjectSLN.darkcomsoft.src.debug;
using ProjectSLN.darkcomsoft.src.enums;
using ProjectSLN.darkcomsoft.src.gui.guisystem.font;
using ProjectSLN.darkcomsoft.src.resources;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ProjectSLN.darkcomsoft.src.engine;

namespace ProjectSLN.darkcomsoft.src.gui.guisystem.guielements
{
    /// <summary>
    /// Simple Gui Text, used for display text on the screen, with a font
    /// </summary>
    public class Text : GUIBase
    {
        private FontRender m_fontRender;

        public Text()
        {
            m_fontRender = new FontRender("Texte Sample", 25, 1,  this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            m_inputEnable = true;
        }

        public Text(string text,RectangleF positionSize, float fontSize) : base(positionSize)
        {
            m_fontRender = new FontRender(text, fontSize, 1,  this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            m_inputEnable = false;
        }

        public Text(string text, RectangleF positionSize, GUIDock gUIDock, float fontSize) : base(positionSize, gUIDock)
        {
            m_fontRender = new FontRender(text, fontSize, 1,  this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            m_inputEnable = true;
        }

        public Text(string text, RectangleF positionSize, GUIDock gUIDock, GUIPivot gUIPivot, float fontSize) : base(positionSize, gUIDock, gUIPivot)
        {
            m_fontRender = new FontRender(text, fontSize, 1,  this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
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
