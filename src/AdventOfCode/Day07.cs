using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 07
    /// </summary>
    public class Day07
    {
        /// <summary>
        /// Step C must be finished before step A can begin.
        /// </summary>
        public string Part1(string[] input)
        {
            Dictionary<string, Node> nodes = BuildNodes(input);

            List<string> result = new List<string>();
            List<Node> remaining = new List<Node>(nodes.Values);

            do
            {
                // chop off next ready node
                var ready = remaining.Where(r => r.Incoming.Count == 0).OrderBy(n => n.Id).First();
                remaining.Remove(ready);
                result.Add(ready.Id);

                // remove that node from any references
                foreach (Node node in remaining)
                {
                    node.Incoming.RemoveAll(n => n == ready.Id);
                }
            } while (remaining.Any());

            return string.Join(string.Empty, result);
        }

        public int Part2(string[] input, int seconds, int workerCount)
        {
            Dictionary<string, Node> nodes = BuildNodes(input, seconds);

            var todo = new List<Node>(nodes.Values);
            var inProgress = new List<Node>();
            List<string> done = new List<string>();

            Node[] workers = new Node[workerCount];
            int counter = 0;

            Debug.WriteLine($"Second\t{string.Join("\t", workers.Select((_, i) => $"Worker {i}"))}\tDone");

            do
            {
                // subtract a remaining second off every in progress node and remove if necessary
                for (int i = 0; i < workers.Length; i++)
                {
                    var worker = workers[i];

                    if (worker == null)
                    {
                        continue;
                    }

                    worker.Remaining--;

                    // check if finished
                    if (worker.Remaining == 0)
                    {
                        workers[i] = null;
                        done.Add(worker.Id);

                        foreach (Node node in todo)
                        {
                            node.Incoming.RemoveAll(n => n == worker.Id);
                        }
                        inProgress.Remove(worker);
                    }
                }

                for (int i = 0; i < workers.Length; i++)
                {
                    if (workers[i] != null)
                    {
                        // already working
                        continue;
                    }

                    // allocate a worker
                    var ready = todo.Where(r => r.Incoming.Count == 0).OrderBy(n => n.Id).FirstOrDefault();

                    if (ready == null)
                    {
                        // nothing is ready, worker stays idle
                        break;
                    }

                    workers[i] = ready;
                    todo.Remove(ready);
                    inProgress.Add(ready);
                }

                Debug.WriteLine($"{counter}\t\t{string.Join("\t\t\t", workers.Select(n => n?.Id ?? "."))}\t\t\t{string.Join("", done)}");

                counter++;
            } while (todo.Any() || inProgress.Any()); // work left or still in progress

            return counter - 1;
        }

        private static Dictionary<string, Node> BuildNodes(string[] input, int seconds = 0)
        {
            var nodes = new Dictionary<string, Node>();

            foreach (string line in input)
            {
                var id1 = line.Substring(5, 1);
                var id2 = line.Substring(36, 1);

                if (!nodes.TryGetValue(id1, out Node node1))
                {
                    node1 = new Node(id1, seconds);
                    nodes[id1] = node1;
                }

                if (!nodes.TryGetValue(id2, out Node node2))
                {
                    node2 = new Node(id2, seconds);
                    nodes[id2] = node2;
                }

                node2.Incoming.Add(node1.Id);
            }

            return nodes;
        }

        public class Node
        {
            public string Id { get; set; }

            public List<string> Incoming { get; set; } = new List<string>();

            public int Remaining { get; set; }

            public Node(string id, int secondsOffset = 0)
            {
                this.Id = id;
                this.Remaining = secondsOffset + (id[0] - 64);
            }

            public override string ToString()
            {
                return $"{this.Id}: [{string.Join(", ", this.Incoming)}], {this.Remaining}";
            }
        }
    }
}
