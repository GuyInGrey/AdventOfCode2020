using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GuyInGrey_AoC2020.Puzzles
{
    [Puzzle(@"PuzzleInputs\Day11\input.txt", "Day11", 11)]
    public class D11
    {
        int width;
        int height;

        Seat[,] SeatsP1;
        Seat[,] SeatsP2;

        [Benchmark(0)]
        public void Setup(PuzzleAttribute info)
        {
            var input = File.ReadAllText(info.DataFilePath).Replace("\r", "")
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            width = input[0].Length;
            height = input.Length;

            SeatsP1 = new Seat[width, height];
            SeatsP2 = new Seat[width, height];

            var y = 0;
            var x = 0;
            foreach (var line in input)
            {
                foreach (var ch in line)
                {
                    if (ch == 'L') 
                    {
                        SeatsP1[x,y] = new Seat(x, y, width, height, ref SeatsP1);
                        SeatsP2[x, y] = new Seat(x, y, width, height, ref SeatsP2);
                    }
                    x++;
                }
                y++;
                x = 0;
            }

            for (y = 0; y < height; y++)
            {
                for (x = 0; x < width; x++)
                {
                    SeatsP1[x, y]?.SetSurrounding(SeatsP1);
                    SeatsP2[x, y]?.SetSurrounding(SeatsP2);
                }
            }
        }

        [Benchmark(1)]
        public int Part1()
        {
            return RunPart(1);
        }

        public int RunPart(int part)
        {
            var s = part == 1 ? SeatsP1 : SeatsP2;

            Seat.AnyChange = true;
            while (Seat.AnyChange)
            {
                Seat.AnyChange = false;

                Parallel.For(0, height, new ParallelOptions()
                {
                    MaxDegreeOfParallelism = 4,
                }, y =>
                {
                    for (var x = 0; x < width; x++)
                    {
                        s[x, y]?.Step(part);
                    }
                });

                Parallel.For(0, height, new ParallelOptions()
                {
                    MaxDegreeOfParallelism = 4,
                }, y =>
                {
                    for (var x = 0; x < width; x++)
                    {
                        if (s[x, y] is null) { continue; }
                        s[x, y].Occupied = s[x, y].NextOccupied;
                    }
                });
            }
            var t = 0;

            Parallel.For(0, height, new ParallelOptions()
            {
                MaxDegreeOfParallelism = 4,
            }, y =>
            {
                for (var x = 0; x < width; x++)
                {
                    if (s[x, y] is null) { continue; }
                    if (s[x, y].Occupied) { t++; }
                }
            });
            return t;
        }

        [Benchmark(2)]
        public int Part2()
        {
            return RunPart(2);
        }

        public class Seat
        {
            public static bool AnyChange;

            public int X;
            public int Y;
            public bool Occupied;
            public bool NextOccupied;

            public int MapWidth;
            public int MapHeight;

            public List<(int, int)> SurroundingP1 = new List<(int, int)>();
            public List<(int, int)> SurroundingP2 = new List<(int, int)>();

            public Seat[,] Seats;

            public Seat(int x, int y, int w, int h, ref Seat[,] s)
            { X = x; Y = y; MapWidth = w; MapHeight = h; Seats = s; }

            public void SetSurrounding(Seat[,] spaces)
            {
                var dirs = new List<(int, int)>();
                for (var y2 = -1; y2 <= 1; y2++)
                {
                    for (var x2 = -1; x2 <= 1; x2++)
                    {
                        if (!(x2 == 0 && y2 == 0)) { dirs.Add((x2, y2)); }
                        var x3 = X + x2;
                        var y3 = Y + y2;
                        if (x3 < 0 || x3 >= MapWidth || y3 < 0 || y3 >= MapHeight ||
                            (x3 == X && y3 == Y)) { continue; }
                        if (!(spaces[x3, y3] is null)) { SurroundingP1.Add((x3, y3)); }
                    }
                }

                var seats = dirs.Select(d => Direction(d.Item1, d.Item2, ref spaces));

                SurroundingP2 = seats.Where(s => s != (int.MaxValue, int.MaxValue)).ToList();
            }

            (int, int) Direction(int xM, int yM, ref Seat[,] spaces)
            {
                var x2 = X; var y2 = Y;
                while (true)
                {
                    x2 += xM;
                    y2 += yM;
                    if (x2 < 0 || x2 >= MapWidth || y2 < 0 || y2 >= MapHeight)
                    {
                        return (int.MaxValue, int.MaxValue);
                    }
                    else if (!(spaces[x2, y2] is null))
                    {
                        return (x2, y2);
                    }
                }
            }

            public void Step(int part)
            {
                var toSearch = part == 1 ? SurroundingP1 : SurroundingP2;
                var max = part == 1 ? 4 : 5;
                var occAround = 0;
                foreach (var t in toSearch)
                {
                    if (Seats[t.Item1, t.Item2].Occupied) { occAround++; }
                }

                NextOccupied = Occupied ? 
                    occAround < max : 
                    occAround == 0;

                if (NextOccupied != Occupied) { AnyChange = true; }
            }
        }
    }
}
