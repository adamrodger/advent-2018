using System.IO;
using Xunit;
using Xunit.Abstractions;


namespace AdventOfCode.Tests
{
    public class Day10Tests
    {
        private const int SampleDetectionThreshold = 8;
        private const int RealDetectionThreshold = 9;

        private readonly ITestOutputHelper output;
        private readonly Day10 solver;

        public Day10Tests(ITestOutputHelper output)
        {
            this.output = output;
            this.solver = new Day10();
        }

        private static string[] GetRealInput()
        {
            string[] input = File.ReadAllLines("inputs/day10.txt");
            return input;
        }

        private static string[] GetSampleInput()
        {
            return new string[]
            {
                "position=< 9,  1> velocity=< 0,  2>",
                "position=< 7,  0> velocity=<-1,  0>",
                "position=< 3, -2> velocity=<-1,  1>",
                "position=< 6, 10> velocity=<-2, -1>",
                "position=< 2, -4> velocity=< 2,  2>",
                "position=<-6, 10> velocity=< 2, -2>",
                "position=< 1,  8> velocity=< 1, -1>",
                "position=< 1,  7> velocity=< 1,  0>",
                "position=<-3, 11> velocity=< 1, -2>",
                "position=< 7,  6> velocity=<-1, -1>",
                "position=<-2,  3> velocity=< 1,  0>",
                "position=<-4,  3> velocity=< 2,  0>",
                "position=<10, -3> velocity=<-1,  1>",
                "position=< 5, 11> velocity=< 1, -2>",
                "position=< 4,  7> velocity=< 0, -1>",
                "position=< 8, -2> velocity=< 0,  1>",
                "position=<15,  0> velocity=<-2,  0>",
                "position=< 1,  6> velocity=< 1,  0>",
                "position=< 8,  9> velocity=< 0, -1>",
                "position=< 3,  3> velocity=<-1,  1>",
                "position=< 0,  5> velocity=< 0, -1>",
                "position=<-2,  2> velocity=< 2,  0>",
                "position=< 5, -2> velocity=< 1,  2>",
                "position=< 1,  4> velocity=< 2,  1>",
                "position=<-2,  7> velocity=< 2, -2>",
                "position=< 3,  6> velocity=<-1, -1>",
                "position=< 5,  0> velocity=< 1,  0>",
                "position=<-6,  0> velocity=< 2,  0>",
                "position=< 5,  9> velocity=< 1, -2>",
                "position=<14,  7> velocity=<-2,  0>",
                "position=<-3,  6> velocity=< 2, -1>"
            };
        }

        [Fact]
        public void Part1_SampleInput_ProducesCorrectResponse()
        {
            var expected =
@"
#   #  ###
#   #   # 
#   #   # 
#####   # 
#   #   # 
#   #   # 
#   #   # 
#   #  ###
";

            this.output.WriteLine(expected);
            this.output.WriteLine("");

            var (_, result) = solver.Solve(GetSampleInput(), SampleDetectionThreshold);

            this.output.WriteLine(result);

            // compare without Windows line endings since that's messing up the tests
            Assert.Equal(expected.Replace("\r", string.Empty), result.Replace("\r", string.Empty));
        }

        [Fact]
        public void Part1_RealInput_ProducesCorrectResponse()
        {
            var expected =
@"
 ####      ###  #    #  #    #  #####   ######  ######  ######
#    #      #   ##   #  #   #   #    #       #  #       #     
#           #   ##   #  #  #    #    #       #  #       #     
#           #   # #  #  # #     #    #      #   #       #     
#           #   # #  #  ##      #####      #    #####   ##### 
#  ###      #   #  # #  ##      #    #    #     #       #     
#    #      #   #  # #  # #     #    #   #      #       #     
#    #  #   #   #   ##  #  #    #    #  #       #       #     
#   ##  #   #   #   ##  #   #   #    #  #       #       #     
 ### #   ###    #    #  #    #  #####   ######  ######  ######
";

            this.output.WriteLine(expected);
            this.output.WriteLine("");

            var (_, result) = solver.Solve(GetRealInput(), RealDetectionThreshold);
            output.WriteLine($"Day 10 - Part 1 -\n{result}");

            // compare without Windows line endings since that's messing up the tests
            Assert.Equal(expected.Replace("\r", string.Empty), result.Replace("\r", string.Empty));
        }

        [Fact]
        public void Part2_SampleInput_ProducesCorrectResponse()
        {
            var expected = 3;

            var (result, _) = solver.Solve(GetSampleInput(), SampleDetectionThreshold);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Part2_RealInput_ProducesCorrectResponse()
        {
            var expected = 10727;

            var (result, _) = solver.Solve(GetRealInput(), RealDetectionThreshold);
            output.WriteLine($"Day 10 - Part 2 - {result}");

            Assert.Equal(expected, result);
        }
    }
}