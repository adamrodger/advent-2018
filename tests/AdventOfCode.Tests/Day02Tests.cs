using Xunit;

namespace AdventOfCode.Tests
{
    public class Day02Tests
    {
        [Fact]
        public void Part1_WhenCalled_ProvidesSolution()
        {
            var solve = new Day02();

            int actual = solve.Part1();

            Assert.Equal(5928, actual);
        }

        [Fact]
        public void Part2_WhenCalled_ProvidesSolution()
        {
            var solve = new Day02();

            string actual = solve.Part2();

            Assert.Equal("bqlporuexkwzyabnmgjqctvfs", actual);
        }
    }
}
