using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 18
    /// </summary>
    public class Day18
    {
        public int Part1(string[] input)
        {
            char[,] grid = new char[input.Length,input.Length];
            input.ForEachChar((x, y, c) => grid[y,x] = c);

            grid.Print();

            int score = -1;

            foreach (int generation in Enumerable.Range(1, 10))
            {
                (grid, score) = this.Simulate(grid);
            }

            return score;
        }

        public int Part2(string[] input)
        {
            char[,] grid = new char[input.Length, input.Length];
            input.ForEachChar((x, y, c) => grid[y, x] = c);

            grid.Print();

            long max = 1_000_000_000;
            int score = -1;
            int generation = 1;
            var scores = new Dictionary<int, int>(500);
            int repeats = 0;

            while (generation < max)
            {
                (grid, score) = this.Simulate(grid);

                scores[generation] = score;

                if (generation > 28 && scores[generation - 28] == score)
                {
                    repeats++;
                }
                else
                {
                    repeats = 0;
                }

                if (repeats > 28)
                {
                    // hit the cycle
                    break;
                }

                generation++;
            }

            int cycleIndex = (int)((max - generation) % 28);
            int scoreIndex = generation - 28 + cycleIndex;
            score = scores[scoreIndex];

            return score;
        }

        private (char[,] next, int score) Simulate(char[,] grid)
        {
            char[,] next = new char[grid.GetLength(0), grid.GetLength(1)];

            grid.ForEach((int x, int y, char c) =>
            {
                var adjacent = this.GetAdjacent(grid, x, y);

                if (c == '.')
                {
                    next[y, x] = adjacent.Count(a => a == '|') >= 3 ? '|' : '.';
                }
                else if (c == '|')
                {
                    next[y, x] = adjacent.Count(a => a == '#') >= 3 ? '#' : '|';
                }
                else if (c == '#')
                {
                    next[y, x] = adjacent.Contains('#') && adjacent.Contains('|') ? '#' : '.';
                }
            });

            //next.Print();

            int woods = next.Search(c => c == '|').Count();
            int lumber = next.Search(c => c == '#').Count();

            return (next, woods * lumber);
        }

        public ICollection<char> GetAdjacent(char[,] grid, int x, int y)
        {
            List<char> adjacent = new List<char>(8);

            for (int dy = -1; dy <= 1; dy++)
            {
                int offsetY = y + dy;

                if (offsetY < 0 || offsetY >= grid.GetLength(0))
                {
                    // fell off the edge
                    continue;
                }

                for (int dx = -1; dx <= 1; dx++)
                {
                    int offsetX = x + dx;

                    if (offsetX < 0 || offsetX >= grid.GetLength(1) || (offsetX == x && offsetY == y))
                    {
                        // fell off the edge or we're on the origin cell
                        continue;
                    }

                    adjacent.Add(grid[offsetY, offsetX]);
                }
            }

            return adjacent;
        }
    }
}
