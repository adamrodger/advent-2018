using System.IO;
using Xunit;
using Xunit.Abstractions;


namespace AdventOfCode.Tests
{
    public class Day17Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day17 solver;

        public Day17Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day17();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day17.txt");
            return input;
        }

        private static string[] GetSampleInput()
        {
            /*return new string[]
            {
                "x=495, y=2..7",
                "y=7, x=495..501",
                "x=501, y=3..7",
                "x=498, y=2..4",
                "x=506, y=1..2",
                "x=498, y=10..13",
                "x=504, y=10..13",
                "y=13, x=498..504"
            };*/

            return new string[] // small bucket fully inside a big bucket
            {
                "x=495, y=1..10",
                "x=505, y=2..10",
                "y=10, x=495..505",
                "x=498, y=4..7",
                "x=502, y=4..7",
                "y=7, x=498..502"
            };
        }

        [Fact]
        public void Solve_SampleInput_ProducesCorrectResponse()
        {
            var expected = 57;

            var result = solver.Solve(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Solve_RealInput_ProducesCorrectResponse()
        {
            var expected = -1;

            var result = solver.Solve(GetRealInput());
            output.WriteLine($"Day 17 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }
    }
}