using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 06
    /// </summary>
    public class Day06
    {
        public int Part1(string[] input)
        {
            (List<Location> locations, int xMax, int yMax) = BuildLocations(input);

            // check the Manhattan distances
            for (int y = 0; y <= yMax + 1; y++)
            {
                for (int x = 0; x <= xMax + 1; x++)
                {
                    var closest = locations.MinBy(l => Math.Abs(x - l.X) + Math.Abs(y - l.Y));

                    if (closest.Count() == 1)
                    {
                        Location nearest = closest.First();
                        nearest.Count++;

                        if (x == 0 || x == xMax || y == 0 || y == yMax)
                        {
                            // if it's on an edge then it must be infinite because otherwise it would stretch out forever
                            nearest.Infinite = true;
                        }
                    }
                }
            }

            return locations.Where(l => !l.Infinite).Max(l => l.Count);
        }

        public int Part2(string[] input, int maxRegion)
        {
            (List<Location> locations, int xMax, int yMax) = BuildLocations(input);

            int regionSize = 0;

            for (int y = 0; y <= yMax + 1; y++)
            {
                for (int x = 0; x <= xMax + 1; x++)
                {
                    var distances = locations.Select(l => Math.Abs(x - l.X) + Math.Abs(y - l.Y));
                    int totalDistance = distances.Sum();

                    if (totalDistance < maxRegion)
                    {
                        regionSize++;
                    }
                }
            }

            return regionSize;
        }

        private static (List<Location>, int xMax, int yMax) BuildLocations(string[] input)
        {
            var locations = new List<Location>();

            // get marked locations
            for (int i = 0; i < input.Length; i++)
            {
                string line = input[i];

                locations.Add(new Location
                {
                    Id = (char)(i + 65),
                    X = int.Parse(line.Split(',')[0]),
                    Y = int.Parse(line.Split(',')[1].Trim()),
                    Count = 0
                });
            }

            int xMax = locations.Max(l => l.X);
            int yMax = locations.Max(l => l.Y);

            return (locations, xMax, yMax);
        }

        public class Location
        {
            public char Id { get; set; }

            public int X { get; set; }

            public int Y { get; set; }

            public int Count { get; set; }

            public bool Infinite { get; set; }

            /// <summary>Returns a string that represents the current object.</summary>
            /// <returns>A string that represents the current object.</returns>
            public override string ToString()
            {
                return $"{Id} : ({X}, {Y}) = {Count}, Infinite: {this.Infinite}";
            }
        }
    }
}
