using System.IO;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day05Tests
    {
        [Fact]
        public void Part1_SampleInput_ProvidesSolution()
        {
            string input = "dabAcCaCBAcCcaDA";
            var solve = new Day05();

            int actual = solve.React(input);

            Assert.Equal(10, actual);
        }

        [Fact]
        public void Part1_WhenCalled_ProvidesSolution()
        {
            var input = File.ReadAllText("inputs/day05.txt");
            var solve = new Day05();

            int actual = solve.React(input);

            Assert.Equal(11590, actual);
        }

        [Fact]
        public void Part2_SampleInput_ProvidesSolution()
        {
            string input = "dabAcCaCBAcCcaDA";
            var solve = new Day05();

            int actual = solve.Part2(input);

            Assert.Equal(4, actual);
        }

        [Fact]
        public void Part2_WhenCalled_ProvidesSolution()
        {
            var input = File.ReadAllText("inputs/day05.txt");
            var solve = new Day05();

            int actual = solve.Part2(input);

            Assert.Equal(4504, actual);
        }
    }
}
