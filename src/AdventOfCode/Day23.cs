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
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
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
