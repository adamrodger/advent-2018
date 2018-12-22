using System;

namespace AdventOfCode
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Solver for Day 22
    /// </summary>
    public class Day22
    {
        public int Part1(int depth, (int x, int y) target)
        {
            int[,] map = BuildMap(depth, target);

            //Print(map);

            // add up risk
            int risk = map.Where((x, y, _) => x <= target.x && y <= target.y).Select(erosion => erosion % 3).Sum();

            return risk;
        }

        public int Part2(int depth, (int x, int y) target)
        {
            int[,] map = BuildMap(depth, target);

            Print(map);

            var allowedTools = new Dictionary<int, Tool[]>
            {
                [0] = new[] { Tool.Climbing, Tool.Torch }, // rocky
                [1] = new[] { Tool.Climbing, Tool.Neither }, // wet
                [2] = new[] { Tool.Torch, Tool.Neither } // narrow
            };

            // build up a graph which depends on which tool we currently have equipped
            int Heuristic((int x, int y, Tool tool) current, (int x, int y, Tool tool) end)
            {
                int manhattan = Math.Abs(current.x - end.x) + Math.Abs(current.y - end.y);
                return manhattan;// + (current.tool != end.tool ? 7 : 0);
            }
            var graph = new Graph<(int x, int y, Tool tool)>(Heuristic);

            map.ForEach((x, y, cell) =>
            {
                foreach (Tool tool in allowedTools[cell % 3])
                {
                    // add two-way edge to self with cost 7 for changing tools
                    foreach (Tool otherTool in allowedTools[cell % 3])
                    {
                        if (tool == otherTool)
                        {
                            continue;
                        }

                        graph.AddVertex((x, y, tool), (x, y, otherTool), cost: 7);
                    }

                    // if the adjacent square allows this tool, add an edge with cost 1
                    // else the adjacent square doesn't allow this tool, add an edge with cost 1 + 7 for changing tool
                    foreach ((int x, int y) move in new[] { (0, -1), (-1, 0), (1, 0), (0, 1) })
                    {
                        int dx = x + move.x;
                        int dy = y + move.y;

                        if (dy < 0 || dy >= map.GetLength(0) || dx < 0 || dx >= map.GetLength(1))
                        {
                            // fell off the map
                            continue;
                        }

                        var next = map[dy, dx] % 3;

                        foreach (Tool nextTool in allowedTools[next])
                        {
                            if (nextTool != tool)
                            {
                                // can only move to other squares with the same tool
                                continue;
                            }

                            graph.AddVertex((x, y, tool), (dx, dy, nextTool));
                        }
                    }
                }
            });

            List<((int x, int y, Tool tool) node, int distance)> path = graph.GetShortestPath((0, 0, Tool.Torch), (target.x, target.y, Tool.Torch));

            // guessed 1080 -- too low
            // guessed 1090 -- too low
            // actual answer 1092
            return path.Last().distance;
        }

        private static int[,] BuildMap(int depth, (int x, int y) target)
        {
            int[,] map = new int[target.y + 15, target.x + 15];

            // build map
            map.ForEach((x, y, _) =>
            {
                int index;

                if (x == 0 && y == 0)
                {
                    index = 0;
                }
                else if (x == target.x && y == target.y)
                {
                    index = 0;
                }
                else if (y == 0)
                {
                    index = x * 16807;
                }
                else if (x == 0)
                {
                    index = y * 48271;
                }
                else
                {
                    index = map[y - 1, x] * map[y, x - 1];
                }

                int erosion = (index + depth) % 20183;
                map[y, x] = erosion;
            });

            return map;
        }

        private static void Print(int[,] map)
        {
            var builder = new StringBuilder(map.GetLength(0) * (map.GetLength(1) + Environment.NewLine.Length));
            map.ForEach(cell =>
            {
                switch (cell % 3)
                {
                    case 0:
                        builder.Append('.');
                        break;
                    case 1:
                        builder.Append('=');
                        break;
                    case 2:
                        builder.Append('|');
                        break;
                }
            }, () => builder.AppendLine());
            builder.AppendLine();

            Debug.Write(builder.ToString());
        }
    }

    /// <summary>
    /// Represents a tool used when exploring the cave
    /// </summary>
    public enum Tool
    {
        Neither, Torch, Climbing
    }
}
