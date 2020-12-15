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

        public void Run()
        {
            var iterations = 500;
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            // Initialize Objects
            var instances = new List<object>();
            for (var i = 0; i < iterations; i++)
            {
                instances.Add(Activator.CreateInstance(ClassType));
            }

            foreach (var m2 in ToBenchmark.Select(m => (m, m.GetAttribute<BenchmarkAttribute>())).OrderBy(m => m.Item2.Priority))
            {
                var m = m2.Item1;
                ManageGC();
                Thread.Sleep(300);
                var p = new object[] { Info };

                object result = null;

                var startTime = HighResolutionDateTime.UtcNow;
                var iterationsRan = 0;
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
                    iterationsRan++;
                    if (iterationsRan >= m2.Item2.BenchmarkRuns) { break; }

                    //if ((HighResolutionDateTime.UtcNow - startTime).TotalSeconds > 30) { break; }
                }
                var endTime = HighResolutionDateTime.UtcNow;
                var timeTaken = endTime - startTime;
                timeTaken = new TimeSpan(timeTaken.Ticks / iterationsRan);

                BenchmarkResults.Add(new TimingResult()
                {
                    Information = Info,
                    BenchmarkedType = ClassType,
                    BenchmarkedMethod = m,
                    IterationsRan = iterationsRan,
                    Result = result,
                    TimeTaken = timeTaken,
                });
            }
            ManageGC();
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
                $" {IterationsRan}x " +
                $"- {TimeTaken.TotalMilliseconds} ms" + 
                (Result != null ? $" - {Result}" : "");
        }
    }
}
