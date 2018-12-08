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
            (int part1, int part2) = SumMetadata(numbers, null, ref index);

            return (part1, part2);
        }

        public (int, int) SumMetadata(int[] input, Node parent, ref int offset)
        {
            var node = new Node
            {
                Id = NextId,
                Children = input[offset++],
                MetadataSize = input[offset++]
            };

            if (parent != null)
            {
                parent.ChildNodes.Add(node);
            }

            NextId = (char)((int)NextId + 1);

            for (int i = 0; i <  node.Children; i++)
            {
                // recurse into children
                node.MetadataSum += this.SumMetadata(input, node, ref offset).Item1;
            }

            for (int i = offset; i < offset + node.MetadataSize; i++)
            {
                node.MetadataValues.Add(input[i]);
                node.MetadataSum += input[i];
            }

            offset += node.MetadataSize;

            int childTotal = 0;

            if (offset == input.Length)
            {
                CalculateTreeSum(node, ref childTotal);
            }

            return (node.MetadataSum, childTotal);
        }

        private void CalculateTreeSum(Node node, ref int total)
        {
            // we've reached the end, calc the root node
            foreach (int childIndex in node.MetadataValues)
            {
                if (childIndex > 0 && node.ChildNodes.Count > 0 && childIndex <= node.ChildNodes.Count)
                {
                    var childNode = node.ChildNodes.ElementAt(childIndex - 1);

                    if (childNode.Children > 0)
                    {
                        CalculateTreeSum(childNode, ref total);
                    }
                    else
                    {
                        total += childNode.MetadataValues.Sum();
                    }
                }
            }
        }

        public class Node
        {
            public char Id { get; set; }

            public int MetadataSize { get; set; }

            public int MetadataSum { get; set; }

            public int Children { get; set; }

            public List<int> MetadataValues { get; set; } = new List<int>();

            public List<Node> ChildNodes { get; set; } = new List<Node>();

            /// <summary>Returns a string that represents the current object.</summary>
            /// <returns>A string that represents the current object.</returns>
            public override string ToString()
            {
                return $"{this.Id}, Children: {this.Children}, Metadata: {this.MetadataSize}, Current Sum: {this.MetadataSum}";
            }
        }
    }
}
