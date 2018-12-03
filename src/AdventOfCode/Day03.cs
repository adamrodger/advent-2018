using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 3
    /// </summary>
    public class Day03
    {
        private const string InputFile = "inputs/day03.txt";

        /// <summary>
        /// Claims are formatted - #1 @ 469,741: 22x26
        /// </summary>
        public (int overlaps, int nonOverlap) Solve()
        {
            var lines = File.ReadAllLines(InputFile);

            var index = 0;
            var coords = new List<(int x, int y)>();
            var squareCoords = new Dictionary<int, HashSet<(int x, int y)>>(lines.Length);

            foreach (string line in lines)
            {
                // parse
                string[] parts = line.Split(':');
                int[] startCoords = parts[0].Split('@')[1].Trim().Split(',').Select(int.Parse).ToArray();
                int[] sizeCoords = parts[1].Trim().Split('x').Select(int.Parse).ToArray();

                squareCoords[index] = new HashSet<(int x, int y)>();

                // calculate square co-ords
                for (int x = startCoords[0]; x < startCoords[0] + sizeCoords[0]; x++)
                {
                    for (int y = startCoords[1]; y < startCoords[1] + sizeCoords[1]; y++)
                    {
                        var position = (x, y);
                        coords.Add(position);
                        squareCoords[index].Add(position);
                    }
                }

                index++;
            }

            // part 1
            var overlaps = coords.GroupBy(c => c).Count(g => g.Count() > 1);

            // part 2 - find the claim that doesn't overlap with any other
            int nonOverlapIndex = -1;

            for (int i = 0; i < squareCoords.Count; i++)
            {
                var outer = squareCoords[i];
                bool overlapped = false;

                for (int j = 0; j < squareCoords.Count; j++)
                {
                    if (i == j)
                    {
                        // don't compare to self
                        continue;
                    }

                    var inner = squareCoords[j];

                    // TODO: There's got to be a simpler mathematical way to do this just by looking at the corners and subtracting
                    // instead of brute-forcing set-diffing
                    if (inner.Intersect(outer).Any())
                    {
                        overlapped = true;
                        break;
                    }
                }

                if (!overlapped)
                {
                    nonOverlapIndex = i + 1; // claim IDs are 1-indexed not 0-indexed
                    break;
                }
            }

            return (overlaps, nonOverlapIndex);
        }
    }
}
