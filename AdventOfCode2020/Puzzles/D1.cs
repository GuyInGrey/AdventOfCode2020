using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GuyInGrey_AoC2020.Puzzles
{
    [Puzzle(@"PuzzleInputs\Day1\input.txt", "Day1", 1)]
    public class D1
    {
        List<int> Numbers;

        [Benchmark(0)]
        public void Setup(PuzzleAttribute info)
        {
            Numbers = File.ReadAllText(info.DataFilePath)
                .Split('\n').ToList()
                .ConvertAll(n => n.Trim()).Where(n => n != "").ToList()
                .ConvertAll(n => int.Parse(n))
                .OrderBy(i => i).ToList();
        }

        [Benchmark(1)]
        public object Part1()
        {
            return string.Join(", ", FindAddingNums(Numbers.Select(n => (long)n).ToList(), 2020, 2));
        }

        [Benchmark(2)]
        public object Part2()
        {
            return string.Join(", ", FindAddingNums(Numbers.Select(n => (long)n).ToList(), 2020, 3));
        }

        /// <summary>
        /// REQUIRES <paramref name="input"/> TO BE SORTED
        /// </summary>
        public static List<long> FindAddingNums(List<long> input, long numToAddTo, long numCount)
        {
            if (numCount == 1)
            {
                foreach (var num in input)
                {
                    if (num > numToAddTo) { return new List<long>(); }
                    if (num == numToAddTo) { return new List<long>() { num }; }
                }
                return new List<long>();
            }
            else
            {
                foreach (var num in input)
                {
                    var input2 = input.Where(i => i != num);
                    var combo = FindAddingNums(input2.ToList(), numToAddTo - num, numCount - 1);
                    if (combo.Count == 0) { continue; }
                    else
                    {
                        combo.Add(num);
                        return combo;
                    }
                }

                return new List<long>();
            }
        }
    }
}
