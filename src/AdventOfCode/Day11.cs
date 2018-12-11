using System;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 11
    /// </summary>
    public class Day11
    {
        public (int, int) Part1(int input)
        {
            int[,] grid = BuildGrid(input);

            (int x, int y) largest = (0, 0);
            int largestPower = 0;

            // search grid
            for (int y = 0; y < grid.GetLength(1) - 2; y++)
            {
                for (int x = 0; x < grid.GetLength(0) - 2; x++)
                {
                    int totalPower = grid[x, y + 0] + grid[x + 1, y + 0] + grid[x + 2, y + 0] +
                                     grid[x, y + 1] + grid[x + 1, y + 1] + grid[x + 2, y + 1] +
                                     grid[x, y + 2] + grid[x + 1, y + 2] + grid[x + 2, y + 2];

                    if (totalPower > largestPower)
                    {
                        largestPower = totalPower;
                        largest = (x , y);
                    }
                }
            }

            return largest;
        }

        public (int x, int y, int size) Part2(int input)
        {
            int[,] grid = BuildGrid(input);

            int[,,] memoised = new int[300, 300, 300];

            (int x, int y, int size) largest = (0, 0, 0);
            int largestPower = 0;

            // search grid
            for (int size = 1; size < grid.GetLength(0); size++)
            {
                for (int y = 0; y < grid.GetLength(1) - size; y++)
                {
                    for (int x = 0; x < grid.GetLength(0) - size; x++)
                    {
                        // don't redo all the work of the previous square size, just use the memoised version
                        // then add the extra row and column for the new size
                        int totalPower = memoised[x, y, size - 1];
                        
                        // add the extra row
                        int rowIndex = y + size - 1;
                        for (int dx = x; dx < x + size; dx++)
                        {
                            totalPower += grid[dx, rowIndex];
                        }

                        // add the extra column
                        int columnIndex = x + size - 1;
                        for (int dy = y; dy < y + size; dy++)
                        {
                            totalPower += grid[columnIndex, dy];
                        }

                        // memoise for next size iteration
                        memoised[x, y, size] = totalPower;

                        // keep track of largest so far
                        if (totalPower > largestPower)
                        {
                            largestPower = totalPower;
                            largest = (x, y, size);
                        }
                    }
                }
            }

            return largest;
        }

        private static int[,] BuildGrid(int input)
        {
            int[,] grid = new int[300, 300];

            // build grid
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    int rack = x + 10;

                    int power = rack * y;
                    power += input;
                    power *= rack;
                    power = (power % 1000) / 100;
                    power -= 5;

                    grid[x, y] = power;
                }
            }

            return grid;
        }
    }
}
