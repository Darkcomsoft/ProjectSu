using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Projectsln.darkcomsoft.src.debug
{
    /// <summary>
    /// Use <see cref="System.Diagnostics.Stopwatch"/> to calculate time comsumption in the systems, <see cref="Start(string)"/> Start the timer. <see cref="Stop(string)"/> Stop the timer, and return the time passed
    /// </summary>
    public static class PerformanceProfiler
    {
        public static Dictionary<string, Stopwatch> m_stopWatchList = new Dictionary<string, Stopwatch>();

        public static void Start(string watchName)
        {
            if (!Debug.isDebugEnabled) { return; }

            if (!m_stopWatchList.ContainsKey(watchName))
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                m_stopWatchList.Add(watchName, sw);
            }
        }

        public static PerformanceTime Stop(string watchName)
        {
            if (!Debug.isDebugEnabled) { return new PerformanceTime(); }

            if (m_stopWatchList.ContainsKey(watchName))
            {
                Stopwatch stopwatch = m_stopWatchList[watchName];
                m_stopWatchList[watchName].Stop();

                m_stopWatchList.Remove(watchName);

                if (stopwatch.Elapsed.TotalMilliseconds >= 00.9d)
                {
                    Debug.Log("This system Take: " + watchName + " >>> " + stopwatch.Elapsed.ToString() + ". to Execute!");
                }

                return new PerformanceTime(stopwatch.Elapsed, stopwatch.ElapsedMilliseconds, stopwatch.ElapsedTicks);
            }

            return new PerformanceTime();
        }
    }

    public struct PerformanceTime
    {
        public TimeSpan timeSpan;
        public long ElapsedMilliseconds;
        public long ElapsedTicks;

        public PerformanceTime(TimeSpan elapsed, long elapsedMilliseconds, long elapsedTicks) : this()
        {
            timeSpan = elapsed;
            ElapsedMilliseconds = elapsedMilliseconds;
            ElapsedTicks = elapsedTicks;
        }

        public bool isNull()
        {
            return this.Equals(new PerformanceTime());
        }
    }
}
