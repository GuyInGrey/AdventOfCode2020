using System;
using System.IO;

namespace GuyInGrey_AoC2020.Puzzles
{
    [Puzzle(@"PuzzleInputs\Day12\input.txt", "Day12", 12)]
    public class D12
    {
        string[] instructions;

        [Benchmark(0)]
        public void Setup(PuzzleAttribute info)
        {
            instructions = File.ReadAllText(info.DataFilePath).Replace("\r", "")
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        [Benchmark(1)]
        public int Part1()
        {
            var x = 0;
            var y = 0;
            var direction = Direction.East;
            foreach (var s in instructions)
            {
                var p1 = s.Substring(0, 1);
                var p2 = int.Parse(s.Substring(1, s.Length - 1));
                switch (p1)
                {
                    case "W": x -= p2; break;
                    case "E": x += p2; break;
                    case "N": y -= p2; break;
                    case "S": y += p2; break;
                    case "R": direction = (Direction)(((int)direction + p2 + 360) % 360); break;
                    case "L": direction = (Direction)(((int)direction - p2 + 360) % 360); break;
                    case "F": 
                        switch (direction)
                        {
                            case Direction.West: x -= p2; break;
                            case Direction.East: x += p2; break;
                            case Direction.North: y -= p2; break;
                            case Direction.South: y += p2; break;
                        }
                        break;
                }

                //Console.WriteLine($"({x}, {y}, {direction}) - {s}");
            }

            return Math.Abs(x) + Math.Abs(y);
        }

        [Benchmark(2)]
        public int Part2()
        {
            var x = 0;
            var y = 0;
            var wX = 10;
            var wY = -1;
            foreach (var s in instructions)
            {
                var p1 = s.Substring(0, 1);
                var p2 = int.Parse(s.Substring(1, s.Length - 1));
                switch (p1)
                {
                    case "W": wX -= p2; break;
                    case "E": wX += p2; break;
                    case "N": wY -= p2; break;
                    case "S": wY += p2; break;
                    case "R":
                        var ans = Rotate(wX, wY, p2);
                        wX = ans.Item1;
                        wY = ans.Item2;
                        break;
                    case "L":
                        ans = Rotate(wX, wY, -p2);
                        wX = ans.Item1;
                        wY = ans.Item2;
                        break;
                    case "F":
                        x += wX * p2;
                        y += wY * p2;
                        break;
                }

                //Console.WriteLine($"({x}, {y}, {wX}, {wY}) - {s}");
            }

            return Math.Abs(x) + Math.Abs(y);
        }

        public enum Direction
        {
            North = 0,
            East = 90,
            South = 180,
            West = 270,
        }

        public (int, int) Rotate(int x, int y, double angle)
        {
            angle = (Math.PI / 180) * angle;
            var s = Math.Sin(angle);
            var c = Math.Cos(angle);

            var x2 = (int)Math.Round(x * c - y * s);
            var y2 = (int)Math.Round(x * s + y * c);

            return (x2, y2);
        }
    }
}
