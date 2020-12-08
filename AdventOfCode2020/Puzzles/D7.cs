using System;
using System.Collections.Generic;
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
            var lines = File.ReadAllText(info.DataFilePath).Replace("bags", "bag").Replace(".", "")
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

                if (!lines[i].Contains("no other bag"))
                {
                    var cS = lines[i].Split(new[] { " contain " }, StringSplitOptions.RemoveEmptyEntries)[1]
                        .Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
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
            return (int)Shiny.CountAllChildren();
        }

        //[Benchmark(3)]
        public (BagNode, long) FindLargest()
        {
            return Rules.Select(r => (r, r.CountAllChildren())).OrderBy(c => c.Item2).Last();
        }
    }

    public class BagNode
    {
        public string Name;
        public List<(BagNode, long)> Children = new List<(BagNode, long)>();
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

        public long CountAllChildren()
        {
            var toReturn = 0l;
            foreach (var c in Children)
            {
                toReturn += (1 + c.Item1.CountAllChildren()) * c.Item2;
            }
            return toReturn;
        }

        public override string ToString() =>
            $"{Name} ({Children.Count} children, {Parents.Count} parents)";
    }
}
