using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        var safeCount = reports.Count(IsSafe);

        Console.WriteLine($"Part1: {safeCount}");
    
        var safeCount2 = reports.Count(IsSafe2);

        Console.WriteLine($"Part2: {safeCount2}");

        bool IsSafe(int[] levels)
        {
            var deltas = levels.ToPairs()
                               .Select(x => x.First - x.Second);

            return deltas.All(x => Math.Abs(x) <= 3)
                && (deltas.All(x => x > 0) || deltas.All(x => x < 0));
        }

        bool IsSafe2(int[] levels)
        {
            return IsSafe(levels)
                || levels.Drop1().Any(x => IsSafe(x));
        }
    }

    public static IEnumerable<(T First, T Second)> ToPairs<T>(this IEnumerable<T> values) where T : struct
    {
        return values.Zip(values.Skip(1));
    }

    public static IEnumerable<T[]> Drop1<T>(this T[] levels)
    {
        for(int i = 0; i < levels.Length; i++)
        {
            yield return levels.Select((x, j) => (x, j != i))
                .Where(x => x.Item2)
                .Select(x => x.Item1)
                .ToArray();
        }
    }
}
