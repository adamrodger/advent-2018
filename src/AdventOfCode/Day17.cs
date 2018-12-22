using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode
{
    using Utilities;

    /// <summary>
    /// Solver for Day 17
    /// </summary>
    public class Day17
    {
        public (int, int) Solve(string[] input)
        {
            (var grid, var origin) = Parse(input);

            this.Search(origin, 0, grid);

            grid.Print();

            int still = grid.Search(cell => cell == '~').Count();
            int flowing = grid.Search(cell => cell == '|').Count();

            return (still + flowing, still);
        }

        /// <summary>
        /// Recursively search through the grid and mark the areas of still and flowing water
        /// </summary>
        /// <param name="x">X co-ordinate</param>
        /// <param name="y">Y co-ordinate</param>
        /// <param name="grid">Grid</param>
        private void Search(int x, int y, char[,] grid)
        {
            // flow into this cell
            grid[y, x] = '|';

            if (y + 1 >= grid.GetLength(0) || grid[y + 1, x] == '|')
            {
                // hit the bottom or landed on an existing stream
                return;
            }

            // move down
            if (grid[y + 1, x] == '.')
            {
                this.Search(x, y + 1, grid);
            }

            bool onShelf = grid[y + 1, x] == '#' || grid[y + 1, x] == '~';

            if (onShelf)
            {
                // move left
                if (grid[y, x - 1] == '.')
                {
                    this.Search(x - 1, y, grid);
                }

                // move right
                if (grid[y, x + 1] == '.')
                {
                    this.Search(x + 1, y, grid);
                }
            }

            // reached a dead end, fill this entire row with settled water if we're inside a container
            if (IsBetweenWalls(x, y, grid))
            {
                FillBasin(x, y, grid);
            }

            if (grid.GetLength(0) < 20) // only for sample input
            {
                Debug.WriteLine(string.Empty);
                grid.Print();
            }
        }

        private static bool IsBetweenWalls(int x, int y, char[,] grid)
        {
            // follow the flow horizontally each way until we hit a wall or standing water
            bool wallLeft = false;
            bool wallRight = false;

            // look left
            for (int i = x - 1; i >= 0; i--)
            {
                if (grid[y + 1, i] != '#' && grid[y + 1, i] != '~')
                {
                    // fell off the edge
                    return false;
                }

                if (grid[y, i] == '~' || grid[y, i] == '#')
                {
                    wallLeft = true;
                    break;
                }
            }

            // look right
            for (int i = x + 1; i < grid.GetLength(1); i++)
            {
                if (grid[y + 1, i] != '#' && grid[y + 1, i] != '~')
                {
                    // fell off the edge
                    return false;
                }

                if (grid[y, i] == '~' || grid[y, i] == '#')
                {
                    wallRight = true;
                    break;
                }
            }

            return wallLeft && wallRight;
        }

        private static void FillBasin(int x, int y, char[,] grid)
        {
            // fill left
            for (int i = x; i > 0; i--)
            {
                if (grid[y, i] == '#')
                {
                    break;
                }

                grid[y, i] = '~';
            }

            // fill right
            for (int i = x; i < grid.GetLength(1); i++)
            {
                if (grid[y, i] == '#')
                {
                    break;
                }

                grid[y, i] = '~';
            }
        }

        private static (char[,] grid, int origin) Parse(string[] input)
        {
            var verticals = input.Where(i => i[0] == 'x').Select(i => i.Numbers()).ToArray();
            var horizontals = input.Where(i => i[0] == 'y').Select(i => i.Numbers()).ToArray();

            var minX = Math.Min(verticals.Min(v => v[0]), horizontals.Min(h => h[1])) - 1; // room to flow off the side
            var maxX = Math.Max(verticals.Max(v => v[0]), horizontals.Max(h => h[2])) + 1;

            var minY = Math.Min(horizontals.Min(v => v[0]), verticals.Min(h => h[1]));
            var maxY = Math.Max(horizontals.Max(v => v[0]), verticals.Max(h => h[2]));

            var xSize = maxX - minX + 1;
            var ySize = maxY - minY + 1;

            char[,] grid = new char[ySize, xSize];

            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    grid[y, x] = '.';
                }
            }

            int origin = 500 - minX;
            grid[0, origin] = '+';

            foreach (int[] wall in verticals)
            {
                for (int y = wall[1] - minY; y <= wall[2] - minY; y++)
                {
                    grid[y, wall[0] - minX] = '#';
                }
            }

            foreach (int[] wall in horizontals)
            {
                for (int x = wall[1] - minX; x <= wall[2] - minX; x++)
                {
                    grid[wall[0] - minY, x] = '#';
                }
            }

            return (grid, origin);
        }
    }
}
