using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal static class Day3
{
    public static void Solve(string path)
    {


        var data = File.ReadAllText(path);

        var matches = Regex.Matches(data, @"mul\((\d+),(\d+)\)");

        var part1 = matches.Aggregate(0, (acc, m) => {
            int x = Convert.ToInt32(m.Groups[1].Value);
            int y = Convert.ToInt32(m.Groups[2].Value);
            return acc + (x * y);
        });

        Console.WriteLine($"Part 1: {part1}");


        var matches2 = Regex.Matches(data, @"(do)\(\)|(don't)\(\)|(mul)\((\d+),(\d+)\)");

        var part2 = matches2.Aggregate((0, true), (acc, m) => {
            
            if(m.Groups[1].Value != "") { return (acc.Item1, true); }

            if(m.Groups[2].Value != "") { return (acc.Item1, false); }

            if(m.Groups[3].Value != "" && acc.Item2) 
            {
                int x = Convert.ToInt32(m.Groups[4].Value);
                
                int y = Convert.ToInt32(m.Groups[5].Value);

                return (acc.Item1 + (x * y), false);
            }

            return acc;
        });


        Console.WriteLine($"Part 2: {part2.Item1}");
    }
}
