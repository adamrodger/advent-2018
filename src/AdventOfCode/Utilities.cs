namespace AdventOfCode
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class Utilities
    {
        public static int[] Numbers(this string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\d+");
            return matches.Cast<Match>().Select(m => m.Value).Select(int.Parse).ToArray();
        }

        public static void ForEach<T>(this T[,] grid, Action<T> cellAction, Action lineAction = null)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    cellAction(grid[y, x]);
                }

                lineAction?.Invoke();
            }
        }

        public static void ForEach<T>(this T[,] grid, Action<int, int, T> action)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    action(x, y, grid[y, x]);
                }
            }
        }

        public static void ForEachChar(this string[] input, Action<int, int, char> cellAction)
        {
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    cellAction(x, y, input[y][x]);
                }
            }
        }

        public static IEnumerable<T> Search<T>(this T[,] grid, Predicate<T> predicate)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    T item = grid[y, x];

                    if (predicate(item))
                    {
                        yield return item;
                    }
                }
            }
        }

        public static void Print<T>(this T[,] grid)
        {
            if (!Debugger.IsAttached)
            {
                return;
            }

            var builder = new StringBuilder(grid.GetLength(0) * (grid.GetLength(1) + Environment.NewLine.Length));
            grid.ForEach(cell => builder.Append(cell), () => builder.AppendLine());
            builder.AppendLine();

            Debug.Write(builder.ToString());
        }

        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key) where TValue : new()
        {
            if (!@this.TryGetValue(key, out TValue value))
            {
                value = new TValue();
                @this.Add(key, value);
            }

            return value;
        }
    }
}
