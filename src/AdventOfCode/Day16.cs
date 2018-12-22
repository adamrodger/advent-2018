namespace AdventOfCode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Utilities;

    /// <summary>
    /// Solver for Day 16
    /// </summary>
    public class Day16
    {
        public (int, int) Solve(string[] input)
        {
            var processor = new Processor();
            List<Transition> transitions = ParseTrainingSet(input);

            // part 1 - number of training operations which matched 3 or more instructions
            int part1 = processor.Train(transitions);

            // part 2 - execute the program and get the final value of register 0
            var current = new int[4];

            foreach (string line in input.Skip(3146)) // sample program starts on line 3147
            {
                var instruction = line.Split(' ').Select(int.Parse).ToArray();
                current = processor.Execute(current, instruction);
            }

            return (part1, current[0]);
        }

        private static List<Transition> ParseTrainingSet(string[] input)
        {
            var transitions = new List<Transition>();

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

            return transitions;
        }


        /// <summary>
        /// Emulates a processor with a limited instruction set
        /// </summary>
        public class Processor
        {
            private readonly Dictionary<string, Func<int[], int, int, int>> instructions =
                new Dictionary<string, Func<int[], int, int, int>>
                {
                    ["addr"] = (input, a, b) => input[a] + input[b],
                    ["addi"] = (input, a, b) => input[a] + b,
                    ["mulr"] = (input, a, b) => input[a] * input[b],
                    ["muli"] = (input, a, b) => input[a] * b,
                    ["banr"] = (input, a, b) => input[a] & input[b],
                    ["bani"] = (input, a, b) => input[a] & b,
                    ["borr"] = (input, a, b) => input[a] | input[b],
                    ["bori"] = (input, a, b) => input[a] | b,
                    ["setr"] = (input, a, _) => input[a],
                    ["seti"] = (input, a, _) => a,
                    ["gtir"] = (input, a, b) => a > input[b] ? 1 : 0,
                    ["gtri"] = (input, a, b) => input[a] > b ? 1 : 0,
                    ["gtrr"] = (input, a, b) => input[a] > input[b] ? 1 : 0,
                    ["eqir"] = (input, a, b) => a == input[b] ? 1 : 0,
                    ["eqri"] = (input, a, b) => input[a] == b ? 1 : 0,
                    ["eqrr"] = (input, a, b) => input[a] == input[b] ? 1 : 0
                };

            private readonly Dictionary<int, HashSet<string>> candidates = new Dictionary<int, HashSet<string>>(16);
            private readonly Dictionary<int, string> instructionMap = new Dictionary<int, string>(16);

            /// <summary>
            /// Train the processor with the given training set to calculate the opcode to instruction map
            /// </summary>
            /// <param name="trainingSet">Training set</param>
            /// <returns>Number of items in the training set that map to 3 or more possible instructions</returns>
            public int Train(ICollection<Transition> trainingSet)
            {
                int manyMatches = 0;

                foreach (Transition transition in trainingSet)
                {
                    var outputs = this.instructions
                                      .Keys
                                      .ToDictionary(i => i,
                                                    i => this.Execute(transition.Before, transition.Operation, i));

                    var matches = outputs.Where(o => o.Value.SequenceEqual(transition.After)).ToList();

                    // add all candidates for that opcode
                    this.candidates.GetOrCreate(transition.Operation[0]).UnionWith(matches.Select(m => m.Key));

                    if (matches.Count >= 3)
                    {
                        manyMatches++;
                    }
                }

                // now that training has finished, calculate the opcode to instruction map
                this.CalculateInstructionMap();

                return manyMatches;
            }

            /// <summary>
            /// Execute the given operation against the given input
            /// </summary>
            /// <param name="input">Input state</param>
            /// <param name="operation">Operation</param>
            /// <returns>Output of the operation</returns>
            public int[] Execute(int[] input, int[] operation)
            {
                string instruction = this.instructionMap[operation[0]];
                return this.Execute(input, operation, instruction);
            }

            /// <summary>
            /// Execute the given instruction against the given input with the given operation arguments
            /// </summary>
            /// <param name="input">Input state</param>
            /// <param name="operation">Operation arguments</param>
            /// <param name="instruction">Instruction name</param>
            /// <returns>Output of the instruction</returns>
            private int[] Execute(int[] input, int[] operation, string instruction)
            {
                // extract values from operation
                int a = operation[1];
                int b = operation[2];
                int c = operation[3];

                // perform the operation
                int result = this.instructions[instruction](input, a, b);

                // set C register
                int[] output = new int[input.Length];
                input.CopyTo(output, 0);
                output[c] = result;

                return output;
            }

            /// <summary>
            /// Calculates the map of integer opcodes to instruction names using process of elimination from the training run
            /// </summary>
            private void CalculateInstructionMap()
            {
                // boil down the candidates list via process of elimination
                while (this.candidates.Any())
                {
                    // find the opcode which can only match one possible instruction
                    var unique = this.candidates.First(c => c.Value.Count == 1);
                    string instruction = unique.Value.First();
                    this.instructionMap[unique.Key] = instruction;
                    this.candidates.Remove(unique.Key);

                    // eliminate from others
                    foreach (KeyValuePair<int, HashSet<string>> pair in this.candidates)
                    {
                        pair.Value.Remove(instruction);
                    }
                }
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
}
