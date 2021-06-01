using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.misc
{
    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = true)]
    public class AuthorAttribute : System.Attribute
    {
        private string name;
        public double version;

        public AuthorAttribute(string name)
        {
            this.name = name;
            version = 1.0;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = false)]
    public class EngineOnly : System.Attribute
    {
        public EngineOnly()
        {
            
        }
    }
}
