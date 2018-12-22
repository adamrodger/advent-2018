using System;

namespace AdventOfCode
{
    using System.Collections.Generic;

    /// <summary>
    /// Solver for Day 5
    /// </summary>
    public class Day05
    {
        public int React(string input)
        {
            List<char> reaction = new List<char>(input);
            bool found = false;

            do
            {
                found = false;

                for (int i = 0; i < reaction.Count - 1; i++)
                {
                    if (Math.Abs(reaction[i] - reaction[i + 1]) == 32)
                    {
                        reaction.RemoveAt(i); // remove this index
                        reaction.RemoveAt(i); // remove next (which just shuffled to this index)
                        found = true;
                        i--;
                    }
                }
            } while (found);

            return reaction.Count;
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
