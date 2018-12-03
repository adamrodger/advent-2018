using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 1
    /// </summary>
    public class Day01
    {
        public int Part1()
        {
            int[] elements = File.ReadLines("inputs/day01.txt").Select(int.Parse).ToArray();
            return elements.Sum();
        }

        public int Part2()
        {
            int[] elements = File.ReadLines("inputs/day01.txt").Select(int.Parse).ToArray();

            var seen = new HashSet<int>();
            int frequency = elements[0];
            seen.Add(frequency);

            while (true)
            {
                frequency += elements[seen.Count % elements.Length];

                if (seen.Contains(frequency))
                {
                    return frequency;
                }

                seen.Add(frequency);
            }
        }
    }
}
