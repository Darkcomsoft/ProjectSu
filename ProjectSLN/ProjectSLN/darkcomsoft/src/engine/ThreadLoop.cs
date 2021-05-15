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
        private string m_threadName = "";
        private int m_tickRate = 15;

        private Thread m_thread;
        private System.Diagnostics.Stopwatch m_watchUpdate;

        private ThreadCallBack m_tickCall;
        private ThreadCallBack m_startCall;
        private ThreadCallBack m_destroyCall;

        public ThreadLoop(ThreadCallBack tickDele, ThreadCallBack startDele, ThreadCallBack destroyDele)
        {
            try
            {
                m_runnig = false;

                m_tickCall = tickDele;
                m_startCall = startDele;
                m_destroyCall = destroyDele;

                m_watchUpdate = new System.Diagnostics.Stopwatch();

                m_thread = new Thread(new ThreadStart(Run));
                m_thread.Priority = ThreadPriority.BelowNormal;
                m_thread.IsBackground = true;
                m_thread.Start();

                m_threadName = m_thread.ManagedThreadId.ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ThreadLoop(string ThreadName, ThreadPriority threadPriority, bool threadBackground,int TickRate, ThreadCallBack tickDele, ThreadCallBack startDele, ThreadCallBack destroyDele)
        {
            try
            {
                m_runnig = false;
                m_tickRate = TickRate;

                m_tickCall = tickDele;
                m_startCall = startDele;
                m_destroyCall = destroyDele;

                m_watchUpdate = new System.Diagnostics.Stopwatch();

                m_thread = new Thread(new ThreadStart(Run));
                m_thread.Name = ThreadName;
                m_thread.Priority = threadPriority;
                m_thread.IsBackground = threadBackground;
                m_thread.Start();

                m_threadName = ThreadName;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void Run()
        {
            try
            {
                m_runnig = true;
                Start();

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
                        Debug.Log(l_ticks + " Ticks, ", "THREAD-" + m_threadName);
                        lastTimer1 += 1000;
                        l_ticks = 0;
                    }
                }

                Destroy();//call before thread finish
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void Tick() { m_tickCall(); }

        private void Start() { m_startCall(); }
        private void Destroy() { m_destroyCall(); Debug.Log("Thread Loop is finished!", "THREAD-" + m_threadName); }

        protected override void OnDispose()
        {
            try
            {
                m_runnig = false;

                m_thread.Join();//NAO SEI SE ISSO FAZ O QUE ESTOU QUERENDO, MAS EU VI QUE ISSO DA UM BLOCK NO MAIN THREAD, ATE ESTE THREAD FINALIZAR, NAO SEI

                m_watchUpdate = null;
                m_thread = null;

                m_tickCall = null;
                m_startCall = null;
                m_destroyCall = null;
            }
            catch (Exception e)
            {
                throw e;
            }
            base.OnDispose();
        }
    }
    public delegate void ThreadCallBack();
}
