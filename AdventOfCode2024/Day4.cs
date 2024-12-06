using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal class Day4
{
    public static void Solve(string path)
    {
        string[] lines = File.ReadAllLines(path);

        var testString = new char[4];
        
        int part1 = 0, part2 = 0;

        for(int row = 0; row < lines.Length; row++)
        {
            for(int col = 0; col < lines[row].Length; col++)
            {
                GetTestString(testString, row, col, 0, 1); // horizontal
                CheckIsXmas(testString);

                GetTestString(testString, row, col, 1, 0); // vertical
                CheckIsXmas(testString);

                GetTestString(testString, row, col, 1, 1); // left-to-right diagonal
                CheckIsXmas(testString);

                GetTestString(testString, row, col, 1, -1); // right-to-left diagonal
                CheckIsXmas(testString);
            }
        }

        Console.WriteLine($"Part 1: {part1}");

        char[] diag1 = new char[3];
        char[] diag2 = new char[3];

        for(int row = 0; row < lines.Length; row++)
        {
            for(int col = 0; col < lines[row].Length; col++)
            {
                
                GetTestString(diag1, row, col, 1, 1); // left-to-right diagonal
                GetTestString(diag2, row, col + 2, 1, -1); // right-to-left diagonal

                if(CheckIsMAS(diag1) && CheckIsMAS(diag2))
                {
                    part2++;
                }
            }
        }

        Console.WriteLine($"Part 2: {part2}");


        void CheckIsXmas(IEnumerable<char> test)
        {
            if("XMAS".Zip(test).All(tup => tup.First == tup.Second)
                || "SAMX".Zip(test).All(tup => tup.First == tup.Second))
            {
                part1++;
            }
        }

        bool CheckIsMAS(IEnumerable<char> test)
        {
            return "MAS".Zip(test).All(tup => tup.First == tup.Second)
                || "SAM".Zip(test).All(tup => tup.First == tup.Second);
        }

        void GetTestString(char[] result, int startRow, int startCol, int deltaRow, int deltaCol)
        {
            int row = startRow, col = startCol;

            for(int i = 0; i < result.Length; i++)
            {
                if(row >= 0 && row < lines.Length 
                   && col >= 0 && col < lines[i].Length)
                {
                    result[i] = lines[row][col];
                }
                else
                {
                    result[i] = ' ';
                }
                row += deltaRow;
                col += deltaCol;
            }
        }
    }

}
