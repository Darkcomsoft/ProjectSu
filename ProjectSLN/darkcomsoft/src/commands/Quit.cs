using ProjectIND.darkcomsoft.src;
using ProjectIND.darkcomsoft.src.CLI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.commands
{
    public class Quit : cvar
    {
        public Quit()
        {
            needAdvancedPermission = false;
        }

        protected override void OnExecute(params string[] param)
        {
            if (CheckParamsSize(param.Length, 0))
            {
                Application.CloseApp();
                Log("Closing The App!");
            }
            base.OnExecute(param);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
