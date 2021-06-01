using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.enums
{
    /// <summary>
    /// AppNetworkType define what type of application network is runing.
    /// </summary>
    public enum AppNetworkType : byte
    {
        /// <summary>
        /// None, Application is not Inicialized
        /// </summary>
        none,
        /// <summary>
        /// Application is a Client Type, if is Client is connect to a server
        /// </summary>
        Client,
        /// <summary>
        /// Application is a ClientSinglePlayerServer Type, if is ClientSinglePlayerServer is a Client Hosting a server, not dedicated
        /// </summary>
        ClientSinglePlayerServer,
        /// <summary>
        /// Application is a Server Type, A server Dedicated!
        /// </summary>
        Server
    }
}
