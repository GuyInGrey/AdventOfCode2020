using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GuyInGrey_AoC2020.Puzzles
{
    [Puzzle(@"PuzzleInputs\Day13\input.txt", "Day13", 13)]
    public class D13
    {
        int earliestTime = 0;
        List<int> busTimes;

        [Benchmark(0)]
        public void Setup(PuzzleAttribute info)
        {
            var input = File.ReadAllText(info.DataFilePath)
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            earliestTime = int.Parse(input[0]);
            busTimes = input[1].Split(',').Select(s => s == "x" ? int.MaxValue : int.Parse(s)).ToList();
        }

        [Benchmark(1)]
        public object Part1()
        {
            var l = busTimes.Where(b => b != int.MaxValue)
                .Select(b => (b, b - (earliestTime % b)))
                .OrderBy(b => b.Item2).First();
            return l.b * l.Item2;
        }

        [Benchmark(2)]
        public object Part2()
        {
            return null;
            //var min = 0;
            //for (var i = 0; i < busTimes.Count; i++)
            //{
            //    if (busTimes[i] == int.MaxValue) { continue; }
            //    min = busTimes
            //}
        }
    }
}
