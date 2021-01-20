using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.Engine
{
    public class Camera : ObjectBase
    {
        private static Camera Main;

        public Camera()
        {
            Main = this;
        }

        protected override void OnDispose()
        {
            if (Main == this)
            {
                Main = null;
            }
            base.OnDispose();
        }

        public static Camera main { get { return Main; } private set { } }
    }
}
