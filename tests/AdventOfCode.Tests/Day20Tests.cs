using System.IO;
using Xunit;
using Xunit.Abstractions;


namespace AdventOfCode.Tests
{
    public class Day20Tests
    {
        private readonly ITestOutputHelper output;
        private readonly Day20 solver;

        public Day20Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day20();
        }

        private static string GetRealInput()
        {
            string input = File.ReadAllText("inputs/day20.txt");
            return input;
        }

        [Fact]
        public void Part1_NoBranches_ProducesCorrectResponse()
        {
            var expected = 3;

            var (result, _) = solver.Solve("^WNE$");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_SimpleBranching_ProducesCorrectResponse()
        {
            var expected = 18;

            var (result, _) = solver.Solve("^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_NestedBranching_ProducesCorrectResponse()
        {
            var expected = 10;

            var (result, _) = solver.Solve("^ENWWW(NEEE|SSE(EE|N))$");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected = 3545;

            var (result, _) = solver.Solve(GetRealInput());
            output.WriteLine($"Day 20 - Part 1 - {result}");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 7838;

            var (_, result) = solver.Solve(GetRealInput());
            output.WriteLine($"Day 20 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}