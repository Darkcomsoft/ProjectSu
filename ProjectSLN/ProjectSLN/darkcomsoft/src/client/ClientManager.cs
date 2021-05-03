using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.client
{
    /// <summary>
    /// Client Manager used to controll the game logic, like join a world, etc. <see cref="Client"/> is more for the Client Engine stuff like rendering
    /// </summary>
    public class ClientManager : ClassBase
    {
        public static ClientManager instance { get; private set; }

        public ClientManager()
        {
            instance = this;
        }

        protected override void OnDispose()
        {

            instance = null;
            base.OnDispose();
        }

        #region NetworkStuff
        #endregion

        #region WorldStuff
        private void JoinWorld()
        {

        }
        #endregion
    }
}
