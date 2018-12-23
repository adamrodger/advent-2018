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

            int boxSize = (maxX - minX) * 2; // start with a huuuuuge box
            int bestDistance = -1;

            // start with a really big box and then gradually shrink the box until it's only 1 square in size
            while (boxSize >= 1)
            {
                // argh - left this outside the loop originally so it wasn't zeroing in on the best smaller box
                (int x, int y, int z, int count) best = (0, 0, 0, 0);

                for (int x = minX; x < maxX + 1; x += boxSize)
                for (int y = minY; y < maxY + 1; y += boxSize)
                for (int z = minZ; z < maxZ + 1; z += boxSize) // this effectively cuts the size of the entire space into smaller and smaller boxes
                {
                    // find bots in range of this box
                    var inRange = bots.Count(b => (Math.Abs(b.X - x) + Math.Abs(b.Y - y) + Math.Abs(b.Z - z) - b.Radius) / boxSize <= 0);

                    if (inRange > best.count)
                    {
                        // most we've found so far
                        best = (x, y, z, inRange);
                        bestDistance = Math.Abs(best.x) + Math.Abs(best.y) + Math.Abs(best.z);
                    }
                    else if (inRange == best.count && bestDistance > Math.Abs(x) + Math.Abs(y) + Math.Abs(z))
                    {
                        // new box has same number of bots in range but is closer to 0,0,0
                        best = (x, y, z, inRange);
                        bestDistance = Math.Abs(best.x) + Math.Abs(best.y) + Math.Abs(best.z);
                    }
                }

                // we've found the best box after splitting up the big box, now halve the box size and try the smaller boxes
                // +/- 1 means best.x/y/z is in the middle
                boxSize /= 2;
                minX = best.x - 1 - boxSize;
                maxX = best.x + 1 + boxSize;
                minY = best.y - 1 - boxSize;
                maxY = best.y + 1 + boxSize;
                minZ = best.z - 1 - boxSize;
                maxZ = best.z + 1 + boxSize;
            }

            // when box size gets down to 1, we've found a single best point
            return bestDistance;

            // guessed 121493970 -- too low
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
