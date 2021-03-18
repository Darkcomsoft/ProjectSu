using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Projectsln.darkcomsoft.src.server
{
    public class ServerLogic : ClassBase
    {
		private Application application;
		private bool serverIsRuning = false;
		private System.Diagnostics.Stopwatch _watchUpdate = new System.Diagnostics.Stopwatch();

		public void Run()
        {
			application = new Application(ApplicationType.Server);
			serverIsRuning = true;

            while (serverIsRuning)
            {
				application?.Tick(_watchUpdate.Elapsed.TotalSeconds);

				Application.windowsConsole?.SetTitleConsole("ProjectEvllyn-Server FPS: " + (int)(1f / _watchUpdate.Elapsed.TotalSeconds));

				Time._DeltaTime = (double)_watchUpdate.Elapsed.TotalSeconds;
				Time._DTime += _watchUpdate.Elapsed.TotalSeconds;
				Time._Time++;

				Time._Tick = Time._Time % 60;

				if (Time._Time >= double.MaxValue)
				{
					Time._Time = 0;
				}

				_watchUpdate.Restart();
				Thread.Sleep(10);
			}

			Dispose();//dispose this
		}

        public void Exit()
        {
            serverIsRuning = false;
        }

        protected override void OnDispose()
        {
			application.Dispose();

			application = null;
			_watchUpdate = null;
			base.OnDispose();
        }
	}
}