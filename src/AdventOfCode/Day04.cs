using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 4
    /// </summary>
    public class Day04
    {
        /*
         * [1518-11-01 00:00] Guard #10 begins shift
         * [1518-11-01 00:05] falls asleep
         * [1518-11-01 00:25] wakes up
         */
        public (int, int) Solve(string[] lines)
        {
            var guards = new Dictionary<int, Guard>();
            int currentId = -1;

            foreach (string line in lines.OrderBy(s => s))
            {
                var date = DateTime.Parse(line.Substring(1, 16));
                bool wake = line.Contains("wake");
                bool sleep = line.Contains("asleep");
                currentId = line.Contains('#') ? int.Parse(line.Split('#')[1].Split(' ')[0]) : currentId;

                if (!guards.TryGetValue(currentId, out Guard guard))
                {
                    guard = new Guard { Id = currentId };
                    guards[currentId] = guard;
                }

                if (wake)
                {
                    guard.Wake(date);
                }
                else if (sleep)
                {
                    guard.Sleep(date);
                }
                else
                {
                    guard.StartShift();
                }
            }

            Guard part1 = guards.Values.OrderByDescending(g => g.TotalMinsAsleep).First();
            Guard part2 = guards.Values.OrderByDescending(g => g.Minutes.Max()).First();

            return (part1.Id * part1.LongestMinute(), part2.Id * part2.LongestMinute());
        }

        public class Guard
        {
            public int Id { get; set; }

            public bool Asleep { get; private set; }

            public DateTime LastTransitionTime { get; private set; }

            public int TotalMinsAsleep { get; private set; }

            public int[] Minutes { get; }

            public Guard()
            {
                this.Minutes = new int[60];
            }

            public void StartShift()
            {
                this.LastTransitionTime = DateTime.MinValue;
                this.Asleep = false;
            }

            public void Wake(DateTime time)
            {
                if (!this.Asleep)
                {
                    throw new InvalidOperationException();
                }

                // mark minutes they were asleep
                for (int i = this.LastTransitionTime.Minute; i < time.Minute; i++)
                {
                    this.Minutes[i]++;
                    this.TotalMinsAsleep++;
                }

                this.LastTransitionTime = time;
            }

            public void Sleep(DateTime time)
            {
                this.Asleep = true;
                this.LastTransitionTime = time;
            }

            public int LongestMinute()
            {
                int maxMinute = -1;
                int maxCount = -1;

                for (int i = 0; i < this.Minutes.Length; i++)
                {
                    if (this.Minutes[i] > maxCount)
                    {
                        maxCount = this.Minutes[i];
                        maxMinute = i;
                    }
                }

                return maxMinute;
            }

            public override string ToString()
            {
                return $"Id: {this.Id}, Total: {this.TotalMinsAsleep}, Longest: {this.LongestMinute()}";
            }
        }
    }
}
