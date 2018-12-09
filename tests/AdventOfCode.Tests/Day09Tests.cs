using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day09Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day09 solver;

        public Day09Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day09();
        }
        
        [Theory]
        [InlineData(9, 25, 32)]
        [InlineData(10, 1618, 8317)]
        [InlineData(13, 7999, 146373)]
        [InlineData(17, 1104, 2764)]
        [InlineData(21, 6111, 54718)]
        [InlineData(30, 5807, 37305)]
        public void Part1_SampleInput_ProducesCorrectResponse(int players, int highestMarble, int expected)
        {
            var result = solver.Solve(players, highestMarble);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 437654;

            var result = solver.Solve(400, 71864);
            output.WriteLine($"Day 09 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 3689913905;

            var result = solver.Solve(400, 7186400);
            output.WriteLine($"Day 09 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}