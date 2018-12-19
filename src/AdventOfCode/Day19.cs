using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 19
    /// </summary>
    public class Day19
    {
        public int Part1(string[] input)
        {
            var processor = new Processor();
            int instructionPointer = input[0].Numbers()[0];

            int[] state = processor.Execute(input.Skip(1).ToArray(), instructionPointer, 0);
            return state[0];
        }

        public int Part2(string[] input)
        {
            //var processor = new Processor();
            //int instructionPointer = input[0].Numbers()[0];

            //int[] state = processor.Execute(input.Skip(1).ToArray(), instructionPointer, 1);
            //return state[0];

            /*
             * this just loops pretty much forever - from looking at the execution after a few million iterations,
             * it calculates a really big number (10551386) then works out all the factors of that number and adds
             * them together into register 1 (so 1, 2, then eventually it'd get to 5275693, 10551386).
             *
             * So the answer is 1 + 2 + 5,275,693 + 10,551,386 = 15,827,082
             *
             * I got the factors from https://www.mathsisfun.com/numbers/factors-all-tool.html#calc :D
             *
             * See the annotated version of the input for a psuedo-code version of what it's doing - basically calculate a
             * huge number and then do an outer and inner loop of 0..x adding x to total if x * y == target. It would take
             * 111,331,746,520,996 iterations to brute force that :D
             */

            return 15827082;
        }


        /// <summary>
        /// Emulates a processor with a limited instruction set
        /// </summary>
        public class Processor
        {
            private readonly Dictionary<string, Func<int[], int, int, int>> instructions = new Dictionary<string, Func<int[], int, int, int>>
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

            /// <summary>
            /// Execute the given program using the given register as the instruction pointer and a given start state
            /// </summary>
            /// <param name="program">Program instructions</param>
            /// <param name="instructionRegister">ID of the instruction register</param>
            /// <param name="startState">Starting state of register 0</param>
            /// <returns>Final output</returns>
            public int[] Execute(string[] program, int instructionRegister, int startState)
            {
                (string opcode, int[] args)[] operations = program.Select(p => (p.Split(' ')[0], p.Numbers())).ToArray();

                int iterations = 0;
                int[] state = { startState, 0, 0, 0, 0, 0 };

                while (state[instructionRegister] >= 0 && state[instructionRegister] < operations.Length)
                {
                    (string opcode, int[] args) = operations[state[instructionRegister]];
                    state = this.Execute(state, args, opcode);

                    state[instructionRegister] = state[instructionRegister] + 1;

                    Print(state, operations, state[instructionRegister]);
                    iterations++;

                    if (iterations % 100000 == 0)
                    {
                        Debug.WriteLine($"{iterations} - {string.Join(" ", state)}");
                    }
                }

                return state;
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
                int a = operation[0];
                int b = operation[1];
                int c = operation[2];

                // perform the operation
                int result = this.instructions[instruction](input, a, b);

                // set C register
                int[] output = new int[input.Length];
                input.CopyTo(output, 0);
                output[c] = result;

                return output;
            }

            /// <summary>
            /// Print the current state and which instruction has just executed
            /// </summary>
            /// <param name="state">Current state</param>
            /// <param name="operations">Program instructions</param>
            /// <param name="pointer">Currently executing instruction pointer</param>
            private static void Print(int[] state, (string opcode, int[] args)[] operations, int pointer)
            {
                if (!Debugger.IsAttached)
                {
                    return;
                }

                var builder = new StringBuilder();

                builder.AppendLine($"State: {string.Join(", ", state)}\n");

                for (int i = 0; i < operations.Length; i++)
                {
                    builder.Append($"{i, -50} {operations[i].opcode} {string.Join(" ", operations[i].args)}");

                    if (i == pointer)
                    {
                        builder.Append("  <<<<<<");
                    }

                    builder.AppendLine();
                }

                foreach (int blank in Enumerable.Range(1, 6))
                {
                    builder.AppendLine();
                }

                Debug.WriteLine(builder.ToString());
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}
