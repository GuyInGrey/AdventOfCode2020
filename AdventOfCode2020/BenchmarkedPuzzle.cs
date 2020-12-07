using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace GuyInGrey_AoC2020
{
    public class BenchmarkedPuzzle
    {
        private Type ClassType { get; }
        public PuzzleAttribute Info { get; }
        public MethodInfo[] ToBenchmark { get; }
        public bool FinishedRunning { get; private set; }
        public List<TimingResult> BenchmarkResults { get; private set; } = new List<TimingResult>();

        public BenchmarkedPuzzle(Type t)
        {
            if (t is null) { throw new ArgumentNullException(); }
            ClassType = t;

            Info = ClassType.GetAttribute<PuzzleAttribute>();
            if (Info is null) { throw new Exception("No PuzzleAttribute."); }

            ToBenchmark = t.GetMethods().Where(m => m.GetAttribute<BenchmarkAttribute>() != null).ToArray();
        }

        public void Run(int iterations)
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            // Initialize Objects
            var instances = new List<object>();
            for (var i = 0; i < iterations; i++)
            {
                instances.Add(Activator.CreateInstance(ClassType));
            }

            foreach (var m in ToBenchmark.OrderBy(m => m.GetAttribute<BenchmarkAttribute>().Priority))
            {
                ManageGC();
                Thread.Sleep(300);
                var p = new object[] { Info };

                object result = null;

                var startTime = HighResolutionDateTime.UtcNow;
                foreach (var i in instances)
                {
                    if (m.GetParameters().Length > 0)
                    {
                        result = m.Invoke(i, p);
                    }
                    else
                    {
                        result = m.Invoke(i, null);
                    }
                }
                var endTime = HighResolutionDateTime.UtcNow;
                var timeTaken = endTime - startTime;
                timeTaken = new TimeSpan(timeTaken.Ticks / iterations);

                BenchmarkResults.Add(new TimingResult()
                {
                    Information = Info,
                    BenchmarkedType = ClassType,
                    BenchmarkedMethod = m,
                    IterationsRan = iterations,
                    Result = result,
                    TimeTaken = timeTaken,
                });
            }
        }

        private void ManageGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }

    public class TimingResult
    {
        public PuzzleAttribute Information;
        public Type BenchmarkedType;
        public MethodInfo BenchmarkedMethod;
        public TimeSpan TimeTaken;
        public int IterationsRan;
        public object Result;

        public override string ToString()
        {
            return $"{Information.Name}\\{BenchmarkedMethod.Name}" +
                $" - {TimeTaken.TotalMilliseconds} ms" + 
                (Result != null ? $" - {Result}" : "");
        }
    }
}
