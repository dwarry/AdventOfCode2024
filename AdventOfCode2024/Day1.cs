using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal static class Day1
{
    public static void Part1(string path)
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

        var result = list1.Zip(list2).Select(tup => Math.Abs(tup.First - tup.Second)).Sum();

        Console.WriteLine($"Part 1: {result}");

        Dictionary<int, int> counts = [];

        foreach(var i in list2)
        {
            if(counts.ContainsKey(i))
            {
                counts[i] += i;
            }
            else
            {
                counts[i] = i;
            }
        }
        
        var similarityScore = list1.Sum(x => counts.GetValueOrDefault(x, 0));

        Console.WriteLine(similarityScore);
    }
}
