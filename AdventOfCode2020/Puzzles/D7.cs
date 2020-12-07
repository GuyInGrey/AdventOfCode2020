using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

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
            var lines = File.ReadAllText(info.DataFilePath).Replace("bags", "bag").Replace("\r", "").Replace(".", "")
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var l in lines)
            {
                var node = new BagNode()
                {
                    Name = l.Substring(0, l.IndexOf(" contain "))
                };
                Rules.Add(node);
                if (node.Name == "shiny gold bag") { Shiny = node; }
            }

            for (var i = 0; i < lines.Length; i++)
            {
                var node = Rules[i];
                var cS = lines[i].Split(new[] { " contain " }, StringSplitOptions.RemoveEmptyEntries)[1]
                    .Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

                if (cS[0] != "no other bag")
                {
                    foreach (var s in cS)
                    {
                        var num = int.Parse(s.Split(' ')[0]);
                        var name = s.Substring(s.IndexOf(' ') + 1);
                        var cNode = Rules.First(c => c.Name == name);
                        node.Children.Add((cNode, num));
                        cNode.Parents.Add(node);
                    }
                }
            }

            //var temp = new List<(string, (string, int)[])>();
            //foreach (var i in lines)
            //{
            //    var parts = i.Split(new[] { " contain " }, StringSplitOptions.RemoveEmptyEntries);
            //    var parent = parts[0];
            //    var childrenS = parts[1];
            //    var childrenS2 = new List<string>();

            //    if (childrenS.Contains(", "))
            //    {
            //        childrenS2 = childrenS.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            //    }
            //    else
            //    {
            //        if (childrenS != "no other bag")
            //        {
            //            childrenS2.Add(childrenS);
            //        }
            //    }

            //    var children = childrenS2.Select(c =>
            //    {
            //        var num = int.Parse(c.Split(' ')[0]);
            //        var name = string.Join(" ", c.Split(' ').Skip(1));
            //        return (name, num);
            //    }).ToArray();
            //    temp.Add((parent, children));
            //}

            //foreach (var t in temp)
            //{
            //    Rules.Add(new BagNode()
            //    {
            //        Name = t.Item1,
            //    });
            //}

            //for (var i = 0; i < Rules.Count; i++)
            //{
            //    var node = Rules[i];
            //    var t = temp[i];

            //    foreach (var c in t.Item2)
            //    {
            //        var cNode = Rules.First(c2 => c2.Name == c.Item1);
            //        node.Children.Add((cNode, c.Item2));
            //        cNode.Parents.Add(node);
            //    }

            //    if (node.Name == "shiny gold bag")
            //    {
            //        Shiny = node;
            //    }
            //}
        }

        [Benchmark(1)]
        public int Part1()
        {
            var p = new List<BagNode>();
            Shiny.GetAllParents(ref p);
            return p.Count;
        }

        [Benchmark(2)]
        public int Part2()
        {
            return Shiny.CountAllChildren();
        }
    }

    public class BagNode
    {
        public string Name;
        public List<(BagNode, int)> Children = new List<(BagNode, int)>();
        public List<BagNode> Parents = new List<BagNode>();

        public void GetAllParents(ref List<BagNode> collected)
        {
            foreach (var p in Parents)
            {
                if (!collected.Contains(p)) { collected.Add(p); }
            }

            foreach (var b in Parents)
            {
                b.GetAllParents(ref collected);
            }
        }

        public int CountAllChildren()
        {
            var toReturn = 0;
            foreach (var c in Children)
            {
                toReturn += (1 + c.Item1.CountAllChildren()) * c.Item2;
            }
            return toReturn;
        }
    }
}
