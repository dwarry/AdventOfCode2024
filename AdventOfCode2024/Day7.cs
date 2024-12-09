using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal static class Day7
{
    public static void Solve(string path)
    {
        //        var lines = @"190: 10 19
        //3267: 81 40 27
        //83: 17 5
        //156: 15 6
        //7290: 6 8 6 15
        //161011: 16 10 13
        //192: 17 8 14
        //21037: 9 7 18 13
        //292: 11 6 16 20".Split("\r\n");

        var lines = File.ReadAllLines(path);

        var inputs = lines.Select(line => Input.Create(line)).ToImmutableArray();

        var part1 = inputs.Where(x => x.IsValid).Sum(x => x.TargetValue);

        Console.WriteLine($"Part1: {part1}");


        var part2 = inputs.Where(x => x.IsValid2).Sum(x => x.TargetValue);

        Console.WriteLine($"Part2: {part2}");
    }

    private record Input(long TargetValue, int[] Values)
    {
        public static Input Create(string line)
        {
            var parts = line.Split(": ", 2);

            var targetValue = Convert.ToInt64(parts[0]);

            var values = parts[1].Split(' ').Select(x => Convert.ToInt32(x)).ToArray();

            return new Input(targetValue, values);
        }

        public bool IsValid
        {
            get
            {
                int numberOfOperators = Values.Length - 1;

                var operators = new IEnumerable<Operators>[numberOfOperators];

                for(int i = 0; i < operators.Length; ++i)
                {
                    operators[i] = Enum.GetValues<Operators>();
                }

                var cartesianProduct = operators.CartesianProduct();

                foreach(var ops in cartesianProduct)
                {
                    var result =
                        ops.Zip(Values[1..]).Aggregate((long)Values[0], (acc, opValue) =>
                            opValue.First switch {
                                Operators.Add => acc + opValue.Second,
                                Operators.Multiply => acc * opValue.Second
                            });

                    if(result == TargetValue) { return true; }
                }
                return false;
            }
        }

        public bool IsValid2
        {
            get
            {
                int numberOfOperators = Values.Length - 1;

                var operators = new IEnumerable<Operators2>[numberOfOperators];

                for(int i = 0; i < operators.Length; ++i)
                {
                    operators[i] = Enum.GetValues<Operators2>();
                }

                var cartesianProduct = operators.CartesianProduct();

                foreach(var ops in cartesianProduct)
                {
                    var result =
                        ops.Zip(Values[1..]).Aggregate((long)Values[0], (acc, opValue) =>
                            opValue.First switch {
                                Operators2.Add => acc + opValue.Second,
                                Operators2.Multiply => acc * opValue.Second,
                                Operators2.Concatenate => Convert.ToInt64(acc.ToString() + opValue.Second.ToString())
                            });

                    if(result == TargetValue) { return true; }
                }
                return false;
            }
        }
    }

    public enum Operators
    {
        Add,
        Multiply
    }

    public enum Operators2
    {
        Add,
        Multiply,
        Concatenate
    }
}
