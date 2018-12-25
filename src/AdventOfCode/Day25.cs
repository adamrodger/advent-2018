namespace AdventOfCode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MoreLinq;
    using Utilities;

    /// <summary>
    /// Solver for Day 25
    /// </summary>
    public class Day25
    {
        public int Part1(string[] input)
        {
            var points = new List<(int x, int y, int z, int t)>(input.Length);

            foreach (string line in input)
            {
                var numbers = line.Numbers();
                points.Add((numbers[0], numbers[1], numbers[2], numbers[3]));
            }

            List<HashSet<(int x, int y, int z, int t)>> constellations = new List<HashSet<(int, int, int, int)>>();

            foreach ((int x, int y, int z, int t) point in points)
            {
                // find existing constellations, else start new one
                var existing = constellations.Where(
                    c => c.Any(e => Math.Abs(e.x - point.x) + Math.Abs(e.y - point.y) +
                                    Math.Abs(e.z - point.z) + Math.Abs(e.t - point.t) <= 3)).ToHashSet();

                if (!existing.Any())
                {
                    constellations.Add(new HashSet<(int x, int y, int z, int t)> { point });
                }
                else
                {
                    // this point may have just joined multiple existing ones together
                    existing.ForEach(e => constellations.Remove(e));
                    var joined = existing.SelectMany(p => p).ToHashSet();
                    joined.Add(point);
                    constellations.Add(joined);
                }
            }

            // guessed 603 -- too high -- wasn't joining existing ones together
            return constellations.Count;
        }
    }
}
