using Projectsln.darkcomsoft.src;
using Projectsln.darkcomsoft.src.debug;
using Projectsln.darkcomsoft.src.engine.window;
using Projectsln.darkcomsoft.src.server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProjectSLN.darkcomsoft.src.engine
{
    /// <summary>
    /// This start a loop thread like in the <see cref="WindowMain"/> and <see cref="ServerMain"/>, a normal gameloop
    /// </summary>
    public class ThreadLoop : ClassBase
    {
        private bool m_runnig;
        private Thread m_thread;

        private int m_tickRate = 15;

        private System.Diagnostics.Stopwatch m_watchUpdate;

        public ThreadLoop()
        {
            m_runnig = false;

            m_watchUpdate = new System.Diagnostics.Stopwatch();

            m_thread = new Thread(new ThreadStart(Run));
            m_thread.Name = "Chunk-Spawn-Thread";
            m_thread.Priority = ThreadPriority.BelowNormal;
            m_thread.IsBackground = true;
            m_thread.Start();
        }

        public ThreadLoop(string ThreadName, ThreadPriority threadPriority, bool threadBackground,int TickRate)
        {
            m_runnig = false;
            m_tickRate = TickRate;

            m_watchUpdate = new System.Diagnostics.Stopwatch();

            m_thread = new Thread(new ThreadStart(Run));
            m_thread.Name = ThreadName;
            m_thread.Priority = threadPriority;
            m_thread.IsBackground = threadBackground;
            m_thread.Start();
        }

        private void Run()
        {
            m_runnig = true;
            ThreadOnStart();

            m_watchUpdate.Start();

            var l_lastTime = m_watchUpdate.ElapsedMilliseconds;
            var l_mspertick = 1000.0d / m_tickRate;
            var l_noprocess = 0d;
            var l_ticks = 0;
            var lastTimer1 = m_watchUpdate.ElapsedMilliseconds;
            long now = 0;

            while (m_runnig)
            {
                now = m_watchUpdate.ElapsedMilliseconds;
                l_noprocess += (now - l_lastTime) / l_mspertick;
                l_lastTime = now;
                while (l_noprocess >= 1)
                {
                    l_ticks++;
                    Tick();
                    l_noprocess -= 1;
                }

                Thread.Sleep(1);
                if (m_watchUpdate.ElapsedMilliseconds - lastTimer1 > 1000)
                {
                    Debug.Log(l_ticks + " ticks, ", "THREAD-LOOP");
                    lastTimer1 += 1000;
                    l_ticks = 0;
                }
            }

            ThreadOnDestroy();//call before thread finish
        }

        private void Tick() { }

        private void ThreadOnStart() { }
        private void ThreadOnDestroy() { }

        protected override void OnDispose()
        {
            m_runnig = false;

            if (m_thread.ThreadState != ThreadState.Stopped)
            {
                m_thread.Abort();
            }

            m_watchUpdate = null;
            m_thread = null;

            base.OnDispose();
        }
    }
}
