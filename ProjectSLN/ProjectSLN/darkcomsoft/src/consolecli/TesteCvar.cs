using Projectsln.darkcomsoft.src.debug;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src.consolecli
{
    public class TesteCvar : cvar
    {
        public TesteCvar()
        {
            needAdvancedPermission = false;
        }

        protected override void OnExecute(params object[] param)
        {
            CheckParamsSize(param.Length, 1);

            Log("Teste cvar is working! " + param[0]);
            base.OnExecute(param);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
        }
    }
}
