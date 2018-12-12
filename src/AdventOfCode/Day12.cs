using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 12
    /// </summary>
    public class Day12
    {
        private const int PotsSize = 600;

        // the array of pots has 0 in the middle
        private static readonly int ZeroIndex = PotsSize / 2;

        /// <summary>
        /// What is the sum of the indices of all populated pots after 20 generations?
        /// </summary>
        public long Part1(string[] input)
        {
            bool[] currentState = ParseInitialState(input);
            Dictionary<(bool, bool, bool, bool, bool), bool> rules = ParseRules(input);

            foreach (int generation in Enumerable.Range(1, 20))
            {
                currentState = AdvanceGeneration(currentState, rules);
            }

            var sum = SumPots(currentState);

            return sum;
        }

        /// <summary>
        /// What is the sum of the indices of all populated pots after 50,000,000,000 generations?
        /// 
        /// Solution: Watch the sums until it stabilises on a constant increment every iteration. On my input, this is 23 so
        /// you can multiply out to get to the 50bn-th iteration
        ///
        /// I have no idea why it stabilises, just found that it does by watching the debugger for a few hundred iterations of part 1!
        /// </summary>
        public long Part2(string[] input)
        {
            bool[] currentState = ParseInitialState(input);
            Dictionary<(bool, bool, bool, bool, bool), bool> rules = ParseRules(input);

            int generation = 0;
            long previousSum = 0;
            long[] diffs = new long[5]; // keep the last n diffs

            while (true)
            {
                currentState = AdvanceGeneration(currentState, rules);
                long sum = SumPots(currentState);

                // calculate the diff to the previous generation
                diffs[generation % diffs.Length] = sum - previousSum;
                previousSum = sum;

                // stop when the increment becomes constant
                if (diffs.All(d => d == diffs[0]))
                {
                    break;
                }

                generation++;
            }

            // extrapolate the remaining generations
            long remaining = 50000000000 - generation - 1;
            long extrapolated = (remaining * diffs[0]) + previousSum;

            return extrapolated;
        }

        /// <summary>
        /// Parse the initial state of the pots
        /// </summary>
        /// <param name="input">Input lines</param>
        /// <returns>Initial state</returns>
        private static bool[] ParseInitialState(string[] input)
        {
            // initial state: ##..#.#.#..##..#..##..##..#.#....#.....##.#########...#.#..#..#....#.###.###....#..........###.#.#..

            bool[] initial = input[0].Substring(15).Select(c => c == '#').ToArray();
            bool[] currentState = new bool[PotsSize];
            Array.Copy(initial, 0, currentState, PotsSize / 2, initial.Length);

            return currentState;
        }

        /// <summary>
        /// Parse the rules, returning only those which result in a plant being populated
        /// </summary>
        /// <param name="input">Input lines</param>
        /// <returns>Rules</returns>
        private static Dictionary<(bool, bool, bool, bool, bool), bool> ParseRules(string[] input)
        {
            var rules = new Dictionary<(bool, bool, bool, bool, bool), bool>();

            // ..##. => .
            foreach (string line in input.Skip(2).Where(l => l[9] == '#'))
            {
                var rule = (line[0] == '#', line[1] == '#', line[2] == '#', line[3] == '#', line[4] == '#');
                rules[rule] = true;
            }

            return rules;
        }

        /// <summary>
        /// Produce the next generation from the current generation using the given rules
        /// </summary>
        /// <param name="currentState">Current generation state</param>
        /// <param name="rules">Mutation rules</param>
        /// <returns>Next generation state</returns>
        private static bool[] AdvanceGeneration(bool[] currentState, Dictionary<(bool, bool, bool, bool, bool), bool> rules)
        {
            int first = Array.IndexOf(currentState, true);
            int last = Array.LastIndexOf(currentState, true);

            // allow room for the infinite empty pots
            // Needs to be +/- 4 to match the rules ....# and #....
            first -= 4;
            last += 4;

            bool[] nextState = new bool[PotsSize];

            for (int i = first; i <= last; i++)
            {
                var current = (currentState[i - 2], currentState[i - 1], currentState[i], currentState[i + 1], currentState[i + 2]);
                rules.TryGetValue(current, out bool nextValue);
                nextState[i] = nextValue;
            }

            return nextState;
        }

        /// <summary>
        /// Sum the indices of all pots currently containing a plant
        /// </summary>
        /// <param name="currentState">Current state of the pots</param>
        /// <returns>Sum of the populated pot indices</returns>
        private static long SumPots(bool[] currentState)
        {
            int sum = 0;

            for (int i = 0; i < currentState.Length; i++)
            {
                if (currentState[i])
                {
                    sum += i - ZeroIndex;
                }
            }

            return sum;
        }
    }
}
