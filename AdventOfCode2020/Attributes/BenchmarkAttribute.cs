using System;

namespace GuyInGrey_AoC2020
{
    public class BenchmarkAttribute : Attribute
    {
        public int Priority { get; }
        public BenchmarkAttribute(int priority)
        {
            Priority = priority;
        }
    }
}
