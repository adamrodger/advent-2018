using System.IO;
using Xunit;
using Xunit.Abstractions;


namespace AdventOfCode.Tests
{
    public class Day25Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day25 solver;

        public Day25Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day25();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day25.txt");
            return input;
        }

        private static string[] GetSampleInput()
        {
            return new string[]
            {
                "0,0,0,0",
                "3,0,0,0",
                "0,3,0,0",
                "0,0,3,0",
                "0,0,0,3",
                "0,0,0,6",
                "9,0,0,0",
                "12,0,0,0"
            };
        }

        [Fact]
        public void Part1_SampleInput_ProducesCorrectResponse()
        {
            var expected = 2;

            var result = solver.Part1(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 310;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 25 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }
    }
}