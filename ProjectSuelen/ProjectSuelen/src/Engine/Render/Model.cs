using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.Engine.Render
{
    /// <summary>
    /// Model is a mesh data allready in videocard buffer, ready to draw
    /// </summary>
    public class Model : ObjectBase
    {
        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
