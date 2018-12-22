using System;

namespace AdventOfCode
{
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Solver for Day 22
    /// </summary>
    public class Day22
    {
        public int Part1(int depth, (int x, int y) target)
        {
            int[,] map = new int[target.y + 1, target.x + 1];

            // build map
            map.ForEach((x, y, _) =>
            {
                int index;

                if (x == 0 && y == 0)
                {
                    index = 0;
                }
                else if (x == target.x && y == target.y)
                {
                    index = 0;
                }
                else if (y == 0)
                {
                    index = x * 16807;
                }
                else if (x == 0)
                {
                    index = y * 48271;
                }
                else
                {
                    index = map[y - 1, x] * map[y, x - 1];
                }

                int erosion = (index + depth) % 20183;
                map[y, x] = erosion;
            });

            var builder = new StringBuilder(map.GetLength(0) * (map.GetLength(1) + Environment.NewLine.Length));
            map.ForEach(cell =>
            {
                switch (cell % 3)
                {
                    case 0:
                        builder.Append('.');
                        break;
                    case 1:
                        builder.Append('=');
                        break;
                    case 2:
                        builder.Append('|');
                        break;
                }
            }, () => builder.AppendLine());
            builder.AppendLine();

            Debug.Write(builder.ToString());

            // add up risk
            int result = map.Select(erosion => erosion % 3).Sum();

            return result;
        }

        public int Part2(string[] input)
        {
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
        }
    }
}
