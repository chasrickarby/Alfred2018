using System.Diagnostics;

namespace Sensors
    {
    class Timer
        {
        private static Stopwatch stopwatch;

        static Timer()
            {
            stopwatch = Stopwatch.StartNew();
            }

        /// <summary>
        /// A synchronous wait is used to avoid yielding the thread 
        /// This method calculates the number of CPU ticks will elapse in the specified time and spins
        /// in a loop until that threshold is hit.This allows for very precise timing.
        /// </summary>
        public void Wait(double milliseconds)
            {
            long initialTick = stopwatch.ElapsedTicks;
            long initialElapsed = stopwatch.ElapsedMilliseconds;
            double desiredTicks = milliseconds / 1000.0 * Stopwatch.Frequency;
            double finalTick = initialTick + desiredTicks;
            while (stopwatch.ElapsedTicks < finalTick)
                {
                }
            }
        }
    }
