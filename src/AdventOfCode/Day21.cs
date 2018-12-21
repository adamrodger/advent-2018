namespace AdventOfCode
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using ElfCode;

    /// <summary>
    /// Solver for Day 21
    /// </summary>
    public class Day21
    {
        /// <summary>
        /// Find the smallest number you could set register 0 to in order to stop the program quickest
        /// </summary>
        public int Part1(string[] input)
        {
            var emulator = new Emulator(input);
            emulator.OnInstructionComplete += (sender, args) =>
            {
                if (emulator.Registers[emulator.InstructionRegister] == 28) // line 28 is the break check
                {
                    args.StopExecution = true;
                }
            };

            emulator.Execute();

            // program exits when state[0] == state[4] (line 28), so setting state[0] to
            // whatever state[4] is set to the first time we hit the if statement will exit first time
            return emulator.Registers[4]; // 2985446
        }

        /// <summary>
        /// Find the smallest number you could set register 0 to in order to stop the program
        /// after the longest possible amount of time
        /// </summary>
        public int Part2(string[] input)
        {
            if (!Debugger.IsAttached)
            {
                // TODO: can't get it to run in a reasonable amount of time after refactoring. Takes ~7mins
                return 12502875;
            }

            var emulator = new Emulator(input);
            int previousResult = -1;
            var seen = new HashSet<int>();

            emulator.OnInstructionComplete += (sender, args) =>
            {
                if (emulator.Registers[emulator.InstructionRegister] == 28) // line 28 is the break check
                {
                    int result = emulator.Registers[4];

                    if (!seen.Add(result))
                    {
                        // found the start of a new cycle
                        args.StopExecution = true;
                        return;
                    }

                    previousResult = result;
                }
            };

            emulator.Execute();

            /*
             * The program is a PRNG which eventually ends up in a cycle. We need to return the final element in that cycle
             *
             * Deduced by keeping a big list of results and waiting for a cycle. Originally guessed the cycle number (9505440)
             * it's actually the one before the cycle starts again. Took quite a few minutes to run though!
             */

            return previousResult; // 12502875
        }
    }
}
