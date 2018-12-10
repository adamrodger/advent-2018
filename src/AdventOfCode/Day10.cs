using System;
using System.Linq;
using System.Text;
using MoreLinq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 10
    /// </summary>
    public class Day10
    {
        public (int, string) Solve(string[] input, int threshold)
        {
            var pixels = input.Select(i => new Pixel(i)).ToArray();

            bool found = false;
            int seconds = 0;

            while (!found)
            {
                seconds++;

                foreach (Pixel pixel in pixels)
                {
                    pixel.Move();
                }

                // try to detect a straight vertical line over the detection threshold for some hacky OCR
                // this will match [B, D, E, F, H, I, K, L, M, N, P, R, T], but not [A, C, G, O, Q, S, V, W, X, Y, Z]
                // , and J and U are sore points, so hopefully our result contains at least one of the detectable chars :D
                var groups = pixels.GroupBy(p => p.X);
                var column = groups.FirstOrDefault(g => g.Count() >= threshold);

                if (column == null)
                {
                    continue;
                }

                // check for a straight line
                int columnMin = column.Min(m => m.Y);
                int columnMax = column.Max(c => c.Y);

                // check if everything is within -T to +T
                found = Math.Abs(columnMax) - Math.Abs(columnMin) <= threshold;
            }

            // print result
            int minX = pixels.Min(p => p.X);
            int maxX = pixels.Max(p => p.X);
            int minY = pixels.Min(p => p.Y);
            int maxY = pixels.Max(p => p.Y);

            var output = new StringBuilder();
            output.Append(Environment.NewLine);

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (pixels.Any(p => p.X == x && p.Y == y))
                    {
                        output.Append('#');
                    }
                    else
                    {
                        output.Append(' ');
                    }
                }

                output.Append(Environment.NewLine);
            }
            
            return (seconds, output.ToString());
        }

        public class Pixel
        {
            public int X { get; set; }

            public int Y { get; set; }

            public int VX { get; set; }

            public int VY { get; set; }

            public Pixel(string input)
            {
                // position=< 32316,  43048> velocity=<-3, -4>
                var coords = input.Split('>')[0].Split('<')[1].Split(',');
                var velocity = input.Split('<')[2].Replace(">", string.Empty).Split(',');

                this.X = int.Parse(coords[0].Trim());
                this.Y = int.Parse(coords[1].Trim());
                this.VX = int.Parse(velocity[0].Trim());
                this.VY = int.Parse(velocity[1].Trim());
            }

            public void Move()
            {
                this.X += this.VX;
                this.Y += this.VY;
            }
        }
    }
}
