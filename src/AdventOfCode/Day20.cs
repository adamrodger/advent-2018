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

        /// <summary>
        /// Solve for the given input
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>
        /// Part 1 - Shortest path to the furthest away point
        /// Part 2 - Number of paths >= 1000 steps long
        /// </returns>
        public (int part1, int part2) Solve(string input)
        {
            (int x, int y) location = (0, 0);

            // collection of locations and the doors out of that location
            var rooms = new Dictionary<(int x, int y), HashSet<(int x, int y)>>
            {
                [location] = new HashSet<(int x, int y)>()
            };

            // keep track of where we've visited so we don't re-visit rooms
            var seen = new HashSet<(int x, int y)> { location };

            this.Search(input, location, seen, rooms);

            // we've now got a big list of every room and the doors out of that room - follow every possible path in a BFS (like day 15)
            var lengths = new Dictionary<(int x, int y), int> { [(0, 0)] = 0 };
            var queue = new Queue<(int x, int y)>();
            queue.Enqueue((0, 0));

            while (queue.Any())
            {
                var previous = queue.Dequeue();
                var adjacent = rooms[previous].Where(door => !lengths.ContainsKey(door)); // don't revisit

                // walk outwards through every door and queue it to be checked for more walking outwards
                foreach (var next in adjacent)
                {
                    lengths[next] = lengths[previous] + 1;
                    queue.Enqueue(next);
                }
            }

            return (lengths.Max(kvp => kvp.Value), lengths.Count(kvp => kvp.Value >= 1000));
        }

        /// <summary>
        /// For the given input, follow the path from the given index and location, noting which doors can exit from that location
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="location">Current location</param>
        /// <param name="seen">Locations that have already been visited</param>
        /// <param name="rooms">Mapping of location to destinations from that location to populate</param>
        public void Search(string input, (int x, int y) location, HashSet<(int x, int y)> seen, Dictionary<(int x, int y), HashSet<(int x, int y)>> rooms)
        {
            for(int i = 0; i < input.Length; i++)
            {
                char direction = input[i];

                if (direction == '^') // start
                {
                    continue;
                }

                if (direction == '$') // end
                {
                    return;
                }

                if (direction == '(') // found the start of a branch, recurse down one level
                {
                    (int close, var options) = this.ParseBranchOptions(input, i);

                    // follow the branches
                    foreach (string option in options)
                    {
                        this.Search(option, location, seen, rooms);
                    }

                    // skip to the end of the branch point
                    i = close;
                }
                else if (Compass.ContainsKey(direction)) // found a door, follow it
                {
                    var newLocation = (location.x + Compass[direction].dx, location.y + Compass[direction].dy);

                    if (seen.Contains(newLocation))
                    {
                        // no point following existing paths
                        return;
                    }

                    seen.Add(newLocation);

                    // add a door between the old and new locations
                    rooms[location].Add(newLocation);
                    rooms[newLocation] = new HashSet<(int x, int y)> { location };

                    location = newLocation;
                }
            }
        }

        /// <summary>
        /// From the given index in the input, parse the options available in the 'next' branch. Branches can contain nested branches
        /// but they will be returned as options of the 'next' branch rather than deconstructed. This allows a recursive descent into
        /// the branches.
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="index">Start index. Note this MUST be the index of an opening bracket for this branch</param>
        /// <returns>Branch ending point and branch options</returns>
        private (int branchClose, ICollection<string> options) ParseBranchOptions(string input, int index)
        {
            // find closing bracket
            int close = FindClosingBracket(input, index);

            var options = new List<string>();
            string option = string.Empty;

            for (int i = index + 1; i < close; i++)
            {
                if (input[i] == '|')
                {
                    options.Add(option);
                    option = string.Empty;
                }
                else if (input[i] == '(')
                {
                    // closing bracket
                    int nestedClose = FindClosingBracket(input, i + 1);

                    // nested branch
                    option += input.Substring(i, nestedClose - i + 1);
                    i = nestedClose;
                }
                else
                {
                    option += input[i];
                }
            }

            // add the final option (after the last pipe)
            if (option != string.Empty)
            {
                options.Add(option);
            }

            return (close, options);
        }

        /// <summary>
        /// From an opening bracket, find the matching closing bracket (i.e. skip over nested brackets)
        /// </summary>
        /// <param name="input">Input</param>
        /// <param name="index">Start index</param>
        /// <returns>Index of the matching closing bracket</returns>
        private static int FindClosingBracket(string input, int index)
        {
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

            return close;
        }
    }
}
