using Xunit;

namespace AdventOfCode.Tests
{
    public class Day01Tests
    {
        [Fact]
        public void Part1_WhenCalled_ProvidesSolution()
        {
            var solve = new Day01();

            int actual = solve.Part1();

            Assert.Equal(437, actual);
        }

        [Fact]
        public void Part2_WhenCalled_ProvidesSolution()
        {
            var solve = new Day01();

            int actual = solve.Part2();

            Assert.Equal(655, actual);
        }
    }
}
