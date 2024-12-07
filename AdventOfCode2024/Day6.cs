using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024;
internal static class Day6
{
    private record Coord(int row, int col);

    private record Direction(int deltaRow, int deltaCol, char guard)
    {
        public static readonly Direction Default = new(0, 0, ' ');

        public char GetTrailChar(char ch)
        {
            return ch switch
            {
                '.' or '^' or '>' or 'v' or '<' => deltaRow != 0 ? '|' : '-',
                '|' => deltaRow != 0 ? '|' : '+',
                '-' => deltaRow != 0 ? '+' : '-',
                _ => throw new InvalidOperationException($"Unknown position character: '{ch}'")
            };
        }
    }

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

        var moveUp = new Direction(-1, 0, '^');
        var moveDown = new Direction(1, 0, 'v');
        var moveLeft = new Direction(0, -1, '<');
        var moveRight = new Direction(0, 1, '>');

        (int, int) guardPosition = (0, 0);
        var currentDirection = Direction.Default;

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

        if(currentDirection == Direction.Default)
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
                    if(x[i] is '|' or '-' or '+')
                    {
                        result++;
                    }
                }
                return result;
            });
        }

        bool Step()
        {
            area[guardPosition.Item1][guardPosition.Item2] = currentDirection.GetTrailChar(area[guardPosition.Item1][guardPosition.Item2]);
            var newGuardPosition = (guardPosition.Item1 + currentDirection.deltaRow, guardPosition.Item2 + currentDirection.deltaCol);

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
                    //area[guardPosition.Item1][guardPosition.Item2] = currentDirection.guard;
                }
                else
                {
                    guardPosition = newGuardPosition;
                    //area[guardPosition.Item1][guardPosition.Item2] = currentDirection.guard;
                }

                return false;
            }
        }


        IEnumerable<Direction> MovementDirections()
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
