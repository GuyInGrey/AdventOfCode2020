using System;
using System.Collections.Generic;
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
//            var d = new D6();
//            d.groups = @"abc

//a
//b
//c

//ab
//ac

//a
//a
//a
//a

//b".Replace("\r", "").Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
//            //d.Setup(typeof(D6).GetAttribute<PuzzleAttribute>());
//            Console.WriteLine(d.Part2());

//            return;

            Thread.Sleep(2000);

            while (true)
            {
                var results = new List<List<TimingResult>>();

                Console.Write("Which type to benchmark? (\"all\" for all): ");
                var input = Console.ReadLine();

                if (input == "all")
                {
                    var types = Assembly.GetExecutingAssembly().GetTypes().Where(
                        t => !(t.GetAttribute<PuzzleAttribute>() is null));

                    results = BenchmarkTypes(types.ToArray());
                }
                else
                {
                    var type = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Name == input).FirstOrDefault();
                    if (type is null)
                    {
                        Console.WriteLine("Invalid.");
                    }
                    else { results = BenchmarkTypes(new[] { type }); }
                }

                Console.Write("Save results to readme? (y/n): ");
                var answer = Console.ReadKey();
                Console.WriteLine();

                if (answer.Key == ConsoleKey.Y)
                {
                    var methodNames = results.Select(s => s.Select(s2 => s2.BenchmarkedMethod.Name))
                        .SelectMany(s => s).ToList().Distinct();

                    var markdown = "# Advent Of Code 2020\nGuyInGrey's Solutions!\n" +
                        "Featuring a custom-made benchmarking system, accurate to 0.4 μs.\n\n## Timing Results:\n" +
                        "|Name|" + string.Join("|", methodNames) + "|\n|" + "-|".Repeat(methodNames.Count() + 1) + "\n";

                    foreach (var line in results)
                    {
                        var lineOut = new List<string>();
                        for (var i = 0; i < methodNames.Count(); i++)
                        {
                            lineOut.Add(" ");
                        }

                        for (var i = 0; i < line.Count; i++)
                        {
                            lineOut[methodNames.ToList().IndexOf(line[i].BenchmarkedMethod.Name)] = 
                                line[i].TimeTaken.TotalMilliseconds + " ms";
                        }
                        markdown += $"|{line[0].Information.Name}|{string.Join("|", lineOut)}|\n";
                    }
                    File.WriteAllText(@"C:\Users\NoahL\source\repos\AdventOfCode2020\README.md", markdown);
                }
            }
        }

        public static List<List<TimingResult>> BenchmarkTypes(Type[] types)
        {
            

            var toReturn = new List<List<TimingResult>>();

            Console.WriteLine(" - 1000 iterations each -");
            foreach (var t in types)
            {
                var benchmark = new BenchmarkedPuzzle(t);
                benchmark.Run(1000);

                //foreach (var r in benchmark.BenchmarkResults)
                //{
                //    markdown += $"|{r.Information.Name} - {r.BenchmarkedMethod.Name}|" +
                //        $"{r.TimeTaken.TotalMilliseconds} ms|\n";
                //}
                //markdown += "| | |\n";

                Console.WriteLine(string.Join("\n", benchmark.BenchmarkResults) + "\n");
                toReturn.Add(benchmark.BenchmarkResults);
            }

            return toReturn;
            //File.WriteAllText(@"C:\Users\NoahL\source\repos\AdventOfCode2020\README.md", markdown);
        }
    }
}
