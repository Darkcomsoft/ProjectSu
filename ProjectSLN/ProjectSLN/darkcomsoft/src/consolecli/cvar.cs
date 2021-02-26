using Projectsln.darkcomsoft.src.engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.consolecli
{
    public class cvar : ClassBase
    {
        protected bool needAdvancedPermission = false;
        public void Execute(params object[] param) { if (needAdvancedPermission) { return; } else { OnExecute(param); } }

        protected virtual void OnExecute(params object[] param)
        {

        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }

        public void CheckParamsSize(int Length, int count)
        {
            if (Length > count) { Debug.cvarError("Error params is not the same size of the cvar"); return; }
            if (Length < count) { Debug.cvarError("Error params is not the same size of the cvar"); return; }
        }

        /// <summary>
        /// print a cvar log
        /// </summary>
        /// <param name="msg"></param>
        public void Log(string msg)
        {
            Debug.cvarLog(msg);
        }
    }
}
