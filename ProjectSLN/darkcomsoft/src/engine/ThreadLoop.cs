using ProjectIND.darkcomsoft.src;
using ProjectIND.darkcomsoft.src.debug;
using ProjectIND.darkcomsoft.src.engine.window;
using ProjectIND.darkcomsoft.src.server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProjectIND.darkcomsoft.src.engine
{
    /// <summary>
    /// This start a loop thread like in the <see cref="WindowMain"/> and <see cref="ServerMain"/>, a normal gameloop
    /// </summary>
    public class ThreadLoop : ClassBase
    {
        private bool v_runnig;
        private string v_threadName = "";
        private int v_tickRate = 15;

        private Thread v_thread;
        private System.Diagnostics.Stopwatch v_watchUpdate;

        private ThreadCallBack v_tickCall;
        private ThreadCallBack v_startCall;
        private ThreadCallBack v_destroyCall;

        public ThreadLoop(ThreadCallBack tickDele, ThreadCallBack startDele = null, ThreadCallBack destroyDele = null)
        {
            try
            {
                v_runnig = false;

                v_tickCall = tickDele;
                v_startCall = startDele;
                v_destroyCall = destroyDele;

                v_watchUpdate = new System.Diagnostics.Stopwatch();

                v_thread = new Thread(new ThreadStart(Run));
                v_thread.Priority = ThreadPriority.BelowNormal;
                v_thread.IsBackground = true;
                v_thread.Start();

                v_threadName = v_thread.ManagedThreadId.ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ThreadLoop(string ThreadName, ThreadPriority threadPriority, bool threadBackground,int TickRate, ThreadCallBack tickDele, ThreadCallBack startDele = null, ThreadCallBack destroyDele = null)
        {
            try
            {
                v_runnig = false;
                v_tickRate = TickRate;

                v_tickCall = tickDele;
                v_startCall = startDele;
                v_destroyCall = destroyDele;

                v_watchUpdate = new System.Diagnostics.Stopwatch();

                v_thread = new Thread(new ThreadStart(Run));
                v_thread.Name = ThreadName;
                v_thread.Priority = threadPriority;
                v_thread.IsBackground = threadBackground;
                v_thread.Start();

                v_threadName = ThreadName;
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
                v_runnig = true;
                Start();

                v_watchUpdate.Start();

                var l_lastTime = v_watchUpdate.ElapsedMilliseconds;
                var l_mspertick = 1000.0d / v_tickRate;
                var l_noprocess = 0d;
                var l_ticks = 0;
                var lastTimer1 = v_watchUpdate.ElapsedMilliseconds;
                long now = 0;

                while (v_runnig)
                {
                    now = v_watchUpdate.ElapsedMilliseconds;
                    l_noprocess += (now - l_lastTime) / l_mspertick;
                    l_lastTime = now;
                    while (l_noprocess >= 1)
                    {
                        l_ticks++;
                        Tick();
                        l_noprocess -= 1;
                    }

                    Thread.Sleep(1);
                    if (v_watchUpdate.ElapsedMilliseconds - lastTimer1 > 1000)
                    {
                        if (Debug.isDebugEnabled)
                        {
                            Debug.Log(l_ticks + " Tick", "THREAD-" + v_threadName);
                        }
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

        private void Tick() { v_tickCall?.Invoke(); }

        private void Start() { v_startCall?.Invoke(); }
        private void Destroy() { v_destroyCall?.Invoke(); Debug.Log("Thread Loop is finished!", "THREAD-" + v_threadName); }

        protected override void OnDispose()
        {
            try
            {
                v_runnig = false;

                v_thread.Join();//NAO SEI SE ISSO FAZ O QUE ESTOU QUERENDO, MAS EU VI QUE ISSO DA UM BLOCK NO MAIN THREAD, ATE ESTE THREAD FINALIZAR, NAO SEI

                v_watchUpdate = null;
                v_thread = null;

                v_tickCall = null;
                v_startCall = null;
                v_destroyCall = null;
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
