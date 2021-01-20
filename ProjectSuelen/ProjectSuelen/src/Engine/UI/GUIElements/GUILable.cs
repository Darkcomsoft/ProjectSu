﻿using ProjectSuelen;
using ProjectSuelen.src.Engine.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using ProjectSuelen.src.Engine.UI.Font;
using OpenTK.Graphics;
using ProjectSuelen.src.Engine.AssetsPipeline;

namespace ProjectSuelen.src.Engine.UI.GUIElements
{
    public class GUILable : GUIBase
    {
        public string TextureName;

        public string FontName = "PixelFont2";
        public Color TextColor = Color.Black;

        private FontRender fontRender;

        public GUILable(string Text, Rectangle rec) : base(rec)
        {
            Start(Text);
        }

        public GUILable(string Text, Rectangle rec, UIDock uIDock) : base(rec, uIDock)
        {
            Start(Text);
        }

        private void Start(string text)
        {
            fontRender = new FontRender(text, 22, FontName, new Vector2(0f, 0f), GetRectangle.Width, GetRectangle);

            NoInteractable();
            HideFocus();
            ShowBackGround = false;
        }

        public override void OnResize()
        {
            if (fontRender != null)
            {
                fontRender.Resize(GetRectangle);
            }
            base.OnResize();
        }

        public override void RenderAfter()
        {
            if (fontRender != null)
            {
                fontRender.TickRender();
            }
            base.RenderAfter();
        }

        protected override void OnDispose()
        {
            if (fontRender != null)
            {
                fontRender.Dispose();
            }
            //_drawing.Dispose();
            //RendeTextOption = null;
            base.OnDispose();
        }

        public override void RenderCustomValues()
        {
            if (TextureName != null)
            {
                if (!TextureName.Equals(string.Empty))
                {
                    AssetManager.UseTexture(TextureName);
                    GUI.GetShader.Setbool("HaveTexture", true);
                }
                else
                {
                    GUI.GetShader.Setbool("HaveTexture", false);
                }
            }
            base.RenderCustomValues();
        }

        public void SetText(string text)
        {
            if (fontRender != null)
            {
                fontRender.UpdateText(text);
            }
        }

        public void SetColor(Color4 color)
        {
            if (fontRender != null)
            {
                fontRender.SetColor(color);
            }
        }

        public void SetTextAling(TextAling al)
        {
            fontRender.textAling = al;

            if (fontRender != null)
            {
                fontRender.Resize(GetRectangle);
            }
        }
    }
}
