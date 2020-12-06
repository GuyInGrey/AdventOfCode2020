using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using GuyInGrey_AoC2020.Puzzles;

namespace GuyInGrey_AoC2020
{
    class Program
    {
        static void Main()
        {
            Thread.Sleep(2000);

            while (true)
            {
                Console.Write("Which type to benchmark? (\"all\" for all): ");
                var input = Console.ReadLine();

                if (input == "all")
                {
                    var types = Assembly.GetExecutingAssembly().GetTypes().Where(
                        t => !(t.GetAttribute<PuzzleAttribute>() is null));

                    BenchmarkTypes(types.ToArray());
                }
                else
                {
                    var type = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Name == input).FirstOrDefault();
                    if (type is null)
                    {
                        Console.WriteLine("Invalid.");
                    }
                    else { BenchmarkTypes(new[] { type }); }
                }
            }
        }

        public static void BenchmarkTypes(Type[] types)
        {
            Console.WriteLine(" - 1000 iterations each -");
            foreach (var t in types)
            {
                var benchmark = new BenchmarkedPuzzle(t);
                benchmark.Run(1000);

                Console.WriteLine(string.Join("\n", benchmark.BenchmarkResults) + "\n");
            }
        }
    }
}
