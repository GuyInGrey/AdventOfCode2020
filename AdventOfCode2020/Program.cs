using System;
using System.IO;
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
            var markdown = "# AdventOfCode2020\nGuyInGrey's Solutions!\n" +
                "Featuring a custom-made benchmarking system, accurate to 0.4 μs.\n\n## Timing Results:\n" +
                "|Name|Time|\n|-|-|\n";

            Console.WriteLine(" - 1000 iterations each -");
            foreach (var t in types)
            {
                var benchmark = new BenchmarkedPuzzle(t);
                benchmark.Run(1000);

                foreach (var r in benchmark.BenchmarkResults)
                {
                    markdown += $"|{r.Information.Name} - {r.BenchmarkedMethod.Name}|" +
                        $"{r.TimeTaken.TotalMilliseconds} ms|\n";
                }
                markdown += "| | |\n";

                Console.WriteLine(string.Join("\n", benchmark.BenchmarkResults) + "\n");
            }

            File.WriteAllText(@"C:\Users\NoahL\source\repos\AdventOfCode2020\README.md", markdown);
        }
    }
}
