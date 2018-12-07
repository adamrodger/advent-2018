using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    using TestAttribute = Xunit.FactAttribute; // uncomment to enable tests

    public class Day07Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day07 solver;

        public Day07Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day07();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day07.txt");
            return input;
        }

        private static string[] GetSampleInput()
        {
            return new string[]
            {
                "Step C must be finished before step A can begin.",
                "Step C must be finished before step F can begin.",
                "Step A must be finished before step B can begin.",
                "Step A must be finished before step D can begin.",
                "Step B must be finished before step E can begin.",
                "Step D must be finished before step E can begin.",
                "Step F must be finished before step E can begin."
            };
        }

        [Test]
        public void Part1_SampleInput_ProducesCorrectResponse()
        {
            var expected = "CABDFE";

            var result = solver.Part1(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Test]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = "IOFSJQDUWAPXELNVYZMHTBCRGK";

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 07 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Test]
        public void Part2_SampleInput_ProducesCorrectResponse()
        {
            int expected = 15;

            var result = solver.Part2(GetSampleInput(), 0, 2);

            Assert.Equal(expected, result);
        }

        [Test]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 931;

            var result = solver.Part2(GetRealInput(), 60, 4);
            output.WriteLine($"Day 07 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}