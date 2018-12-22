using System.IO;
using Xunit;
using Xunit.Abstractions;


namespace AdventOfCode.Tests
{
    public class Day22Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day22 solver;

        public Day22Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day22();
        }

        [Fact]
        public void Part1_SampleInput_ProducesCorrectResponse()
        {
            var expected = 114;

            var result = solver.Part1(510, (10, 10));

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 11972;

            var result = solver.Part1(5355, (14, 796));
            output.WriteLine($"Day 22 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }
        
        [Fact]
        public void Part2_SampleInput_ProducesCorrectResponse()
        {
            var expected = 45;

            var result = solver.Part2(510, (10, 10));

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = -1;

            var result = solver.Part2(5355, (14, 796));
            output.WriteLine($"Day 22 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}