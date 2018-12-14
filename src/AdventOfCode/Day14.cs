using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 14
    /// </summary>
    public class Day14
    {
        public string Part1(int input)
        {
            var recipes = new List<int>(input + 10) { 3, 7 };

            int position1 = 0;
            int position2 = 1;

            while (recipes.Count < input + 10)
            {
                int score1 = recipes[position1];
                int score2 = recipes[position2];

                int next = score1 + score2;

                if (next > 9)
                {
                    recipes.Add(next / 10);
                }

                recipes.Add(next % 10);

                position1 = (position1 + 1 + score1) % recipes.Count;
                position2 = (position2 + 1 + score2) % recipes.Count;
            }
            
            var last10 = recipes.Skip(input).Take(10);
            var result = string.Join(string.Empty, last10.Select(i => i.ToString()));
            return result;
        }

        public int Part2(string input)
        {
            var search = input.Select(c => int.Parse(c.ToString())).ToArray();
            var recipes = new List<int>(25000000) { 3, 7 };

            int position1 = 0;
            int position2 = 1;

            while (true)
            {
                int score1 = recipes[position1];
                int score2 = recipes[position2];

                int next = score1 + score2;

                int part2;

                if (next > 9)
                {
                    recipes.Add(next / 10);
                    if (CheckSearchTerm(out part2)) return part2;
                }

                recipes.Add(next % 10);
                if (CheckSearchTerm(out part2)) return part2;

                position1 = (position1 + 1 + score1) % recipes.Count;
                position2 = (position2 + 1 + score2) % recipes.Count;
            }

            bool CheckSearchTerm(out int result)
            {
                if (recipes.Count < search.Length)
                {
                    result = -1;
                    return false;
                }

                int offset = recipes.Count - search.Length;

                for (int i = 0; i < search.Length; i++)
                {
                    if (recipes[offset + i] != search[i])
                    {
                        result = -1;
                        return false;
                    }
                }

                result = recipes.Count - input.Length;
                return true;
            }
        }
    }
}
