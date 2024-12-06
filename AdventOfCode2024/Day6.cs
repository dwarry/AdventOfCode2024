using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal static class Day6
{
    public static void Solve(string path)
    {
//        var lines = @"....#.....
//.........#
//..........
//..#.......
//.......#..
//..........
//.#..^.....
//........#.
//#.........
//......#...".Split("\r\n");
        var lines = File.ReadAllLines(path);

        var area = lines.Select(x => new StringBuilder(x)).ToArray();

        var moveUp = (-1, 0, '^');
        var moveDown = (1, 0, 'v');
        var moveLeft = (0, -1, '<');
        var moveRight = (0, 1, '>');

        (int, int) guardPosition = (0, 0);
        (int, int, char) currentDirection = (0, 0, ' ');

        var movementCycle = MovementDirections().GetEnumerator();

        for(int r = 0; r < lines.Length; r++)
        {
            for(int c = 0; c < lines[r].Length; c++)
            {
                char ch = lines[r][c];

                switch(ch)
                {
                    case '^':
                        guardPosition = (r, c);
                        while(movementCycle.Current != moveUp) { movementCycle.MoveNext(); }
                        currentDirection = moveUp;
                        break;
                    case '>':
                        guardPosition = (r, c);
                        while(movementCycle.Current != moveRight) { movementCycle.MoveNext(); }
                        currentDirection = moveRight;
                        break;
                    case 'v':
                        guardPosition = (r, c);
                        while(movementCycle.Current != moveDown) { movementCycle.MoveNext(); }
                        currentDirection = moveDown;
                        break;
                    case '<':
                        guardPosition = (r, c);
                        while(movementCycle.Current != moveLeft) { movementCycle.MoveNext(); }
                        currentDirection = moveLeft;
                        break;
                    default:
                        break;
                }
            }
        }

        if(currentDirection == (0, 0, ' '))
        {
            Console.Error.WriteLine("No Guard!");
            return;
        }
        
        do
        {
            //DrawState();
        } while(!Step());

        DrawState();
        var part1 = CountOccupiedLocations();  

        Console.Write($"Part 1: {part1}");

        int CountOccupiedLocations()
        {
            return area.Sum(x => {
                int result = 0;
                for(int i = 0; i < x.Length; i++)
                {
                    if(x[i] == 'X')
                    {
                        result++;
                    }
                }
                return result;
            });
        }

        bool Step()
        {
            area[guardPosition.Item1][guardPosition.Item2] = 'X';
            var newGuardPosition = (guardPosition.Item1 + currentDirection.Item1, guardPosition.Item2 + currentDirection.Item2);



            if(newGuardPosition.Item1 < 0 || newGuardPosition.Item1 >= lines.Length
                || newGuardPosition.Item2 < 0 || newGuardPosition.Item2 >= lines[0].Length)
            {
                return true;
            }
            else
            {
                if(area[newGuardPosition.Item1][newGuardPosition.Item2] == '#')
                {
                    movementCycle.MoveNext();
                    currentDirection = movementCycle.Current;
                    area[guardPosition.Item1][guardPosition.Item2] = currentDirection.Item3;
                }
                else
                {
                    guardPosition = newGuardPosition;
                    area[guardPosition.Item1][guardPosition.Item2] = currentDirection.Item3;
                }

                return false;
            }
        }


        IEnumerable<(int, int, char)> MovementDirections()
        {
            while(true)
            {
                yield return moveUp;
                yield return moveRight;
                yield return moveDown;
                yield return moveLeft;
            }
        }

        void DrawState()
        {
            foreach(var line in area)
            {
                Console.Error.WriteLine(line);
            }
            Console.Error.WriteLine();

        }
    }
}
