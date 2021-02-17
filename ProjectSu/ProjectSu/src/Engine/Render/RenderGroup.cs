using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSu.src.Engine.Render
{
    /// <summary>
    /// Used to render many of same model, but with jus one DrawCall
    /// </summary>
    public class RenderGroup : ClassBase
    {
        private string model;
        private int RenderCount = 0;

        public RenderGroup(string model, int startRenderCount)
        {
            this.model = model;
            RenderCount = startRenderCount;
        }

        public void TickDraw()
        {

        }

        protected override void OnDispose()
        {
            RenderCount = 0;
            model = null;
            base.OnDispose();
        }
    }
}
