using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day14Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day14 solver;

        public Day14Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day14();
        }

        [Theory]
        [InlineData(5, "0124515891")]
        [InlineData(9, "5158916779")]
        [InlineData(18, "9251071085")]
        [InlineData(2018, "5941429882")]
        [InlineData(286051, "2111113678")]
        public void Part1_WhenCalled_ProducesCorrectResponse(int iterations, string expected)
        {
            var result = solver.Part1(iterations);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("51589", 9)]
        [InlineData("01245", 5)]
        [InlineData("92510", 18)]
        [InlineData("59414", 2018)]
        [InlineData("286051", 20195114)]
        public void Part2_WhenCalled_ProducesCorrectResponse(string searchTerm, int expected)
        {
            var result = solver.Part2(searchTerm);

            Assert.Equal(expected, result);
        }
    }
}