using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 20
    /// </summary>
    public class Day20
    {
        public static readonly Dictionary<char, (int dx, int dy)> Compass = new Dictionary<char, (int dx, int dy)>
        {
            ['N'] = (0, -1),
            ['S'] = (0, 1),
            ['E'] = (1, 0),
            ['W'] = (-1, 0)
        };

        public int Part1(string input)
        {
            (int x, int y) location = (0, 0);

            var paths = new Dictionary<(int x, int y), Queue<(int x, int y)>>
            {
                [location] = new Queue<(int x, int y)>(0)
            };

            var seen = new HashSet<(int x, int y)> { location };

            this.Search(input, 1, location, seen, 0, paths);

            return paths.Max(kvp => kvp.Value.Count);
        }

        public void Search(string input, int index, (int x, int y) location, HashSet<(int x, int y)> seen, int depth, Dictionary<(int x, int y), Queue<(int x, int y)>> paths)
        {
            char direction = input[index];

            if (direction == '$')
            {
                return;
            }

            // found a branch
            if (direction == '(')
            {
                // find closing bracket
                int openBrackets = 1;
                int close = index;

                while (openBrackets > 0)
                {
                    close++;

                    if (input[close] == '(')
                    {
                        openBrackets++;
                    }

                    if (input[close] == ')')
                    {
                        openBrackets--;
                    }
                }

                // start branch
                var branches = input.Substring(index + 1, close - index - 1)
                                    .Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                                    .ToArray();

                // follow the branches
                foreach (string branch in branches)
                {
                    this.Search(branch, 0, location, seen, depth, paths);
                }

                // advance the index until after the processed branch
                index = close + 1;
                direction = input[index];
            }

            // just found a straight path
            if (Compass.ContainsKey(direction))
            {
                // move
                var newLocation = (location.x + Compass[direction].dx, location.y + Compass[direction].dy);

                if (seen.Contains(newLocation))
                {
                    // can't form circles, so if co-ordinates are in the seen list already this next step is invalid
                    return;
                }

                seen.Add(newLocation);

                // add onto the end of the path from the previous location
                Queue<(int x, int y)> currentPath = paths[location];
                paths[newLocation] = new Queue<(int x, int y)>(currentPath.Append(newLocation).ToArray());

                // follow the path
                this.Search(input, index + 1, newLocation, seen, depth + 1, paths);
            }
        }

        public int Part2(string input)
        {

            return 0;
        }
    }
}
