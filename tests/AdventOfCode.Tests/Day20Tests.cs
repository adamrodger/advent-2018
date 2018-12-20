using System.IO;
using Xunit;
using Xunit.Abstractions;


namespace AdventOfCode.Tests
{
    public class Day20Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day20 solver;

        public Day20Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day20();
        }

        private static string GetRealInput()
        {
            string input = File.ReadAllText("inputs/day20.txt");
            return input;
        }

        [Theory]
        [InlineData("^WNE$", 3)]
        [InlineData("^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$", 18)]
        public void Part1_SampleInput_ProducesCorrectResponse(string input, int expected)
        {
            var result = solver.Part1(input);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_NestedBranches_ProducesCorrectResponse()
        {
            var expected = 10;

            var result = solver.Part1("^ENWWW(NEEE|SSE(EE|N))$");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            // guessed 1604 -- too low
            // guessed 1605 -- too low
            var expected = -1;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 20 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = -1;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 20 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}