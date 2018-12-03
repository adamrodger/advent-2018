using System.IO;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 2
    /// </summary>
    public class Day02
    {
        private const string InputFile = "inputs/day02.txt";

        public int Part1()
        {
            var lines = File.ReadAllLines(InputFile);

            int doubles = 0;
            int triples = 0;

            foreach (string line in lines)
            {
                var groups = line.GroupBy(c => c).ToArray();

                if (groups.Any(g => g.Count() == 2))
                {
                    doubles++;
                }
                if (groups.Any(g => g.Count() == 3))
                {
                    triples++;
                }
            }

            return doubles * triples;
        }

        public string Part2()
        {
            var lines = File.ReadAllLines(InputFile);

            foreach (string outer in lines)
            {
                foreach (string inner in lines)
                {
                    int diff = 0;
                    int diffIndex = 0;

                    for (int i = 0; i < outer.Length; i++)
                    {
                        if(outer[i] != inner[i])
                        {
                            diff++;
                            diffIndex = i;
                        }
                    }

                    if (diff == 1)
                    {
                        return outer.Remove(diffIndex, 1);
                    }
                }
            }

            return "Not found";
        }
    }
}
