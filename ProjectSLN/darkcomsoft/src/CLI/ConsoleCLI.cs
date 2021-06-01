using ProjectIND.darkcomsoft.src.misc;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.CLI
{
    public static class ConsoleCLI
    {
        public static void Execute<T>(params string[] parameters)
        {
            cvar cva = Utilits.CreateInstance<cvar>(typeof(T));
            cva.Execute(parameters);
            cva.Dispose();
        }

        public static void Execute(Type type, params string[] parameters)
        {
            cvar cva = Utilits.CreateInstance<cvar>(type);
            cva.Execute(parameters);
            cva.Dispose();
        }
    }
}
