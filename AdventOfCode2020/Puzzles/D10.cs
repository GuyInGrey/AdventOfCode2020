using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GuyInGrey_AoC2020.Puzzles
{
    [Puzzle(@"PuzzleInputs\Day10\input.txt", "Day10",10)]
    public class D10
    {
        List<long> input;

        [Benchmark(0)]
        public void Setup(PuzzleAttribute info)
        {
            input = File.ReadAllText(info.DataFilePath).Replace("\r", "")
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(c => long.Parse(c)).ToList();
        }

        [Benchmark(1)]
        public long Part1()
        {
            input.Sort();

            var ones = input[0];
            var threes = 1l;

            for (var i = 0; i < input.Count - 1; i++)
            {
                var d = input[i + 1] - input[i];
                if (d == 1) { ones++; }
                else if (d == 3) { threes++; }
            }

            return ones * threes;
        }

        [Benchmark(2)]
        public long Part2()
        {
            var last = input.Last() + 3;

            var nodes = new Dictionary<long, Node>
            {
                { last, new Node() { Num = last } }
            };

            foreach (var i in input)
            {
                nodes.Add(i, new Node() { Num = i });
            }

            foreach (var i in nodes.Keys)
            {
                // If it's near the end
                for (var j = i-3; j < i; j++)
                {
                    if (nodes.ContainsKey(j)) { nodes[i].Children.Add(nodes[j]); }
                }
            }

            return nodes[last].GetPathsToZero();
        }

        public class Node
        {
            public long Num;
            public List<Node> Children = new List<Node>();
            public long PathsToZero = long.MinValue;

            public long GetPathsToZero()
            {
                if (PathsToZero == long.MinValue)
                {
                    PathsToZero = Children.Select(c => c.GetPathsToZero()).Sum() + (Num < 4 ? 1 : 0);
                }
                return PathsToZero;
            }
        }
    }
}
