using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal static class Day2
{
    readonly static string[] data = @"7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9".Split("\r\n");

    public static void Solve(string path)
    {

        var reports = File.ReadLines(path)
                          .Select(x => x.Split(" ")
                                        .Select(y => Convert.ToInt32(y))
                                        .ToArray())
                          .ToArray();

        var safeCount =
            reports.Select(x => x.ToPairs()
                                 .Select(x => x.First - x.Second))
                   .Sum(x => IsSafe(x) ? 1 : 0);

        Console.WriteLine($"Part1: {safeCount}");
    
        bool IsSafe(IEnumerable<int> deltas)
        {
            return deltas.All(x => Math.Abs(x) <= 3)
                && (deltas.All(x => x > 0) || deltas.All(x => x < 0));
        }
    }

    public static IEnumerable<(T First, T Second)> ToPairs<T>(this IEnumerable<T> values) where T : struct
    {
        return values.Zip(values.Skip(1));
    }

    
}
