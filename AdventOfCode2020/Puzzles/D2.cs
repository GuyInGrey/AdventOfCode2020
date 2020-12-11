using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GuyInGrey_AoC2020.Puzzles
{
    [Puzzle(@"PuzzleInputs\Day2\input.txt", "Day2", 2)]
    public class D2
    {
        List<GroupCollection> Input;

        [Benchmark(0)]
        public void Setup(PuzzleAttribute info)
        {
            var inp = File.ReadAllText(info.DataFilePath)
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Trim());

            var reg = new Regex(@"(\d+)-(\d+)\s(.):\s(.+)");
            Input = new List<GroupCollection>();
            foreach (var i in inp)
            {
                Input.Add(reg.Matches(i)[0].Groups);
            }
        }

        [Benchmark(1)]
        public object Part1()
        {
            var invalid = 0;

            foreach (var m in Input)
            {
                var min = int.Parse(m[1].Value);
                var max = int.Parse(m[2].Value);
                var ch = m[3].Value[0];
                var pw = m[4].Value;

                var count = pw.Where(c => c == ch).Count();
                if (count < min || count > max) { invalid++; }
            }

            return Input.Count - invalid;
        }

        [Benchmark(2)]
        public object Part2()
        {
            var invalid = 0;

            foreach (var m in Input)
            {
                var min = int.Parse(m[1].Value);
                var max = int.Parse(m[2].Value);
                var ch = m[3].Value[0];
                var pw = m[4].Value;

                if (!(pw[min - 1] == ch ^ pw[max - 1] == ch))
                {
                    invalid++;
                }
            }

            return Input.Count - invalid;
        }
    }
}
