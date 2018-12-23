using System.IO;
using Xunit;
using Xunit.Abstractions;


namespace AdventOfCode.Tests
{
    public class Day23Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day23 solver;

        public Day23Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day23();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day23.txt");
            return input;
        }

        [Fact]
        public void Part1_SampleInput_ProducesCorrectResponse()
        {
            string[] input =
            {
                "pos=<0,0,0>, r=4",
                "pos=<1,0,0>, r=1",
                "pos=<4,0,0>, r=3",
                "pos=<0,2,0>, r=1",
                "pos=<0,5,0>, r=3",
                "pos=<0,0,3>, r=1",
                "pos=<1,1,1>, r=1",
                "pos=<1,1,2>, r=1",
                "pos=<1,3,1>, r=1"
            };
            var expected = 7;

            var result = solver.Part1(input);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 906;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 23 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_SampleInput_ProducesCorrectResponse()
        {
            string[] input =
            {
                "pos=<10,12,12>, r=2",
                "pos=<12,14,12>, r=2",
                "pos=<16,12,12>, r=4",
                "pos=<14,14,14>, r=6",
                "pos=<50,50,50>, r=200",
                "pos=<10,10,10>, r=5"
            };

            var expected = 36;

            var result = solver.Part2(input);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = -1;

            var result = solver.Part2(GetRealInput());
            output.WriteLine($"Day 23 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}