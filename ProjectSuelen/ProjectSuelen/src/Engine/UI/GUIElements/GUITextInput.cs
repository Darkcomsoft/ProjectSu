using ProjectSu.src.Engine.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectSu;
using OpenTK;

namespace ProjectSu.src.Engine.UI.GUIElements
{
    public class GUITextInput : GUIBase
    {
        public string Value = "";
        public string PlacerHolder = "A Input";

        public string FontName = "OpenSans";

        private Vector3d Pos;
        private SizeF MaxSize;

        public GUITextInput(Rectangle rec) : base(rec)
        {
            Start();
        }

        public GUITextInput(Rectangle rec, UIDock uIDock) : base(rec, uIDock)
        {
            Start();
        }

        private void Start()
        {
            Pos = new Vector3d(GetRectangle.X, GetRectangle.Y + (GetRectangle.Height / 2), 0);
            MaxSize = new SizeF(GetRectangle.Width, GetRectangle.Height);
        }

        public override void OnKeyPress(char KeyChar)
        {
            if (IsFocused)
            {
                if (Input.GetKeyDown(OpenTK.Input.Key.BackSpace))
                {
                    Value = "";
                }
                else
                {
                    Value += KeyChar;
                }
            }
            base.OnKeyPress(KeyChar);
        }

        public override void OnResize()
        {
            Pos = new Vector3d(GetRectangle.X, GetRectangle.Y + (GetRectangle.Height / 2), 0);
            MaxSize = new SizeF(GetRectangle.Width, GetRectangle.Height);
            base.OnResize();
        }

        public override void RenderAfter()
        {
            /*if (Value.Equals(string.Empty))
            {
                if (Time._Time % 60 <= 20 && IsFocused)
                {
                    _drawing.Print(AssetsManager.GetFont(FontName), PlacerHolder + "|", Pos, MaxSize, QFontAlignment.Justify, RendeTextOption);

                }
                else
                {
                    _drawing.Print(AssetsManager.GetFont(FontName), PlacerHolder + " ", Pos, MaxSize, QFontAlignment.Justify, RendeTextOption);
                }
            }
            else
            {
                if (Time._Time % 60 <= 20 && IsFocused)
                {
                    _drawing.Print(AssetsManager.GetFont(FontName), Value + "|", Pos, MaxSize, QFontAlignment.Justify, RendeTextOption);

                }
                else
                {
                    _drawing.Print(AssetsManager.GetFont(FontName), Value + " ", Pos, MaxSize, QFontAlignment.Justify, RendeTextOption);
                }
            }*/
            base.RenderAfter();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
