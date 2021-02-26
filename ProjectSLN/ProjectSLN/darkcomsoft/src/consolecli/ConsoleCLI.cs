using Projectsln.darkcomsoft.src.misc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.consolecli
{
    public static class ConsoleCLI
    {
        public static void Execute<T>(params object[] parameters)
        {
            cvar cva = Utilits.CreateInstance<cvar>(typeof(T));
            cva.Execute(parameters);
            cva.Dispose();
            cva = null;
        }
    }
}
