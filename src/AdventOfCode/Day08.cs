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
        /// 
        /// A header, which is always exactly two numbers:
        ///     The quantity of child nodes.
        ///     The quantity of metadata entries.
        /// Zero or more child nodes(as specified in the header).
        /// One or more metadata entries(as specified in the header).
        ///
        /// 2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public (int, int) Solve(string input)
        {
            int[] numbers = input.Split(' ').Select(int.Parse).ToArray();
            int index = 0;
            (int part1, int part2) = this.SumMetadata(numbers, null, ref index);

            return (part1, part2);
        }

        public (int, int) SumMetadata(int[] input, Node parent, ref int offset)
        {
            var node = new Node { Id = NextId++ };

            // root node has no parent
            if (parent != null)
            {
                parent.ChildNodes.Add(node);
            }

            // get the header values and advance the offset
            int children = input[offset++];
            int metadata = input[offset++];

            // recurse into children starting at the new offset
            for (int i = 0; i < children; i++)
            {
                node.MetadataSum += this.SumMetadata(input, node, ref offset).Item1;
            }

            // add up metadata from the offset
            for (int i = offset; i < offset + metadata; i++)
            {
                node.MetadataValues.Add(input[i]);
                node.MetadataSum += input[i];
            }

            // skip the offset over the metadata to the next node
            offset += node.MetadataValues.Count;

            int childTotal = 0;

            // we've reached the end, calc the root node
            if (offset == input.Length)
            {
                this.CalculateTreeSum(node, ref childTotal);
            }

            return (node.MetadataSum, childTotal);
        }

        /// <summary>
        /// Use the metadata to sum child node values recursively
        ///
        /// Leaf nodes are the sum of their metadata values
        /// Branch nodes are the sum of their child nodes at the indices of their metadata (if they exist)
        /// </summary>
        private void CalculateTreeSum(Node node, ref int total)
        {
            foreach (int childIndex in node.MetadataValues)
            {
                if (!node.ChildNodes.Any() || childIndex > node.ChildNodes.Count)
                {
                    continue;
                }

                // child index is 1-based, not 0-based
                var childNode = node.ChildNodes.ElementAt(childIndex - 1);

                if (childNode.ChildNodes.Any())
                {
                    // recurse
                    this.CalculateTreeSum(childNode, ref total);
                }
                else
                {
                    // leaf nodes are the sum of their metadata
                    total += childNode.MetadataValues.Sum();
                }
            }
        }

        public class Node
        {
            public char Id { get; set; }

            public int MetadataSum { get; set; }

            public List<int> MetadataValues { get; set; } = new List<int>();

            public List<Node> ChildNodes { get; set; } = new List<Node>();

            /// <summary>Returns a string that represents the current object.</summary>
            /// <returns>A string that represents the current object.</returns>
            public override string ToString()
            {
                return $"{this.Id}, Children: {this.ChildNodes.Count}, Metadata: {this.MetadataValues.Count}, Current Sum: {this.MetadataSum}";
            }
        }
    }
}
