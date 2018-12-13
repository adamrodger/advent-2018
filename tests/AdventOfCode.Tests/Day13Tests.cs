using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day13Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day13 solver;

        public Day13Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day13();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day13.txt");
            return input;
        }

        [Fact]
        public void Part1_SampleInput_ProducesCorrectResponse()
        {
            var input = new[]
            {
                @"/->-\        ",
                @"|   |  /----\",
                @"| /-+--+-\  |",
                @"| | |  | v  |",
                @"\-+-/  \-+--/",
                @"  \------/   "
            };
            var expected = (7, 3);

            (var result, _) = solver.Solve(input);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = (69, 46);

            (var result, _) = solver.Solve(GetRealInput());
            output.WriteLine($"Day 13 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_SampleInput_ProducesCorrectResponse()
        {
            var input = new[]
            {
                @"/>-<\  ",
                @"|   |  ",
                @"| /<+-\",
                @"| | | v",
                @"\>+</ |",
                @"  |   ^",
                @"  \<->/"
            };
            var expected = (6, 4);

            (_, var result) = solver.Solve(input);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = (118, 108);

            (_, var result) = solver.Solve(GetRealInput());
            output.WriteLine($"Day 13 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}