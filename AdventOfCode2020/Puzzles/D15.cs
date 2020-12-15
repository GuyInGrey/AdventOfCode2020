using System.Collections.Generic;

namespace GuyInGrey_AoC2020.Puzzles
{
    [Puzzle(@"PuzzleInputs\Day15\input.txt", "Day15", 15)]
    public class D15
    {
        List<int> numbers;

        [Benchmark(0)]
        public void Setup(PuzzleAttribute info)
        {
            numbers = new List<int>() { 0, 1, 4, 13, 15, 12, 16 };
            //numbers = new List<int>() { 0, 3, 6 };
        }

        [Benchmark(1)]
        public object Part1()
        {
            return GetNumAtIndex(2020);
        }

        [Benchmark(2, 3)]
        public object Part2()
        {
            return GetNumAtIndex(30000000);
        }

        public int GetNumAtIndex(int index)
        {
            var lastNum = -1;
            var nums = new Dictionary<int, (int firstIndex, int lastIndex, int secondLastIndex)>();

            for (var turn = 0; turn < index; turn++)
            {
                if (turn < numbers.Count)
                {
                    nums.Add(numbers[turn], (turn, turn, turn));
                    lastNum = numbers[turn];
                    continue;
                }

                if (nums[lastNum].firstIndex == turn - 1)
                {
                    nums[0] = (nums[0].firstIndex, turn, nums[0].lastIndex);
                    lastNum = 0;
                }
                else
                {
                    var num = nums[lastNum].lastIndex - nums[lastNum].secondLastIndex;
                    if (nums.ContainsKey(num))
                    {
                        nums[num] = (nums[num].firstIndex, turn, nums[num].lastIndex);
                    }
                    else
                    {
                        nums.Add(num, (turn, turn, turn));
                    }
                    lastNum = num;
                }
            }
            return lastNum;
        }
    }
}
