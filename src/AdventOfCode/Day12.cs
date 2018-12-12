using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 12
    /// </summary>
    public class Day12
    {
        private const int PotsSize = 600;

        public int Part1(string[] input)
        {
            int zeroIndex = PotsSize / 2;
            int generations = 20;

            /*
             * initial state: ##..#.#.#..##..#..##..##..#.#....#.....##.#########...#.#..#..#....#.###.###....#..........###.#.#..
             *
             * ..##. => .
             */
             
            // initialise pots
            bool[] initial = input[0].Substring(15).Select(c => c == '#').ToArray();
            bool[] currentState = new bool[PotsSize];
            Array.Copy(initial, 0, currentState, PotsSize / 2, initial.Length);

            // create rules
            var rules = new Dictionary<(bool, bool, bool, bool, bool), bool>();

            foreach (string line in input.Skip(2).Where(l => l[9] == '#'))
            {
                ValueTuple<bool, bool, bool, bool, bool> rule = (line[0] == '#', line[1] == '#', line[2] == '#', line[3] == '#', line[4] == '#');
                rules[rule] = line[9] == '#';
            }

            foreach(int generation in Enumerable.Range(1, generations))
            {
                int first = Array.IndexOf(currentState, true);

                if (first == -1)
                {
                    first = PotsSize / 2;
                }

                int last = Array.LastIndexOf(currentState, true);

                if (last == -1)
                {
                    last = (PotsSize / 2) + initial.Length;
                }

                // allow room for the infinite empty pots
                first -= 4;
                last += 4;

                bool[] nextState = new bool[PotsSize];

                for (int i = first; i <= last; i++)
                {
                    var current = (currentState[i - 2], currentState[i - 1], currentState[i], currentState[i + 1], currentState[i + 2]);
                    rules.TryGetValue(current, out bool nextValue);
                    nextState[i] = nextValue;
                }

                currentState = nextState;
            }

            int sum = 0;

            for (int i = 0; i < currentState.Length; i++)
            {
                if (currentState[i])
                {
                    sum += (i - zeroIndex);
                }
            }

            return sum;
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
}
