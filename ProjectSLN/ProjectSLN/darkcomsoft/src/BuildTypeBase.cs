using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src
{
    public abstract class BuildTypeBase : ClassBase
    {
        public virtual void Tick() { }
        public virtual void TickDraw() { }

        public virtual void OnResize() { }
        public virtual void OnMouseMove() { }
        public virtual void OnMouseDown(MouseButtonEventArgs e) { }
        public virtual void OnMouseUp(MouseButtonEventArgs e) { }
    }
}