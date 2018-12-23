using System;

namespace AdventOfCode
{
    using System.Linq;
    using MoreLinq;
    using Utilities;

    /// <summary>
    /// Solver for Day 23
    /// </summary>
    public class Day23
    {
        public int Part1(string[] input)
        {
            var bots = input.Select(i => i.Numbers())
                            .Select(nums => new Nanobot { X = nums[0], Y = nums[1], Z = nums[2], Radius = nums[3] })
                            .ToArray();

            var maxBot = bots.MaxBy(b => b.Radius).First();

            var inRange =
                bots.Where(b => Math.Abs(b.X - maxBot.X) + Math.Abs(b.Y - maxBot.Y) + Math.Abs(b.Z - maxBot.Z) <=
                                maxBot.Radius);

            return inRange.Count();
        }

        public int Part2(string[] input)
        {
            /*
             * Struggled today because I don't know the algorithms, did some reading up:
             *
             * https://blog.mapbox.com/a-dive-into-spatial-search-algorithms-ebd0c5e39d2a
             * https://en.wikipedia.org/wiki/Nearest_neighbor_search
             * https://en.wikipedia.org/wiki/K-d_tree
             * https://en.wikipedia.org/wiki/Ball_tree
             */

            var bots = input.Select(i => i.Numbers())
                            .Select(nums => new Nanobot { X = nums[0], Y = nums[1], Z = nums[2], Radius = nums[3] })
                            .ToArray();

            int minX = bots.Min(b => b.X);
            int maxX = bots.Max(b => b.X);
            int minY = bots.Min(b => b.Y);
            int maxY = bots.Max(b => b.Y);
            int minZ = bots.Min(b => b.Z);
            int maxZ = bots.Max(b => b.Z);

            (int cx, int cy, int cz, int count) best = (0, 0, 0, 0);
            bool bestChanged;

            do
            {
                bestChanged = false;

                // start with a really big 3d box and then gradually 'zoom in' by making the box smaller centered around the current best point
                foreach (int x in Enumerable.Range(minX, maxX - minX))
                foreach (int y in Enumerable.Range(minY, maxY - minY))
                foreach (int z in Enumerable.Range(minZ, maxZ - minZ))
                {
                    // find bots in range of this point
                    var inRange = bots.Count(b => Math.Abs(b.X - x) + Math.Abs(b.Y - y) + Math.Abs(b.Z - z) <= b.Radius);

                    if (inRange > best.count)
                    {
                        // most we've found so far
                        best = (x, y, z, inRange);
                        bestChanged = true;
                    }
                    else if (inRange == best.count && Math.Abs(best.cx) + Math.Abs(best.cy) + Math.Abs(best.cz) > Math.Abs(x) + Math.Abs(y) + Math.Abs(z))
                    {
                        // new one has same number of bots in range but is closer to 0,0,0
                        best = (x, y, z, inRange);
                        bestChanged = true;
                    }
                }

                // we've found the best point in this big box, now halve the box size and centre around the best point
                // +/- 1 means best.x/y/z is in the middle
                minX = (best.cx - 1) * 2;
                maxX = (best.cx + 1) * 2;
                minY = (best.cy - 1) * 2;
                maxY = (best.cy + 1) * 2;
                minZ = (best.cz - 1) * 2;
                maxZ = (best.cz + 1) * 2;
            }
            while (bestChanged);

            return Math.Abs(best.cx) + Math.Abs(best.cy) + Math.Abs(best.cz);
        }
    }

    public class Nanobot
    {
        public int X { get; set;}
        public int Y { get; set; }
        public int Z { get; set; }
        public int Radius { get; set; }
    }
}
