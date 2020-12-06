using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GuyInGrey_AoC2020.Puzzles
{
    [Puzzle(@"PuzzleInputs\Day6\input.txt", "Day6")]
    public class D6
    {
        public List<string> groups;

        [Benchmark(0)]
        public void Setup(PuzzleAttribute info)
        {
            var input = File.ReadAllText(info.DataFilePath).Trim();
            groups = input.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        [Benchmark(1)]
        public int Part1()
        {
            return groups.Select(s => s.Replace("\n", "").Distinct().Count()).Sum();
        }

        [Benchmark(2)]
        public int Part2()
        {
            var toReturn = 0;
            foreach (var g in groups)
            {
                var people = g.Split('\n').Select(p => p.ToCharArray());
                var intersection = people
                    .Skip(1)
                    .Aggregate(
                        new HashSet<char>(people.First()),
                        (h, e) => { h.IntersectWith(e); return h; }
                    );
                toReturn += intersection.Distinct().Count();
            }

            return toReturn;
        }
    }
}
