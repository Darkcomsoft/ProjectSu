using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSLN.darkcomsoft.src.enums
{
    /// <summary>
    /// ApplicationType define what type of application is runing, or to decide what type of app start.
    /// </summary>
    public enum ApplicationType : byte
    {
        /// <summary>
        /// None, Application is not Inicialized
        /// </summary>
        none, 
        /// <summary>
        /// Application is a Client Type
        /// </summary>
        Client, 
        /// <summary>
        /// Application is a Server Type
        /// </summary>
        Server
    }
}
