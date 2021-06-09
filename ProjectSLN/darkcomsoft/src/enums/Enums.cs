using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.enums
{
    public enum ChunkState : byte
    {
        notready, ready, deleted
    }

    public enum ChunkStage : byte
    {
        empty, do_voxel, do_Mesh, do_nature, do_finals, ready
    }

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

    public enum GUIElementStatus : byte
    {
        none, Hover, Focus
    }

    /// <summary>
    /// Set the pivot of a GUI element
    /// </summary>
    public enum GUIPivot : byte
    {
        Default,
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
