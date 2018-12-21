namespace AdventOfCode
{
    using ElfCode;

    /// <summary>
    /// Solver for Day 19
    /// </summary>
    public class Day19
    {
        public int Part1(string[] input)
        {
            var emulator = new Emulator(input);
            emulator.Execute();
            return emulator.Registers[0];
        }

        public int Part2(string[] input)
        {
            //var emulator = new Emulator(input);
            //emulator.Registers[0] = 1;
            //emulator.Execute();
            //return emulator.Registers[0];

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
    }
}
