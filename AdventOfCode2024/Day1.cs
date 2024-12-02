using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal static class Day1
{
    public static void Solve(string path)
    {
        List<int> list1 = [];
        List<int> list2 = [];

        foreach(string line in File.ReadLines(path))
        {
            var parts = Regex.Split(line, @"\s+");

            list1.Add(Convert.ToInt32(parts[0]));
            list2.Add(Convert.ToInt32(parts[1]));
        }

        list1.Sort();
        list2.Sort();

        var result = list1
            .Zip(list2)
            .Select(tup => Math.Abs(tup.First - tup.Second))
            .Sum();

        Console.WriteLine($"Part 1: {result}");

        var lookup = list2.ToLookup(x => x);

        var similarityScore = list1.Sum(x => lookup[x].Sum());

        Console.WriteLine($"Part 2 = {similarityScore}");
    }
}
