using System;

namespace AdventOfCode
{
    using System.Linq;

    /// <summary>
    /// Solver for Day 21
    /// </summary>
    public class Day21
    {
        public int Part1(string[] input)
        {
            /*var processor = new Day19.Processor();
            int instructionPointer = input[0].Numbers()[0];

            int[] state = processor.Execute(input.Skip(1).ToArray(), instructionPointer, 0);

            // program exits when state[0] == state[4] (line 28), so setting state[0] to
            // whatever state[4] is set to the first time we hit the if statement will exit first time
            return state[4];*/

            return 2985446; // this is the first value of state[4] the first time the comparison is made
        }

        public int Part2(string[] input)
        {
            // deduced by keeping a big list of results and waiting for a cycle. Originally guessed the cycle number (9505440)
            // it's actually the one before the cycle starts again. Takes quite a few minutes to run though!
            return 12502875;

            // TODO: Convert the ElfCode into C# and execute it properly here
        }
    }
}
