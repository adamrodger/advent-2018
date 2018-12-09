using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    using FactAttribute = System.Runtime.CompilerServices.CompilerGeneratedAttribute;
    //using FactAttribute = Xunit.FactAttribute; // uncomment to enable tests

    // use ctrl+h to replace all XX with day number
    public class DayXXTests
    {
        private readonly ITestOutputHelper output;
        private readonly DayXX solver;

        public DayXXTests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new DayXX();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/dayXX.txt");
            return input;
        }

        private static string[] GetSampleInput()
        {
            return new string[]
            {
                
            };
        }

        [Fact]
        public void Part1_SampleInput_ProducesCorrectResponse()
        {
            var expected = -1;

            var result = solver.Part1(GetSampleInput());

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = -1;

            var result = solver.Part1(GetRealInput());
            output.WriteLine($"Day XX - Part 1 - {result}");

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
            output.WriteLine($"Day XX - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}