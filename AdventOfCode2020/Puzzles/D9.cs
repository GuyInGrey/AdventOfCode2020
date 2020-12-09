using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GuyInGrey_AoC2020.Puzzles
{
    [Puzzle(@"PuzzleInputs\Day9\input.txt", "Day9")]
    public class D9
    {
        List<long> nums;
        long invalid;

        [Benchmark(0)]
        public void Setup(PuzzleAttribute info)
        {
            nums = File.ReadAllText(info.DataFilePath).Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => long.Parse(s)).ToList();
        }

        [Benchmark(1)]
        public long Part1()
        {
            for (var i = 25; i < nums.Count; i++)
            {
                var val = nums[i];

                if (!SumExists(nums.Skip(i - 25).Take(25).ToList(), val))
                {
                    invalid = val;
                    return val; 
                }
            }

            return int.MinValue;
        }

        public bool SumExists(List<long> nums, long toAdd)
        {
            for (var i = 0; i < nums.Count; i++)
            {
                for (var j = i + 1; j < nums.Count; j++)
                {
                    if (nums[i] + nums[j] == toAdd) { return true; }
                }
            }
            return false;
        }

        [Benchmark(2)]
        public object Part2()
        {
            var f = FindFactors(nums, invalid);
            return f.Min() + f.Max();
        }

        public List<long> FindFactors(List<long> data, long toAdd)
        {
            for (var i = 0; i < data.Count; i++)
            {
                var currentSum = data[i];
                //var amount = 2;
                for (var j = i + 1; j < data.Count; j++)
                {
                    currentSum += data[j];
                    if (currentSum == toAdd)
                    {
                        return data.Skip(i).Take((j - i) + 1).ToList();
                    }
                    else if (currentSum > toAdd) { j = data.Count; }
                    //amount++;
                }
            }

            return new List<long>();
        }
    }
}
