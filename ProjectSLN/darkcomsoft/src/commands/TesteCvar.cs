using ProjectIND.darkcomsoft.src.CLI;
using ProjectIND.darkcomsoft.src.debug;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectIND.darkcomsoft.src.commands
{
    public class TesteCvar : cvar
    {
        public TesteCvar()
        {
            needAdvancedPermission = false;
        }

        protected override void OnExecute(params string[] param)
        {
            if (CheckParamsSize(param.Length, 1))
            {
                Log("Teste cvar is working! " + param[0]);
            }
            base.OnExecute(param);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
