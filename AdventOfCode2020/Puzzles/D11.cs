using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GuyInGrey_AoC2020.Puzzles
{
    [Puzzle(@"PuzzleInputs\Day11\input.txt", "Day11", 11)]
    public class D11
    {
        /// <summary>
        /// 0 = floor, 1 = empty, 2 = occupied
        /// </summary>
        int[,] Seats;

        [Benchmark(0)]
        public void Setup(PuzzleAttribute info)
        {
            var input = File.ReadAllText(info.DataFilePath).Replace("\r", "")
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            Seats = new int[input[0].Length, input.Length];

            var y = 0;
            foreach (var line in input)
            {
                var x = 0;
                foreach (var ch in line)
                {
                    Seats[x, y] =
                        ch == '.' ? 0 :
                        ch == 'L' ? 1 :
                        2;
                    x++;
                }
                y++;
            }
        }

        [Benchmark(1)]
        public int Part1()
        {
            var currentState = (int[,])Seats.Clone();
            var previousState = new int[Seats.GetLength(0), Seats.GetLength(1)];

            while (!MapsMatch(currentState, previousState))
            {
                //Console.WriteLine(GetVisual(currentState));
                //Console.WriteLine(CountType(currentState, 2));
                previousState = currentState;
                currentState = Step(currentState);
            }

            return CountType(currentState, 2);
        }

        [Benchmark(2)]
        public int Part2()
        {
            var currentState = (int[,])Seats.Clone();
            var previousState = new int[Seats.GetLength(0), Seats.GetLength(1)];

            while (!MapsMatch(currentState, previousState))
            {
                //Console.WriteLine(GetVisual(currentState));
                //Console.WriteLine(CountType(currentState, 2));
                previousState = currentState;
                currentState = Step2(currentState);
            }

            return CountType(currentState, 2);
        }

        public bool MapsMatch(int[,] a, int[,] b)
        {
            for (var y = 0; y < a.GetLength(1); y++)
            {
                for (var x = 0; x < a.GetLength(0); x++)
                {
                    if (a[x,y] != b[x,y]) { return false; }
                }
            }

            return true;
        }

        public int[,] Step(int[,] a)
        {
            var width = a.GetLength(0);
            var height = a.GetLength(1);

            var b = (int[,])a.Clone();
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var sitting = 0;

                    for (var y2 = -1; y2 <= 1; y2++)
                    {
                        for (var x2 = -1; x2 <= 1; x2++)
                        {
                            var x3 = x + x2;
                            var y3 = y + y2;
                            if (x3 < 0 || x3 >= width || y3 < 0 || y3 >= height) { continue; }
                            if (!(x3 == x && y3 == y) && a[x3, y3] == 2) { sitting++; }
                        }
                    }

                    if (a[x, y] == 1 && sitting == 0) { b[x, y] = 2; }
                    else if (a[x, y] == 2 && sitting >= 4) { b[x, y] = 1; }
                }
            }

            return b;
        }

        public int[,] Step2(int[,] a)
        {
            var width = a.GetLength(0);
            var height = a.GetLength(1);

            var b = (int[,])a.Clone();
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var sitting = new List<int>()
                    {
                        Direction(1, 0),
                        Direction(1, 1),
                        Direction(0, 1),
                        Direction(-1, 1),
                        Direction(-1, 0),
                        Direction(-1, -1),
                        Direction(0, -1),
                        Direction(1, -1),
                    }.Sum();

                    int Direction(int xM, int yM)
                    {
                        if (x == 9 && y == 7)
                        {

                        }

                        var x2 = x;
                        var y2 = y;

                        while (true)
                        {
                            x2 += xM;
                            y2 += yM;
                            if (x2 < 0 || x2 >= width || y2 < 0 || y2 >= height
                                || a[x2, y2] == 1)
                            {
                                return 0;
                            }
                            else if (a[x2, y2] == 2)
                            {
                                return 1;
                            }
                        }
                    }

                    if (a[x, y] == 1 && sitting == 0) { b[x, y] = 2; }
                    else if (a[x, y] == 2 && sitting >= 5) { b[x, y] = 1; }
                }
            }

            return b;
        }

        public int CountType(int[,] a, int type)
        {
            var amount = 0;
            for (var y = 0; y < a.GetLength(1); y++)
            {
                for (var x = 0; x < a.GetLength(0); x++)
                {
                    if (a[x, y] == type) { amount++; }
                }
            }
            return amount;
        }

        public string GetVisual(int[,] a)
        {
            var s = "";
            for (var y = 0; y < a.GetLength(1); y++)
            {
                for (var x = 0; x < a.GetLength(0); x++)
                {
                    var v = a[x, y];
                    s += v == 0 ? '.' :
                        v == 1 ? 'L' : '#';
                }
                s += "\n";
            }

            return s;
        }
    }
}
