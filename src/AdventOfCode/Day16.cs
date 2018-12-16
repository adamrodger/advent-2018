using System;

namespace AdventOfCode
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using MoreLinq;

    /// <summary>
    /// Solver for Day 16
    /// </summary>
    public class Day16
    {
        public int Part1(string[] input)
        {
            /*var instructions = input.Batch(4)
                                    .TakeUntil(i => i.ElementAt(1).Trim() == string.Empty) // start of example program
                                    .Select(i => new Transition(i.ToArray()))
                                    .ToArray();*/

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

            var operations = new Operations();

            return transitions.Count(transition => operations.MatchingOperations(transition) >= 3);
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

    public class Operations
    {
        /*private Dictionary<string, Func<int[], int[], int[]>> instructions;

        public Operations()
        {
            this.instructions = new Dictionary<string, Func<int[], int[], int[]>>
            {
                ["addr"] = this.AddR,
                ["addi"] = this.AddI,
                ["mulr"] = this.MulR,
                ["muli"] = this.MulI,
                ["banr"] = this.BanR,
                ["bani"] = this.BanI,
                ["borr"] = this.BorR,
                ["bori"] = this.BorI,
                ["setr"] = this.SetR,
                ["seti"] = this.SetI,
                ["gtir"] = this.GtIR,
                ["gtri"] = this.GtRI,
                ["gtrr"] = this.GtRR,
                ["eqir"] = this.EqIR,
                ["eqri"] = this.EqRI,
                ["eqrr"] = this.EqRR
            };
        }*/

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

        public int MatchingOperations(Transition transition)
        {
            var simple = new[] { "ad", "mu", "ba", "bo" };

            int matches = 0;

            Debug.WriteLine(string.Join(", ", transition.After));
            Debug.WriteLine(string.Empty);

            foreach (var instruction in this.instructions)
            {
                int result;
                string opcode = instruction.Substring(0, 2);

                // set a and b to literals
                int a = transition.Operation[1];
                int b = transition.Operation[2];
                int c = transition.Operation[3];

                // check if A and B should refer to registers
                if (simple.Contains(opcode) || // A is always a register for add/mul/ban/bor
                    (opcode == "se" && instruction[3] == 'r') || // set uses the 4th character, not the third
                    (opcode != "se" && instruction[2] != 'i')) // gt and eq use 3rd character
                {
                    // look up the register
                    a = transition.Before[a];
                }

                if (instruction[3] != 'i')
                {
                    // look up the register
                    b = transition.Before[b];
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

                Debug.WriteLine($"{opcode} {a} {b} = {result}");

                // set C register
                int[] output = new int[transition.Before.Length];
                transition.Before.CopyTo(output, 0);
                output[c] = result;

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

                Debug.WriteLine(instruction + " = " + string.Join(", ", output) + " " + (match ? "MATCH" : ""));

                if (match)
                {
                    matches++;
                }
            }

            return matches;
        }

        /*private int[] AddR(int[] instruction, int[] input)
        {
            int[] output = new int[input.Length];
            input.CopyTo(output, 0);

            output[instruction[3]] = input[instruction[1]] + input[instruction[2]];

            return output;
        }

        private int[] AddI(int[] instruction, int[] input)
        {
            int[] output = new int[input.Length];
            input.CopyTo(output, 0);

            output[input[instruction[3]]] = input[instruction[1]] + instruction[2];

            return output;
        }

        private int[] MulR(int[] instruction, int[] input)
        {
            int[] output = new int[input.Length];
            input.CopyTo(output, 0);

            output[instruction[3]] = input[instruction[1]] * input[instruction[2]];

            return output;
        }

        private int[] MulI(int[] instruction, int[] input)
        {
            int[] output = new int[input.Length];
            input.CopyTo(output, 0);

            output[input[instruction[3]]] = input[instruction[1]] * instruction[2];

            return output;
        }

        private int[] BanR(int[] instruction, int[] input)
        {
            int[] output = new int[input.Length];
            input.CopyTo(output, 0);

            output[instruction[3]] = input[instruction[1]] & input[instruction[2]];

            return output;
        }

        private int[] BanI(int[] instruction, int[] input)
        {
            int[] output = new int[input.Length];
            input.CopyTo(output, 0);

            output[input[instruction[3]]] = input[instruction[1]] & instruction[2];

            return output;
        }

        private int[] BorR(int[] instruction, int[] input)
        {
            int[] output = new int[input.Length];
            input.CopyTo(output, 0);

            output[instruction[3]] = input[instruction[1]] | input[instruction[2]];

            return output;
        }

        private int[] BorI(int[] instruction, int[] input)
        {
            int[] output = new int[input.Length];
            input.CopyTo(output, 0);

            output[input[instruction[3]]] = input[instruction[1]] | instruction[2];

            return output;
        }

        private int[] SetR(int[] instruction, int[] input)
        {
            int[] output = new int[input.Length];
            input.CopyTo(output, 0);

            output[instruction[3]] = input[instruction[1]];

            return output;
        }

        private int[] SetI(int[] instruction, int[] input)
        {
            int[] output = new int[input.Length];
            input.CopyTo(output, 0);

            output[input[instruction[3]]] = instruction[1];

            return output;
        }

        private int[] GtIR(int[] instruction, int[] input)
        {
            int[] output = new int[input.Length];
            input.CopyTo(output, 0);

            output[instruction[3]] = instruction[1] > input[instruction[2]] ? 1 : 0;

            return output;
        }

        private int[] GtRI(int[] instruction, int[] input)
        {
            int[] output = new int[input.Length];
            input.CopyTo(output, 0);

            output[instruction[3]] = input[instruction[1]] > instruction[2] ? 1 : 0;

            return output;
        }

        private int[] GtRR(int[] instruction, int[] input)
        {
            int[] output = new int[input.Length];
            input.CopyTo(output, 0);

            output[instruction[3]] = input[instruction[1]] > input[instruction[2]] ? 1 : 0;

            return output;
        }

        private int[] EqIR(int[] instruction, int[] input)
        {
            int[] output = new int[input.Length];
            input.CopyTo(output, 0);

            output[instruction[3]] = instruction[1] == input[instruction[2]] ? 1 : 0;

            return output;
        }

        private int[] EqRI(int[] instruction, int[] input)
        {
            int[] output = new int[input.Length];
            input.CopyTo(output, 0);

            output[instruction[3]] = input[instruction[1]] == instruction[2] ? 1 : 0;

            return output;
        }

        private int[] EqRR(int[] instruction, int[] input)
        {
            int[] output = new int[input.Length];
            input.CopyTo(output, 0);

            output[instruction[3]] = input[instruction[1]] == input[instruction[2]] ? 1 : 0;

            return output;
        }*/
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
