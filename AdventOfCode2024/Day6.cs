using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
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
                '+' => '+',
                _ => throw new InvalidOperationException($"Unknown position character: '{ch}'")
            };
        }
    }

    private record Part2State(StringBuilder[] Area, Coord GuardPosition, Direction Direction, bool FoundLoop = false, bool StopIterating = false);

    public static void Solve(string path)
    {
        var lines = File.ReadAllLines(path);

        var area = MakeArea(lines);

        var moveUp = new Direction(-1, 0, '^');
        var moveDown = new Direction(1, 0, 'v');
        var moveLeft = new Direction(0, -1, '<');
        var moveRight = new Direction(0, 1, '>');

        var movementCycle = MovementDirections().GetEnumerator();

        var (guardPosition, currentDirection) = FindGuardPositionAndOrientation(area, movementCycle);

        var initialGuardPosition = guardPosition;
        var initialDirection = currentDirection;

        do
        {
            //DrawState();
        } while(!Step(area));

        //DrawState(area);
        var part1 = CountOccupiedLocations();

        Console.WriteLine($"Part 1: {part1}");

        int part2 = 0;

        
        for(var row = 0; row < area.Length; ++row)
        {
            for(int col = 0; col < area[row].Length; col++)
            {

                // Try placing an obstacle in each visited position
                if(area[row][col] is '.' or '#' or '^' or '>' or 'v' or '<') {
                    continue;
                }
                if(initialGuardPosition == (row, col)) { continue; }

                var area2 = MakeArea(lines);

                area2[row][col] = 'O';

                var guardPosition2 = initialGuardPosition;

                while(movementCycle.Current != initialDirection) { movementCycle.MoveNext(); }

                var state = new Part2State(area2,
                    new Coord(initialGuardPosition.Item1, initialGuardPosition.Item2),
                    movementCycle.Current);

                do
                {
                    state = Step2(state);

                } while(!state.StopIterating);

                if(state.FoundLoop)
                {
                    part2++;
                }
               
            }
        }

        Console.WriteLine($"Part2: {part2}");


        StringBuilder[] MakeArea(string[] lines)
        {
            return lines.Select(x => new StringBuilder(x)).ToArray();
        }

        ((int, int) position, Direction direction) FindGuardPositionAndOrientation(StringBuilder[] area, IEnumerator<Direction> movementCycle)
        {
            var position = (-1, -1);
            var direction = Direction.Default;

            for(int r = 0; r < lines.Length; r++)
            {
                for(int c = 0; c < lines[r].Length; c++)
                {
                    char ch = lines[r][c];

                    switch(ch)
                    {
                        case '^':
                            position = (r, c);
                            while(movementCycle.Current != moveUp) { movementCycle.MoveNext(); }
                            direction = moveUp;
                            break;
                        case '>':
                            position = (r, c);
                            while(movementCycle.Current != moveRight) { movementCycle.MoveNext(); }
                            direction = moveRight;
                            break;
                        case 'v':
                            position = (r, c);
                            while(movementCycle.Current != moveDown) { movementCycle.MoveNext(); }
                            direction = moveDown;
                            break;
                        case '<':
                            position = (r, c);
                            while(movementCycle.Current != moveLeft) { movementCycle.MoveNext(); }
                            direction = moveLeft;
                            break;
                        default:
                            break;
                    }
                }
            }

            return (position, direction);
        }

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

        bool Step(StringBuilder[] area)
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
                }
                else
                {
                    guardPosition = newGuardPosition;
                }

                return false;
            }
        }

        Part2State Step2(Part2State state)
        {
            var area = state.Area;
            var guardPosition = state.GuardPosition;

            var trailChar = area[guardPosition.row][guardPosition.col] switch
            {
                '.' => '1',
                '1' => '2',
                '2' => '3',
                '3' => '4',
                '4' => '5',
                _ => '?'
            };
            
            area[guardPosition.row][guardPosition.col] = trailChar;

            if(trailChar == '5') // can enter a position from any direction
            {
                //DrawState(state.Area);
                return state with { FoundLoop = true, StopIterating = true };
            }

            
            var newGuardPosition = new Coord(guardPosition.row + state.Direction.deltaRow, 
                                             guardPosition.col + state.Direction.deltaCol);

            if(newGuardPosition.row < 0 || newGuardPosition.row >= lines.Length
                || newGuardPosition.col < 0 || newGuardPosition.col >= lines[0].Length)
            {
                return state with { GuardPosition = newGuardPosition, StopIterating = true };
            }
            else
            {
                char mark = area[newGuardPosition.row][newGuardPosition.col];
                
                return mark switch
                {
                    //'O' => state with { Direction = Turn(movementCycle)},
                    '#' or 'O' => state with { Direction = Turn(movementCycle) },
                    _ => state with { GuardPosition = newGuardPosition }
                };
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

        Direction Turn(IEnumerator<Direction> iterator)
        {
            iterator.MoveNext();
            return iterator.Current;
        }

        
    }

    static void DrawState(StringBuilder[] area)
    {
        foreach(var line in area)
        {
            Console.Error.WriteLine(line);
        }
        Console.Error.WriteLine();

    }
}
