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
                var adjacent = map.Adjacent4(x, y);

                foreach (Tool tool in allowedTools[cell])
                {
                    // if the adjacent square allows this tool, add an edge with cost 1

                    // if the adjacent square doesn't allow this tool, add an edge with cost 7
                }
            });

            (var path, int cost) = graph.GetShortestPath((0, 0, Tool.Torch), (target.x, target.y, Tool.Torch));
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

    public class Graph<TNode> where TNode : IEquatable<TNode>
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
            var distances = new Dictionary<TNode, int>();
            var nodes = new List<TNode>();

            List<TNode> path = null;
            int pathCost = -1;

            // initialise everything to +inf except start node
            foreach (var node in this.Vertices.Keys)
            {
                if (node.Equals(start))
                {
                    distances[node] = 0;
                }
                else
                {
                    distances[node] = int.MaxValue;
                }

                nodes.Add(node);
            }

            while (nodes.Count != 0)
            {
                // sort by distance
                nodes.Sort((n1, n2) => distances[n1] - distances[n2]);

                // pop closest node
                TNode current = nodes.First();
                nodes.Remove(current);

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

                /*if (distances[current] == int.MaxValue)
                {
                    break;
                }*/

                // walk outwards along edges to see if we can find a closer node
                foreach ((TNode next, int cost) in this.Vertices[current].OrderBy(edge => edge.cost))
                {
                    var distance = distances[current] + cost;

                    // found a closer route back to the current node
                    if (distance < distances[next])
                    {
                        distances[next] = distance;
                        previous[next] = current;
                    }
                }
            }

            return (path, pathCost);
        }
    }
}
