using System;

namespace AdventOfCode
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

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

            var allowedMoves = new Dictionary<Tool, int[]>
            {
                [Tool.Neither] = new[] { 1, 2 }, // wet or narrow
                [Tool.Torch] = new[] { 0, 2 }, // rocky or narrow
                [Tool.Climbing] = new[] { 0, 1 } // rocky or wet
            };

            // build up a graph which depends on which tool we currently have equipped and where we can move to
            var graph = new Graph<(int x, int y, Tool tool)>();

            map.ForEach((x, y, cell) =>
            {
                foreach (Tool tool in allowedTools[cell % 3])
                {
                    // if the adjacent square allows this tool, add an edge with cost 1
                    // else the adjacent square doesn't allow this tool, add an edge with cost 7

                    if (y > 0) // up
                    {
                        var next = map[y - 1, x] % 3;

                        foreach (Tool nextTool in allowedTools[next])
                        {
                            graph.AddVertex((x, y, tool), (x, y - 1, nextTool), nextTool == tool ? 1 : 7);
                        }
                    }

                    if (x > 0) // left
                    {
                        var next = map[y, x - 1] % 3;

                        foreach (Tool nextTool in allowedTools[next])
                        {
                            graph.AddVertex((x, y, tool), (x - 1, y, nextTool), nextTool == tool ? 1 : 7);
                        }
                    }

                    if (x + 1 < map.GetLength(1)) // right
                    {
                        var next = map[y, x + 1] % 3;

                        foreach (Tool nextTool in allowedTools[next])
                        {
                            graph.AddVertex((x, y, tool), (x + 1, y, nextTool), nextTool == tool ? 1 : 7);
                        }
                    }

                    if (y + 1 < map.GetLength(0)) // down
                    {
                        var next = map[y + 1, x] % 3;

                        foreach (Tool nextTool in allowedTools[next])
                        {
                            graph.AddVertex((x, y, tool), (x, y + 1, nextTool), nextTool == tool ? 1 : 7);
                        }
                    }
                }
            });

            (var _, int cost) = graph.GetShortestPath((0, 0, Tool.Torch), (target.x, target.y, Tool.Torch));

            // guessed 1080 -- too low
            return cost;
        }

        private static int[,] BuildMap(int depth, (int x, int y) target)
        {
            int[,] map = new int[target.y + 30, target.x + 30];

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

    public enum Tool
    {
        Neither, Torch, Climbing
    }

    public class Graph<TNode>
    {
        public Dictionary<TNode, List<(TNode node, int cost)>> Vertices { get; }

        public Graph()
        {
            this.Vertices = new Dictionary<TNode, List<(TNode next, int cost)>>();
        }

        /// <summary>
        /// Create a path from start to end with the given cost (default to 1)
        /// </summary>
        /// <param name="start">Start node</param>
        /// <param name="end">End node</param>
        /// <param name="cost">Cost of moving from start to end (default to 1)</param>
        public void AddVertex(TNode start, TNode end, int cost = 1)
        {
            this.Vertices.GetOrCreate(start).Add((end, cost));
        }

        public (List<TNode> path, int cost) GetShortestPath(TNode start, TNode finish)
        {
            var previous = new Dictionary<TNode, TNode>();
            var distances = new Dictionary<TNode, int> { [start] = 0 };
            var search = new List<TNode> { start} ;

            List<TNode> path = null;
            int pathCost = -1;

            while (search.Any())
            {
                // sort by distance
                search.Sort((n1, n2) => distances[n1] - distances[n2]);

                // pop closest node
                TNode current = search.First();
                search.Remove(current);

                // check if reached destination
                if (current.Equals(finish))
                {
                    path = new List<TNode>();
                    pathCost = distances[current];

                    while (previous.ContainsKey(current))
                    {
                        path.Add(current);
                        current = previous[current];
                    }

                    break;
                }

                if (!distances.ContainsKey(current))
                {
                    // don't know how to get to this node yet - how did it get queued?!
                    Debug.Fail("How did we queue a node when we've not been there yet?");
                }

                // walk outwards along edges to see if we can find a closer node
                foreach ((TNode next, int cost) in this.Vertices[current].OrderBy(edge => edge.cost))
                {
                    var distance = distances[current] + cost;

                    // found the first or a closer route to the next node (from the current node)
                    if (!distances.ContainsKey(next) || distance < distances[next])
                    {
                        distances[next] = distance;
                        previous[next] = current;

                        search.Add(next);
                    }
                }
            }

            Debug.Assert(path != null, "No path found");

            return (path, pathCost);
        }
    }
}
