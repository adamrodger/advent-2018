using System.IO;
using Xunit;
using Xunit.Abstractions;


namespace AdventOfCode.Tests
{
    public class Day15Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day15 solver;

        public Day15Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day15();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day15.txt");
            return input;
        }

        private static string[] GetSampleInput()
        {
            return new string[]
            {
                "#######",
                "#.G...#",
                "#...EG#",
                "#.#.#G#",
                "#..G#E#",
                "#.....#",
                "#######"
            };
        }

        [Fact]
        public void Part1_SampleInput_ProducesCorrectResponse()
        {
            var expected = 27730;

            var result = solver.Part1(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 221754;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 15 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_SampleInput_ProducesCorrectResponse()
        {
            var expected = -1;

            var result = solver.Part2(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = -1;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 15 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}