using System;

namespace AdventOfCode
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Solver for Day 08
    /// </summary>
    public class Day08
    {
        public static char NextId = 'A';

        /// <summary>
        /// A header, which is always exactly two numbers:
        ///     The quantity of child nodes.
        ///     The quantity of metadata entries.
        /// Zero or more child nodes(as specified in the header).
        /// One or more metadata entries(as specified in the header).
        /// </summary>
        /// <example>
        /// 2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2
        /// </example>
        /// <param name="input">File input</param>
        /// <returns>Part 1 and part 2</returns>
        public (int, int) Solve(string input)
        {
            var numbers = new Queue<int>(input.Split(' ').Select(int.Parse));

            Node root = this.BuildTree(numbers);

            int part1 = this.SumMetadata(root);
            int part2 = this.ScoreMetadata(root);

            return (part1, part2);
        }

        /// <summary>
        /// Parse the input to generate the node tree with attached metadata
        /// </summary>
        /// <param name="input">Numeric input</param>
        /// <returns>Fully populated root node</returns>
        private Node BuildTree(Queue<int> input)
        {
            var node = new Node { Id = NextId++ };

            int children = input.Dequeue();
            int metadata = input.Dequeue();

            // recurse to add child nodes
            for (int i = 0; i < children; i++)
            {
                Node childNode = this.BuildTree(input);
                node.ChildNodes.Add(childNode);
            }

            // record the metadata for this node
            for (int i = 0; i < metadata; i++)
            {
                node.Metadata.Add(input.Dequeue());
            }

            return node;
        }

        /// <summary>
        /// Recurse into child nodes and sum all the metadata
        /// </summary>
        /// <param name="node">Current node</param>
        /// <returns>Total of this node and all child nodes</returns>
        private int SumMetadata(Node node)
        {
            return node.Metadata.Sum() + node.ChildNodes.Select(n => this.SumMetadata(n)).Sum();
        }

        /// <summary>
        /// Score each node where:
        ///
        /// - Leaf nodes are scored as the sum of their metadata
        /// - Branch nodes are the sum of their child nodes, where the branch metadata indicates which child to pick (if relevant)
        /// </summary>
        /// <param name="node">Current node</param>
        /// <returns>Score of this node, having taken into account child nodes</returns>
        private int ScoreMetadata(Node node)
        {
            if (!node.ChildNodes.Any())
            {
                // leaf nodes are just the sum of metadata
                return node.Metadata.Sum();
            }

            // branch nodes are the sum of child nodes at the metadata indices
            return node.Metadata
                       .Where(m => m > 0 && m <= node.ChildNodes.Count) // metadata is only valid if it points to a child node (1-indexed)
                       .Select(i => this.ScoreMetadata(node.ChildNodes[i - 1]))
                       .Sum();
        }

        public class Node
        {
            public char Id { get; set; }

            public List<int> Metadata { get; set; } = new List<int>();

            public List<Node> ChildNodes { get; set; } = new List<Node>();

            /// <summary>Returns a string that represents the current object.</summary>
            /// <returns>A string that represents the current object.</returns>
            public override string ToString()
            {
                return $"{this.Id}, Children: {this.ChildNodes.Count}, Metadata: {this.Metadata.Count}";
            }
        }
    }
}
