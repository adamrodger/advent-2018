using System;

namespace AdventOfCode
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Solver for Day 16
    /// </summary>
    public class Day16
    {
        public int Part1(string[] input)
        {
            List<Transition> transitions = new List<Transition>();

            for (int i = 0; i < input.Length; i += 4)
            {
                string[] slice = input.Skip(i).Take(4).ToArray();

                if (slice[0] == string.Empty)
                {
                    break;
                }

                var transition = new Transition(slice);
                transitions.Add(transition);
            }

            var processor = new Processor();

            int part1 = transitions.Count(transition => processor.Train(transition) >= 3);

            var map = new Dictionary<int, string>();

            /*foreach (KeyValuePair<int, HashSet<string>> candidate in processor.Candidates)
            {
                Debug.WriteLine($"{candidate.Key}: {string.Join(", ", candidate.Value)}");
            }*/

            // boil down the candidates list
            while (processor.Candidates.Any())
            {
                var unique = processor.Candidates.First(c => c.Value.Count == 1);
                string instruction = unique.Value.First();
                map[unique.Key] = instruction;
                processor.Candidates.Remove(unique.Key);

                // eliminate from others
                foreach (KeyValuePair<int, HashSet<string>> pair in processor.Candidates)
                {
                    pair.Value.Remove(instruction);
                }
            }

            /*foreach (KeyValuePair<int, string> candidate in map)
            {
                Debug.WriteLine($"{candidate.Key}: {candidate.Value}");
            }*/

            var current = transitions.Last().After;

            for (int i = 3146; i < input.Length; i++)
            {
                var instruction = input[i].Split(' ').Select(int.Parse).ToArray();
                current = processor.Execute(current, instruction, map);
            }

            return part1;
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

    public class Processor
    {
        private readonly ICollection<string> simple = new[] { "ad", "mu", "ba", "bo" };

        private readonly string[] instructions =
        {
            "addr",
            "addi",
            "mulr",
            "muli",
            "banr",
            "bani",
            "borr",
            "bori",
            "setr",
            "seti",
            "gtir",
            "gtri",
            "gtrr",
            "eqir",
            "eqri",
            "eqrr"
        };

        public Dictionary<int, HashSet<string>> Candidates { get; } = new Dictionary<int, HashSet<string>>
        {
            [0] = new HashSet<string>(),
            [1] = new HashSet<string>(),
            [2] = new HashSet<string>(),
            [3] = new HashSet<string>(),
            [4] = new HashSet<string>(),
            [5] = new HashSet<string>(),
            [6] = new HashSet<string>(),
            [7] = new HashSet<string>(),
            [8] = new HashSet<string>(),
            [9] = new HashSet<string>(),
            [10] = new HashSet<string>(),
            [11] = new HashSet<string>(),
            [12] = new HashSet<string>(),
            [13] = new HashSet<string>(),
            [14] = new HashSet<string>(),
            [15] = new HashSet<string>()
        };

        public int Train(Transition transition)
        {
            var matches = new List<string>();
            
            foreach (var instruction in this.instructions)
            {
                int[] output = this.Execute(transition.Before, transition.Operation, instruction);

                // check for match
                bool match = true;

                for (int i = 0; i < transition.After.Length; i++)
                {
                    if (output[i] != transition.After[i])
                    {
                        match = false;
                        break;
                    }
                }
                
                if (match)
                {
                    matches.Add(instruction);
                    this.Candidates[transition.Operation[0]].Add(instruction);
                }
            }

            return matches.Count;
        }

        public int[] Execute(int[] input, int[] operation, Dictionary<int, string> instructionMap)
        {
            string instruction = instructionMap[operation[0]];
            return this.Execute(input, operation, instruction);
        }

        private int[] Execute(int[] input, int[] operation, string instruction)
        {
            int result;
            string opcode = instruction.Substring(0, 2);

            // set a and b to literals
            int a = operation[1];
            int b = operation[2];
            int c = operation[3];

            // check if A and B should refer to registers
            if (this.simple.Contains(opcode) || // A is always a register for add/mul/ban/bor
                (opcode == "se" && instruction[3] == 'r') || // set uses the 4th character, not the third
                (opcode != "se" && instruction[2] != 'i')) // gt and eq use 3rd character
            {
                // look up the register
                a = input[a];
            }

            if (instruction[3] != 'i')
            {
                // look up the register
                b = input[b];
            }

            // perform the operation
            switch (opcode)
            {
                case "ad":
                    result = a + b;
                    break;
                case "mu":
                    result = a * b;
                    break;
                case "ba":
                    result = a & b;
                    break;
                case "bo":
                    result = a | b;
                    break;
                case "se":
                    b = -1;
                    result = a;
                    break;
                case "gt":
                    result = a > b ? 1 : 0;
                    break;
                case "eq":
                    result = a == b ? 1 : 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // set C register
            int[] output = new int[input.Length];
            input.CopyTo(output, 0);
            output[c] = result;
            return output;
        }
    }

    public class Transition
    {
        public int[] Before { get; set; }

        public int[] Operation { get; set; }

        public int[] After { get; set; }

        public Transition(string[] input)
        {
            /*
             * Before: [2, 0, 1, 1]
             * 3 0 3 3
             * After:  [2, 0, 1, 1]
             */

            this.Before = input[0].Substring(9, 10).Split(',').Select(int.Parse).ToArray();
            this.Operation = input[1].Split(' ').Select(int.Parse).ToArray();
            this.After = input[2].Substring(9, 10).Split(',').Select(int.Parse).ToArray();
        }
    }
}
