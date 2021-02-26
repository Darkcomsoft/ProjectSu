using Projectsln.darkcomsoft.src.engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.world
{
    public class MainMenuWorld : World
    {
        public override void Start()
        {
            Debug.Log("MainMenu Started!");
            base.Start();
        }

        protected override void OnDispose()
        {
            Debug.Log("MainMenu Disposed!");
            base.OnDispose();
        }
    }
}
