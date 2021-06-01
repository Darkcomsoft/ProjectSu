using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSLN.darkcomsoft.src.enums
{
    /// <summary>
    /// Gui Dock Type, EX: LeftTop of the screen
    /// </summary>
    public enum GUIDock : byte
    {
        Free, 
        Center,
        Left, 
        Right, 
        Top, 
        Bottom, 
        LeftTop, 
        LeftBottom, 
        RightTop, 
        RightBottom
    }
}
