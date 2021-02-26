using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.misc
{
    public static class Utilits
    {
        public static I CreateInstance<I>(Type type) where I : class
        {
            return Activator.CreateInstance(type) as I;
        }
    }
}
