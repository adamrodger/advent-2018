using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
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

            var minX = Math.Min(verticals.Min(v => v.same), horizontals.Min(h => h.start)) - 1; // room to flow off the side
            var maxX = Math.Max(verticals.Max(v => v.same), horizontals.Max(h => h.end)) + 1;

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
