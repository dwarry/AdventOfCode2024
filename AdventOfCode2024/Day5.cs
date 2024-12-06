using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal static class Day5
{
    public static void Solve(string path)
    {

        var lines = File.ReadAllLines(path);

        var breakPosition = Array.FindIndex(lines, x => x?.Length == 0);
        var rulesSection = lines[..breakPosition];
        var pagesSection = lines[(breakPosition + 1)..];

        var beforeAndAfter = rulesSection.Select(x => x.Split('|', 2));
        var allBeforeRules = beforeAndAfter.ToLookup(x => x[0], x => x[1]);
        var allAfterRules = beforeAndAfter.ToLookup(x => x[1], x => x[0]);

        var valid = pagesSection.Select(x => x.Split(',')).Where(isValid);

        var part1 = valid.Sum(x => Convert.ToInt32(x[x.Length / 2]));

        Console.WriteLine(part1);

        var invalid = pagesSection.Select(x => x.Split(',')).Where(x => !isValid(x));

        var reordered = invalid.Select(reorder).ToArray();

        var part2 = reordered.Sum(x => Convert.ToInt32(x[x.Length / 2]));

        Console.WriteLine(part2);

        bool isValid(string[] pages)
        {
            for(int i = 0; i < pages.Length; ++i) { 
                var currentPage = pages[i];
                var beforeRules = allBeforeRules[currentPage];
            
                if(pages[..i].Intersect(beforeRules).Count() > 0)
                {
                    return false;
                }
                
            }
            return true;
        }

        string[] reorder(string[] pages)
        {
            var result = new List<string>(pages);

            for(int i = 0; i < result.Count; ++i)
            {
                var currentPage = result[i];
                var beforeRules = allBeforeRules[currentPage];

                var mustBeBefore = result[..i].Intersect(beforeRules);

                if(mustBeBefore.Count() > 0)
                {
                    var positions = mustBeBefore.Select(x => result.IndexOf(x));
                    var firstPos = positions.Count() > 0 ? positions.Min() : -1;
                    if(firstPos >= 0)
                    {
                        result.RemoveAt(i);
                        result.Insert(firstPos, currentPage);
                    }
                    
                }
                
                
            }
            return result.ToArray();
        }
    }
}
