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

        /*[Theory]
        [InlineData("1212", 6)]
        [InlineData("1221", 0)]
        [InlineData("123425", 4)]
        [InlineData("123123", 12)]
        [InlineData("12131415", 4)]
        [InlineData(Part2Input, 1092)]
        public void Part2_WhenCalled_ProvidesSolution(string input, int expected)
        {
            var solve = new Day01();

            int actual = solve.Part2(input);

            Assert.Equal(expected, actual);
        }*/
    }
}
