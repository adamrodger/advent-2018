using Xunit;

namespace AdventOfCode.Tests
{
    public class Day03Tests
    {
        [Fact]
        public void Solve_WhenCalled_ReturnsCorrectResult()
        {
            var solver = new Day03();

            var actual = solver.Solve();

            Assert.Equal((96569, 1023), actual);
        }
    }
}
