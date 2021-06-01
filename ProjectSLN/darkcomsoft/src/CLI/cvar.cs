using ProjectSLN.darkcomsoft.src.debug;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSLN.darkcomsoft.src.CLI
{
    public class cvar : ClassBase
    {
        protected bool needAdvancedPermission = false;
        public void Execute(params string[] param) { if (needAdvancedPermission) { return; } else { OnExecute(param); } }

        protected virtual void OnExecute(params string[] param)
        {
            Debug.Log("Cvar Executed: " + this.GetType().FullName, "CVAR");
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }

        public bool CheckParamsSize(int Length, int count)
        {
            if (count == 0) { return true; }
            if (Length == 0) { return false; }
            if (Length > count) { Debug.cvarError("Error params is not the same size of the cvar"); return false; }
            if (Length < count) { Debug.cvarError("Error params is not the same size of the cvar"); return false; }

            return true;
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
