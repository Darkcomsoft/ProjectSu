using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.enums
{
    /// <summary>
    /// Gui Dock Type, EX: LeftTop, gui pivot is on the topleft of the screen
    /// </summary>
    public enum GUIDock : byte
    {
        Free, 
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
