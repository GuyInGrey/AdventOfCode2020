using System;

namespace GuyInGrey_AoC2020
{
    public class BenchmarkAttribute : Attribute
    {
        public int Priority { get; }
        public int BenchmarkRuns { get; }
        public BenchmarkAttribute(int priority, int runs = 500)
        {
            Priority = priority;
            BenchmarkRuns = runs;
        }
    }
}
