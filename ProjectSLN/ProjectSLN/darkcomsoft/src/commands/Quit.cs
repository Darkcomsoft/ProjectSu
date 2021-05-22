using ProjectSLN.darkcomsoft.src;
using ProjectSLN.darkcomsoft.src.consolecli;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSLN.darkcomsoft.src.commands
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
