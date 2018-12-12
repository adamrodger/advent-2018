using System.IO;
using Xunit;
using Xunit.Abstractions;


namespace AdventOfCode.Tests
{
    public class Day12Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day12 solver;

        public Day12Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day12();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day12.txt");
            return input;
        }

        private static string[] GetSampleInput()
        {
            return new string[]
            {
                "initial state: #..#.#..##......###...###",
                "           ",
                "...## => #",
                "..#.. => #",
                ".#... => #",
                ".#.#. => #",
                ".#.## => #",
                ".##.. => #",
                ".#### => #",
                "#.#.# => #",
                "#.### => #",
                "##.#. => #",
                "##.## => #",
                "###.. => #",
                "###.# => #",
                "####. => #"
            };
        }

        [Fact]
        public void Part1_SampleInput_ProducesCorrectResponse()
        {
            var expected = 325;

            var result = solver.Part1(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 1987;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 12 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = -1;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 12 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}