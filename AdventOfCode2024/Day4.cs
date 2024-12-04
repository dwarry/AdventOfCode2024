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
//        string[] lines = @"MMMSXXMASM
//MSAMXMSMSA
//AMXSXMAAMM
//MSAMASMSMX
//XMASAMXAMM
//XXAMMXXAMA
//SMSMSASXSS
//SAXAMASAAA
//MAMMMXMMMM
//MXMXAXMASX".Split("\r\n");
        
        string[] lines = File.ReadAllLines(path);

        var testString = new char[4];
        
        var xmasCount = 0;

        for(int row = 0; row < lines.Length; row++)
        {
            for(int col = 0; col < lines[row].Length; col++)
            {
                GetTestString(row, col, 0, 1); // horizontal
                CheckIsXmas();

                GetTestString(row, col, 1, 0); // vertical
                CheckIsXmas();

                GetTestString(row, col, 1, 1); // left-to-right diagonal
                CheckIsXmas();

                GetTestString(row, col, 1, -1); // right-to-left diagonal
                CheckIsXmas();
            }
        }

        Console.WriteLine($"Part 1: {xmasCount}");

        void CheckIsXmas()
        {
            if( testString.Zip("XMAS").All(tup => tup.First == tup.Second)
                || testString.Zip("SAMX").All(tup => tup.First == tup.Second))
            {
                xmasCount++;
            }
        }

        

        void GetTestString(int startRow, int startCol, int deltaRow, int deltaCol)
        {
            int row = startRow, col = startCol;

            for(int i = 0; i < 4; i++)
            {
                if(row >= 0 &&
                   row < lines.Length &&
                   col >= 0 &&
                   col < lines[i].Length)
                {
                    testString[i] = lines[row][col];
                }
                else
                {
                    testString[i] = ' ';
                }
                row += deltaRow;
                col += deltaCol;
            }
        }
    }

}
