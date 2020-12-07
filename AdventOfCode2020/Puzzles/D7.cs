using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace GuyInGrey_AoC2020.Puzzles
{
    [Puzzle(@"PuzzleInputs\Day7\input.txt", "Day7")]
    public class D7
    {
        List<BagNode> Rules = new List<BagNode>();
        BagNode Shiny;

        [Benchmark(0)]
        public void Setup(PuzzleAttribute info)
        {
            var input = File.ReadAllText(info.DataFilePath).Replace("bags", "bag").Replace("\r", "")
                .Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

            var temp = new List<(string, (string, int)[])>();
            foreach (var i in input)
            {
                var parts = i.Split(new[] { " contain " }, System.StringSplitOptions.RemoveEmptyEntries);
                var parent = parts[0];
                var childrenS = parts[1].Replace(".", "");
                var childrenS2 = new List<string>();

                if (childrenS.Contains(", "))
                {
                    childrenS2 = childrenS.Split(new[] { ", " }, System.StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                else
                {
                    if (childrenS != "no other bag")
                    {
                        childrenS2.Add(childrenS);
                    }
                }

                var children = childrenS2.Select(c =>
                {
                    var num = int.Parse(c.Split(' ')[0]);
                    var name = string.Join(" ", c.Split(' ').Skip(1));
                    return (name, num);
                }).ToArray();
                temp.Add((parent, children));
            }

            foreach (var t in temp)
            {
                Rules.Add(new BagNode()
                {
                    Name = t.Item1,
                });
            }

            for (var i = 0; i < Rules.Count; i++)
            {
                var node = Rules[i];
                var t = temp[i];

                foreach (var c in t.Item2)
                {
                    var cNode = Rules.First(c2 => c2.Name == c.Item1);
                    node.Children.Add((cNode, c.Item2));
                }

                if (node.Name == "shiny gold bag")
                {
                    Shiny = node;
                }
            }
        }

        [Benchmark(1)]
        public int Part1()
        {
            var searched = new List<(string, bool)>();

            var contains = 0;
            foreach (var r in Rules)
            {
                if (ContainsBag(r, Shiny, ref searched))
                {
                    contains++;
                }
            }

            return contains;
        }

        [Benchmark(2)]
        public int Part2()
        {
            return CountChildren(Shiny) - 1;
        }

        public bool ContainsBag(BagNode baseBag, BagNode searchingFor, ref List<(string, bool)> searched)
        {
            var a = searched.Where(s => s.Item1 == baseBag.Name);
            if (a.Count() > 0)
            {
                return a.First().Item2;
            }

            foreach (var b in baseBag.Children)
            {
                if (b.Item1.Name == searchingFor.Name)
                {
                    return true;
                }
            }

            foreach (var b in baseBag.Children)
            {
                if (ContainsBag(b.Item1, searchingFor, ref searched))
                {
                    searched.Add((baseBag.Name, true));
                    return true;
                }
            }

            searched.Add((baseBag.Name, false));
            return false;
        }

        public int CountChildren(BagNode baseBag)
        {
            var total = 1;
            foreach (var b in baseBag.Children)
            {
                total += CountChildren(b.Item1) * b.Item2;
            }
            return total;
        }
    }

    public class BagNode
    {
        public string Name;
        public List<(BagNode, int)> Children = new List<(BagNode, int)>();
    }
}
