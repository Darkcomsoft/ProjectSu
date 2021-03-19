﻿using Projectsln.darkcomsoft.src.engine;
using Projectsln.darkcomsoft.src.enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Projectsln.darkcomsoft.src.server
{
    public class ServerMain : ClassBase
    {
		private Application application;
		private bool serverIsRuning = false;
		private System.Diagnostics.Stopwatch _watchUpdate = new System.Diagnostics.Stopwatch();
		private System.Diagnostics.Stopwatch _watchTick = new System.Diagnostics.Stopwatch();
		private System.Diagnostics.Stopwatch _watchDrawn = new System.Diagnostics.Stopwatch();

		public void Run()
        {
			application = new Application(ApplicationType.Server);
			serverIsRuning = true;

			_watchUpdate.Start();
			_watchTick.Start();
			_watchDrawn.Start();

			var l_lastTime = _watchUpdate.ElapsedMilliseconds;
			var l_mspertick = 1000.0d / 60;
			var l_noprocess = 0d;
			var l_frames = 0;
			var l_ticks = 0;
			var lastTimer1 = _watchUpdate.ElapsedMilliseconds;
			long now = 0;
			var dorender = true;

			//REMOVER A PARTE DE RENDER DESSE LOPPING, NO SERVER NAO A NECESSIDADE DE TER UM RENDER LOOP APENAS UM TICK LOOP
			while (serverIsRuning)
            {
				dorender = true;
				now = _watchUpdate.ElapsedMilliseconds;
				l_noprocess += (now - l_lastTime) / l_mspertick;
				l_lastTime = now;
				while (l_noprocess >= 1)
				{
					l_ticks++;
					Tick();
					l_noprocess -= 1;
					dorender = true;
				}

				Thread.Sleep(1);

				if (dorender)
				{
					l_frames++;
					TickDraw();
				}
				
				if (_watchUpdate.ElapsedMilliseconds - lastTimer1 > 1000)
				{
					Application.windowsConsole?.SetTitleConsole("ProjectEvllyn-Server | " + l_ticks + " ticks, " + l_frames + " fps");
					//Debug.Log(ticks + " ticks, " + frames + " fps");
					lastTimer1 += 1000;
					l_frames = 0;
					l_ticks = 0;
				}
			}

			Dispose();//dispose this
		}

		private void Tick()
        {
			application?.Tick(_watchTick.Elapsed.TotalSeconds);

			Time._DeltaTime = (double)_watchTick.Elapsed.TotalSeconds;
			Time._DTime += _watchTick.Elapsed.TotalSeconds;
			Time._Time++;

			Time._Tick = Time._Time % 60;

			if (Time._Time >= double.MaxValue)
			{
				Time._Time = 0;
			}
			_watchTick.Restart();
		}

		private void TickDraw()
        {

			_watchDrawn.Restart();
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
			_watchTick = null;
			_watchDrawn = null;
			base.OnDispose();
        }
	}
}