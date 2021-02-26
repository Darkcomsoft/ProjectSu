using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.world
{
    /// <summary>
    /// Used for global game system, this world dont destroy only if you quit to main menu
    /// </summary>
    public class SystemWorld : World
    {
        public override void Start()
        {
            base.Start();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
