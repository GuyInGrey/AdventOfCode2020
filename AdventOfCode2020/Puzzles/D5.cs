using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GuyInGrey_AoC2020.Puzzles
{
    [Puzzle(@"PuzzleInputs\Day5\input.txt", "Day5", 5)]
    public class D5
    {
        List<int> Indices;

        [Benchmark(0)]
        public void Setup(PuzzleAttribute info)
        {
            Indices = File.ReadAllText(info.DataFilePath)
                .Replace("F", "0").Replace("L", "0").Replace("R", "1").Replace("B", "1")
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(n => Convert.ToInt32(n, 2)).ToList();
        }

        [Benchmark(1)]
        public object Part1()
        {
            return Indices.Max();
        }

        [Benchmark(2)]
        public object Part2()
        {
            var min = Indices.Min();
            var max = Indices.Max();

            var range = Enumerable.Range(min, max - min);
            var missing = range.Except(Indices).First();
            return missing;
        }
    }
}
