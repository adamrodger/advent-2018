using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day11Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day11 solver;

        public Day11Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day11();
        }

        [Theory]
        [InlineData(18, 33, 45)]
        [InlineData(42, 21, 61)]
        public void Part1_SampleInput_ProducesCorrectResponse(int serial, int x, int y)
        {
            var expected = (x, y);

            var result = solver.Part1(serial);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = (243, 17);

            var result = solver.Part1(7347);
            output.WriteLine($"Day 11 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(18, 90, 269, 16)]
        [InlineData(42, 232, 251, 12)]
        public void Part2_SampleInput_ProducesCorrectResponse(int serial, int x, int y, int size)
        {
            var expected = (x, y, size);

            var result = solver.Part2(serial);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = (233, 228, 12);

            var result = solver.Part2(7347);
            output.WriteLine($"Day 11 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}