using System;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 5
    /// </summary>
    public class Day05
    {

        public int React(string input)
        {
            bool found = false;

            do
            {
                found = false;

                for (int i = 0; i < input.Length - 1; i++)
                {
                    if (Math.Abs(input[i] - input[i + 1]) == 32)
                    {
                        input = input.Remove(i, 2);
                        found = true;
                        i--;
                    }
                }
            } while (found);

            return input.Length;
        }

        public int Part2(string input)
        {
            int shortest = int.MaxValue;

            for (char c = 'a'; c <= 'z'; c++)
            {
                string replaced = input.Replace(c.ToString(), string.Empty).Replace(c.ToString().ToUpper(), string.Empty);
                int length = this.React(replaced);

                if (length < shortest)
                {
                    shortest = length;
                }
            }

            return shortest;
        }
    }
}
