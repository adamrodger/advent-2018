using System.IO;
using Xunit;

namespace AdventOfCode.Tests
{
    public class Day04Tests
    {
        [Fact]
        public void Part1_SampleInput_ProducesCorrectResponse()
        {
            var lines = new[]
            {
                "[1518-11-01 00:00] Guard #10 begins shift",
                "[1518-11-01 00:05] falls asleep",
                "[1518-11-01 00:25] wakes up",
                "[1518-11-01 00:30] falls asleep",
                "[1518-11-01 00:55] wakes up",
                "[1518-11-01 23:58] Guard #99 begins shift",
                "[1518-11-02 00:40] falls asleep",
                "[1518-11-02 00:50] wakes up",
                "[1518-11-03 00:05] Guard #10 begins shift",
                "[1518-11-03 00:24] falls asleep",
                "[1518-11-03 00:29] wakes up",
                "[1518-11-04 00:02] Guard #99 begins shift",
                "[1518-11-04 00:36] falls asleep",
                "[1518-11-04 00:46] wakes up",
                "[1518-11-05 00:03] Guard #99 begins shift",
                "[1518-11-05 00:45] falls asleep",
                "[1518-11-05 00:55] wakes up"
            };

            var solve = new Day04();

            (int, int) actual = solve.Solve(lines);

            Assert.Equal((240, 4455), actual);
        }

        [Fact]
        public void Part1_WhenCalled_ProvidesSolution()
        {
            var lines = File.ReadAllLines("inputs/day04.txt");
            var solve = new Day04();

            (int, int) actual = solve.Solve(lines);

            Assert.Equal((11367, 36896), actual);
        }
    }
}
