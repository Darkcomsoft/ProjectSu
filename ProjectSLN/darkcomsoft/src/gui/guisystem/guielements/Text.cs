using ProjectIND.darkcomsoft.src.debug;
using ProjectIND.darkcomsoft.src.enums;
using ProjectIND.darkcomsoft.src.gui.guisystem.font;
using ProjectIND.darkcomsoft.src.resources;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ProjectIND.darkcomsoft.src.engine;

namespace ProjectIND.darkcomsoft.src.gui.guisystem.guielements
{
    /// <summary>
    /// Simple Gui Text, used for display text on the screen, with a font
    /// </summary>
    public class Text : GUIBase
    {
        private FontRender v_fontRender;

        public Text()
        {
            v_fontRender = new FontRender("Texte Sample", 25, 1,  this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            v_inputEnable = true;
        }

        public Text(string text,RectangleF positionSize, float fontSize) : base(positionSize)
        {
            v_fontRender = new FontRender(text, fontSize, 1,  this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            v_inputEnable = false;
        }

        public Text(string text, RectangleF positionSize, GUIDock gUIDock, float fontSize) : base(positionSize, gUIDock)
        {
            v_fontRender = new FontRender(text, fontSize, 1,  this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            v_inputEnable = true;
        }

        public Text(string text, RectangleF positionSize, GUIDock gUIDock, GUIPivot gUIPivot, float fontSize) : base(positionSize, gUIDock, gUIPivot)
        {
            v_fontRender = new FontRender(text, fontSize, 1,  this, ResourcesManager.GetFont("PixelFont2"), ResourcesManager.GetShader("Font"));
            v_inputEnable = true;
        }

        public void SetText(string text)
        {
            v_fontRender?.SetText(text);
        }

        public void SetTextAling(TextAling textAling)
        {
            v_fontRender?.SetTextPivot(textAling);
        }

        public void SetTextColor(Color4 color)
        {
            v_fontRender?.SetColor(color);
        }

        protected override void OnDraw()
        {
            v_fontRender?.Draw();
            Gizmo.DrawRectangle(v_worldPosition, v_projection, Color4.Red,PrimitiveType.LineLoop);
            base.OnDraw();
        }

        protected override void OnResize(bool after)
        {
            v_fontRender?.OnResize();
            base.OnResize(after);
        }

        protected override void OnDispose()
        {
            v_fontRender?.Dispose();
            v_fontRender = null;
            base.OnDispose();
        }
    }
}
