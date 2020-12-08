using System;
using System.IO;

namespace GuyInGrey_AoC2020.Puzzles
{
    //[Puzzle(@"PuzzleInputs\Day#\input.txt", "Day#")]
    public class DT
    {
        [Benchmark(0)]
        public void Setup(PuzzleAttribute info)
        {
            var input = File.ReadAllText(info.DataFilePath);
        }

        [Benchmark(1)]
        public int Part1()
        {
            return 1;
        }

        [Benchmark(2)]
        public int Part2()
        {
            return 2;
        }
    }
}
