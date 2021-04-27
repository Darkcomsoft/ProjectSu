﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src
{
    public abstract class BuildTypeBase : ClassBase
    {
        public virtual void Tick() { }
        public virtual void TickDraw() { }

        public virtual void OnResize() { }
    }
}
