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

//        public PuzzleAttribute Info { get; }
//        public MethodInfo Setup;
//        public MethodInfo Part1;
//        public MethodInfo Part2;
//        public object Puzzle;

//        public static TimingResults[] TimeMultiple(Type[] toTest, int iterationCount)
//        {
//            var toReturn = new TimingResults[toTest.Length];
//            for (var i = 0; i < toTest.Length; i++)
//            {
//                toReturn[i] = Time(toTest[i], iterationCount);
//            }
//            return toReturn;
//        }

//        public static TimingResults Time(Type toTest, int iterationCount)
//        {
//            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
//            Thread.CurrentThread.Priority = ThreadPriority.Highest;

//            var info = toTest.GetAttribute<PuzzleAttribute>();
//            if (info is null) { throw new Exception("Invalid class. Must have proper attributes."); }
//            var day = info.Day;

//            var instances = new List<BenchmarkedPuzzle>();
//            for (var i = 0; i < iterationCount; i++)
//            {
//                var t = (BenchmarkedPuzzle)Activator.CreateInstance(toTest);
//                instances.Add(t);
//            }

//            GCManage();

//            var start = HighResolutionDateTime.UtcNow;
//            foreach (var i in instances)
//            {
//                i.LoadFile(day);
//                i.SetupOld();
//            }
//            var end = HighResolutionDateTime.UtcNow;
//            var setupTime = end - start;

//            GCManage();

//            start = HighResolutionDateTime.UtcNow;
//            foreach (var i in instances)
//            {
//                i.Part1Old();
//            }
//            end = HighResolutionDateTime.UtcNow;
//            var part1Time = end - start;

//            GCManage();

//            start = HighResolutionDateTime.UtcNow;
//            foreach (var i in instances)
//            {
//                i.Part2Old();
//            }
//            end = HighResolutionDateTime.UtcNow;
//            var part2Time = end - start;

//            var timings = new TimingResults()
//            {
//                IterationsRan = iterationCount,
//                Day = day,
//                Setup = new TimeSpan(setupTime.Ticks / iterationCount),
//                Part1 = new TimeSpan(part1Time.Ticks / iterationCount),
//                Part2 = new TimeSpan(part2Time.Ticks / iterationCount),
//            };

//            // Get puzzle results with a new instance
//            var result = (BenchmarkedPuzzle)Activator.CreateInstance(toTest);
//            result.LoadFile(day);
//            result.SetupOld();
//            timings.Part1Result = result.Part1Old();
//            timings.Part2Result = result.Part2Old();
//            return timings;
//        }

//        public static void GCManage()
//        {
//            Thread.Sleep(300);
//            GC.Collect();
//            GC.WaitForPendingFinalizers();
//            GC.Collect();
//        }
//    }

//    public class TimingResults
//    {
//        public int Day;
//        public int IterationsRan;
//        public TimeSpan Setup;
//        public TimeSpan Part1;
//        public TimeSpan Part2;
//        public TimeSpan Total => Setup + Part1 + Part2;

//        public object Part1Result;
//        public object Part2Result;

//        public override string ToString() =>
//            $"Day {Day}\n" +
//            $"Iterations Ran: {IterationsRan}\n\n" +
//            $"Setup: {Setup.TotalMilliseconds}ms\n" +
//            $"Part 1: {Part1.TotalMilliseconds}ms\n" +
//            $"Part 2: {Part2.TotalMilliseconds}ms\n" +
//            $"Total: {Total.TotalMilliseconds}ms\n\n";
//            //$"Part 1 Result: {Part1Result}\n" +
//            //$"Part 2 Result: {Part2Result}";
//    }
//}
