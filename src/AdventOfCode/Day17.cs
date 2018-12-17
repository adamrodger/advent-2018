using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 17
    /// </summary>
    public class Day17
    {
        public int Solve(string[] input)
        {
            (var grid, var origin) = Parse(input);

            PrintGrid(grid);

            this.Search(origin, 0, grid);

            PrintGrid(grid);

            int still = 0;
            int flowing = 0;

            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    if (grid[y, x] == '~')
                    {
                        still++;
                    }
                    else if (grid[y, x] == '|')
                    {
                        flowing++;
                    }
                }
            }

            return still + flowing;
        }

        /// <summary>
        /// Recursively search through the grid and mark the areas of still and flowing water
        /// </summary>
        /// <param name="x">X co-ordinate</param>
        /// <param name="y">Y co-ordinate</param>
        /// <param name="grid">Grid</param>
        private bool Search(int x, int y, char[,] grid)
        {
            grid[y, x] = '|';

            if (y + 1 >= grid.GetLength(0))
            {
                // hit the bottom
                return false;
            }

            Debug.WriteLine(string.Empty);
            PrintGrid(grid);

            if (grid[y + 1, x] == '.')
            {
                if (!this.Search(x, y + 1, grid))
                {
                    // this path hits the bottom, don't spread sideways
                    return false;
                }
            }

            if (grid[y, x - 1] == '.')
            {
                this.Search(x - 1, y, grid);
            }

            if (grid[y, x + 1] == '.')
            {
                this.Search(x + 1, y, grid);
            }

            // we need to stop because we've hit walls or other water
            grid[y, x] = '~';

            return true;
        }

        private static void PrintGrid(char[,] grid)
        {
            if (!Debugger.IsAttached)
            {
                return;
            }

            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    Debug.Write(grid[y, x]);
                }

                Debug.WriteLine(string.Empty);
            }
        }

        private static (char[,] grid, int origin) Parse(string[] input)
        {
            var walls = new List<(bool vertical, int same, int start, int end)>(input.Length);

            foreach (string line in input)
            {
                /*
                 * x=513, y=877..886
                 * y=334, x=496..500
                 */

                string[] parts = line.Split(',').Select(t => t.Trim()).ToArray();
                string[] range = parts[1].Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                int[] nums =
                {
                    int.Parse(parts[0].Substring(2)),
                    int.Parse(range[0].Substring(2)),
                    int.Parse(range[1])
                };

                walls.Add((line[0] == 'x', nums[0], nums[1], nums[2]));
            }

            var verticals = walls.Where(w => w.vertical).ToArray();
            var horizontals = walls.Where(w => !w.vertical).ToArray();

            var minX = Math.Min(verticals.Min(v => v.same), horizontals.Min(h => h.start));
            var maxX = Math.Max(verticals.Max(v => v.same), horizontals.Max(h => h.end));

            var minY = Math.Min(horizontals.Min(v => v.same), verticals.Min(h => h.start));
            var maxY = Math.Max(horizontals.Max(v => v.same), verticals.Max(h => h.end));

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

            foreach ((bool vertical, int same, int start, int end) wall in walls)
            {
                if (wall.vertical)
                {
                    for (int y = wall.start - minY; y <= wall.end - minY; y++)
                    {
                        grid[y, wall.same - minX] = '#';
                    }
                }
                else
                {
                    for (int x = wall.start - minX; x <= wall.end - minX; x++)
                    {
                        grid[wall.same - minY, x] = '#';
                    }
                }
            }

            return (grid, origin);
        }
    }
}
