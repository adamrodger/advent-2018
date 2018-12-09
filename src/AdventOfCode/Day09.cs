using System;

namespace AdventOfCode
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Solver for Day 09
    /// </summary>
    public class Day09
    {
        public long Solve(int playerCount, int maxMarble)
        {
            long[] players = new long[playerCount];
            LinkedList<int> marbles = new LinkedList<int>();

            // for 0 to maxMarbles:
            //     if i % 23 != 0:
            //         place marble in circular buffer
            //     else:
            //         score[player] += i;
            //         score[player] += (marble 7 to the left)
            //         current = marble right of removed marble

            // initialise
            var current = marbles.AddFirst(0);

            foreach (int marble in Enumerable.Range(1, maxMarble))
            {
                int player = marble % playerCount;

                if (marble % 23 > 0)
                {
                    // not a scoring marble, just place it
                    var next = current.Next ?? marbles.First; // circle round
                    current = marbles.AddAfter(next, marble);
                }
                else
                {
                    // scoring marble
                    players[player] += marble;

                    // unwind 7 times
                    for (int i = 0; i < 7; i++)
                    {
                        current = current.Previous ?? marbles.Last; // circle round
                    }

                    var remove = current;
                    players[player] += remove.Value;
                    current = remove.Next ?? marbles.First; // circle around
                    marbles.Remove(remove);
                }
            }

            return players.Max();
        }
    }
}
