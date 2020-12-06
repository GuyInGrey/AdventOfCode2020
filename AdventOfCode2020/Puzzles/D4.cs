using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GuyInGrey_AoC2020.Puzzles
{
    [Puzzle(filePath: @"PuzzleInputs\Day4\input.txt", name: "Day4")]
    public class D4
    {
        string[] Passports;

        [Benchmark(0)]
        public void Setup(PuzzleAttribute info)
        {
            Passports = File.ReadAllText(info.DataFilePath)
                .Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        [Benchmark(1)]
        public object Part1()
        {
            return Passports.Where(p =>
            {
                var keys = p.Split(new[] { " ", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList()
                    .ConvertAll(c => c.Split(':')[0]);
                keys.RemoveAll(k => k == "cid");
                return keys.Distinct().Count() == 7;
            }).Count();
        }

        [Benchmark(2)]
        public object Part2()
        {
            var validNum = new Func<string, int, int, int, bool>((number, min, max, digits) =>
            {
                if (int.TryParse(number, out var num))
                {
                    return num.ToString().Length != digits ? false : num >= min && num <= max;
                }
                else { return false; }
            });

            var validators = new Dictionary<string, Func<string, bool>>()
            {
                { "byr", new Func<string, bool>((data) => validNum(data, 1920, 2002, 4))},
                { "iyr", new Func<string, bool>((data) => validNum(data, 2010, 2020, 4))},
                { "eyr", new Func<string, bool>((data) => validNum(data, 2020, 2030, 4))},
                { "hgt", new Func<string, bool>((data) =>
                {
                    if (data.EndsWith("cm") || data.EndsWith("in"))
                    {
                        var type = data.Substring(data.Length - 2, 2);
                        var num = int.Parse(data.Substring(0, data.Length - 2));

                        return type == "cm" ? num >= 150 && num <= 193
                        : type == "in" ? num >= 59 && num <= 76 : false;
                    } else { return false; }
                })},
                { "hcl", new Func<string, bool>((data) => Regex.IsMatch(data, @"#[0-9a-f]{6}")) },
                { "ecl", new Func<string, bool>((data) => new [] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth"}.Contains(data)) },
                { "pid", new Func<string, bool>((data) => data.Length == 9) }
            };

            var valid = 0;
            foreach (var p in Passports)
            {
                var types = new List<string>();

                var parts = p.Split(new[] { "\n", " " }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var p2 in parts)
                {
                    var type = p2.Split(':')[0];
                    var value = p2.Split(':')[1];

                    if (validators.ContainsKey(type))
                    {
                        if (validators[type](value)) { types.Add(type); }
                    }
                }

                types.RemoveAll(c => c == "cid");
                if (types.Count == 7) { valid++; }
            }
            return valid;
        }

        public D4()
        {

        }
    }
}
