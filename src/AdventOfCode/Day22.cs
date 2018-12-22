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

            // build up a graph which depends on which tool we currently have equipped
            Func<(int x, int y, Tool tool), (int x, int y, Tool tool), int> manhattanHeuristic = (n1, n2) => Math.Abs(n1.x - n2.x) + Math.Abs(n1.y - n2.y);
            var graph = new Graph<(int x, int y, Tool tool)>(manhattanHeuristic);

            map.ForEach((x, y, cell) =>
            {
                foreach (Tool tool in allowedTools[cell % 3])
                {
                    // if the adjacent square allows this tool, add an edge with cost 1
                    // else the adjacent square doesn't allow this tool, add an edge with cost 1 + 7 for changing tool
                    foreach ((int x, int y) move in new[] { (0, -1), (-1, 0), (1, 0), (0, 1) })
                    {
                        int dx = x + move.x;
                        int dy = y + move.y;

                        if (dy < 0 || dy >= map.GetLength(0) || dx < 0 || dx >= map.GetLength(1))
                        {
                            continue;
                        }

                        var next = map[dy, dx] % 3;

                        foreach (Tool nextTool in allowedTools[next])
                        {
                            graph.AddVertex((x, y, tool), (dx, dy, nextTool), nextTool == tool ? 1 : 8);
                        }
                    }
                }
            });

            (_, int cost) = graph.GetShortestPath((0, 0, Tool.Torch), (target.x, target.y, Tool.Torch));

            // guessed 1080 -- too low
            // guessed 1090 -- too low
            return cost;
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

    /// <summary>
    /// A generic graph of nodes with edges between them
    /// </summary>
    /// <typeparam name="TNode">Type of the node</typeparam>
    public class Graph<TNode>
    {
        /// <summary>
        /// Nodes in the graph
        /// </summary>
        public Dictionary<TNode, List<(TNode node, int cost)>> Vertices { get; }

        /// <summary>
        /// Additional heuristic used to order nodes (effectively turns Dijkstra into A* algorithm) 
        /// </summary>
        private readonly Func<TNode, TNode, int> heuristic;

        /// <summary>
        /// Initialises a new instance of the <see cref="Graph"/> class.
        /// </summary>
        public Graph(Func<TNode, TNode, int> heuristic = null)
        {
            this.Vertices = new Dictionary<TNode, List<(TNode next, int cost)>>();
            this.heuristic = heuristic ?? ((_, __) => 0);
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

        /// <summary>
        /// Find the shortest path from the start node to the end node
        /// </summary>
        /// <param name="start">Start node</param>
        /// <param name="finish">End node</param>
        /// <returns>Shortest path and cost of that path</returns>
        public (List<TNode> path, int cost) GetShortestPath(TNode start, TNode finish)
        {
            var previous = new Dictionary<TNode, TNode>();
            var distances = new Dictionary<TNode, int> { [start] = 0 };
            var search = new List<TNode> { start };

            List<TNode> path = null;
            int pathCost = -1;

            while (search.Any())
            {
                // sort by distance to get the most promising path at the moment
                search.Sort((n1, n2) => (distances[n1] + this.heuristic(n1, finish)) - (distances[n2] + this.heuristic(n2, finish)));

                // pop closest node
                TNode current = search.First();
                search.Remove(current);

                // check if we've reached destination
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

                // walk outwards along edges to see if we can find a closer node
                foreach ((TNode next, int cost) in this.Vertices[current])
                {
                    var newDistance = distances[current] + cost;

                    // found the first or a closer route to the next node (from the current node)
                    if (!distances.ContainsKey(next) || newDistance < distances[next])
                    {
                        distances[next] = newDistance;
                        previous[next] = current;

                        search.Add(next);
                    }
                }
            }

            Debug.Assert(path != null, "No path found");

            path.Reverse();
            return (path, pathCost);
        }
    }
}
