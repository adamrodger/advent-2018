using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day06Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day06 solver;

        public Day06Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day06();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day06.txt");
            return input;
        }

        private static string[] GetSampleInput()
        {
            return new string[]
            {
                "1, 1",
                "1, 6",
                "8, 3",
                "3, 4",
                "5, 5",
                "8, 9"
            };
        }

        [Fact]
        public void Part1_SampleInput_ProducesCorrectResponse()
        {
            // aaaaa.cccc
            // aAaaa.cccc
            // aaaddecccc
            // aadddeccCc
            // ..dDdeeccc
            // bb.deEeecc
            // bBb.eeee..
            // bbb.eeefff
            // bbb.eeffff
            // bbb.ffffFf

            var expected = 17;

            var result = solver.Part1(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 4342;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day 06 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_SampleInput_ProducesCorrectResponse()
        {
            var expected = 16;

            var result = solver.Part2(GetSampleInput(), 32);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 42966;

            var result = solver.Part2(GetRealInput(), 10000);
            output.WriteLine($"Day 06 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}