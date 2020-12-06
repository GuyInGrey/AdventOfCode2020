using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GuyInGrey_AoC2020.Puzzles
{
    [Puzzle(filePath: @"PuzzleInputs\Day3\input.txt", name: "Day3")]
    public class D3
    {
        TreeMap Map;

        [Benchmark(0)]
        public void Setup(PuzzleAttribute info)
        {
            Map = new TreeMap(File.ReadAllText(info.DataFilePath));
        }

        [Benchmark(1)]
        public object Part1()
        {
            return Map.TreesEncounteredOnPath(3, 1);
        }

        [Benchmark(2)]
        public object Part2()
        {
            var slopes = new List<(int, int)>()
            {
                (1, 1),
                (3, 1),
                (5, 1),
                (7, 1),
                (1, 2),
            };

            long final = 1;
            foreach (var s in slopes)
            {
                final *= Map.TreesEncounteredOnPath(s.Item1, s.Item2);
            }
            return final;
        }
    }

    public class TreeMap
    {
        bool[,] Map;

        public int Width => Map.GetLength(0);
        public int Height => Map.GetLength(1);

        public TreeMap(string input)
        {
            var lines = input.Split().ToList();
            lines.ConvertAll(l => l.Trim());
            lines.RemoveAll(l => l == "");

            Map = new bool[lines[0].Length, lines.Count];
            for (var x = 0; x < lines[0].Length; x++)
            {
                for (var y = 0; y < lines.Count; y++)
                {
                    Map[x, y] = lines[y][x] == '.';
                }
            }
        }

        public bool GetCoord(int x, int y)
        {
            x %= Width;
            y %= Height;

            return Map[x, y];
        }

        public int TreesEncounteredOnPath(int xSlope, int ySlope)
        {
            var x = 0;
            var y = 0;

            var trees = 0;
            do
            {
                trees = GetCoord(x, y) ? trees : trees + 1;

                x += xSlope;
                y += ySlope;
            } while (y < Height);
            return trees;
        }
    }
}
