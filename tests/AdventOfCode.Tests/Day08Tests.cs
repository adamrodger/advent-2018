using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    using TestAttribute = Xunit.FactAttribute;

    public class Day08Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day08 solver;

        public Day08Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day08();
        }

        private static string GetRealInput()
        {
            string input = File.ReadAllText("inputs/day08.txt");
            return input;
        }

        private static string GetSampleInput()
        {
            return "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";
        }

        [Test]
        public void Part1_SampleInput_ProducesCorrectResponse()
        {
            var expected = 138;

            (var result, _) = solver.Solve(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Test]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 42951;

            (var result, _) = solver.Solve(GetRealInput());
            output.WriteLine($"Day 08 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Test]
        public void Part2_SampleInput_ProducesCorrectResponse()
        {
            var expected = 66;

            (_, var result) = solver.Solve(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Test]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 18568;

            (_, var result) = solver.Solve(GetRealInput());
            output.WriteLine($"Day 08 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}