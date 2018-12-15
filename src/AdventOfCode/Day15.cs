using System;

namespace AdventOfCode
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using MoreLinq.Extensions;

    /// <summary>
    /// Solver for Day 15
    /// </summary>
    public class Day15
    {
        public int Part1(string[] input)
        {
            var map = new Map(input);
            var round = 0;

            // stop when only 1 type remains
            while (true)
            {
                if (StartRound(map))
                {
                    // only increment if the entire round completed fully
                    round++;
                }
                else
                {
                    break;
                }
            }

            return round * map.Players.Where(p => p.Alive).Select(p => p.HP).Sum();
        }

        private static bool StartRound(Map map)
        {
            var players = map.Players.OrderBy(p => p.Location.y).ThenBy(p => p.Location.x).ToArray();

            // perform a round
            foreach (var player in players)
            {
                if (!player.Alive)
                {
                    // died previously, possibly even mid-round
                    continue;
                }

                player.TakeTurn(map);
            }

            return players.Where(p => p.Alive).Select(p => p.Type).Distinct().Count() > 1;
        }

        public int Part2(string[] input)
        {
            foreach (string line in input)
            {
                throw new NotImplementedException("Part 2 not implemented");
            }

            return 0;
        }
    }

    public class Map
    {
        public Dictionary<(int x, int y), bool> Tiles { get; }
        public List<Player> Players { get; }

        public Map(string[] input)
        {
            this.Tiles = new Dictionary<(int x, int y), bool>(input.Length * input.Length);
            this.Players = new List<Player>();
            this.ParseInput(input);
        }

        private void ParseInput(string[] input)
        {
            // initial parse
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    char tile = input[y][x];

                    // mark whether tile was habitable
                    this.Tiles[(x, y)] = tile != '#';

                    if (tile == 'G' || tile == 'E')
                    {
                        // parse the player
                        var player = new Player(tile, x, y);
                        this.Players.Add(player);
                    }
                }
            }
        }
    }

    public static class LocationExtensions
    {
        public static IEnumerable<(int x, int y)> AdjacentTiles(this (int x, int y) location)
        {
            (int x, int y) = location;
            yield return (x, y - 1); // up
            yield return (x, y + 1); // down
            yield return (x - 1, y); // left
            yield return (x + 1, y); // right
        }

        public static ICollection<(int x, int y)> ReachableTiles(this (int x, int y) location, Map map)
        {
            return location.AdjacentTiles()
                           .Where(l => map.Tiles.ContainsKey(l) && map.Tiles[l]) // must be valid on the map
                           .Where(l => !map.Players.Any(p => p.Alive && p.Location.x == l.x && p.Location.y == l.y)) // not occupied by alive player
                           .ToArray();
        }
    }

    public class Player
    {
        public PlayerType Type { get; set; }

        public (int x, int y) Location { get; set; }

        public int AP { get; } = 3;

        public int HP { get; private set; } = 200;

        public bool Alive => this.HP > 0;

        public Player(char tile, int x, int y)
        {
            this.Type = tile == 'E' ? PlayerType.E : PlayerType.G;
            this.Location = (x, y);
        }

        public void TakeTurn(Map map)
        {
            if (!this.Alive)
            {
                return;
            }

            // try and move to an enemy, which does nothing if we are already at one or there's no path to one
            this.Move(map);

            // attack!
            this.Attack(map);
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{this.Type}: {this.Location}";
        }

        private void Move(Map map)
        {
            // find locations next to all alive enemies
            var enemies = map.Players.Where(p => p.Type != this.Type && p.Alive).ToArray();
            var enemyAdjacent = enemies.SelectMany(enemy => enemy.Location.ReachableTiles(map)).ToHashSet();

            if (enemyAdjacent.Contains(this.Location) || !enemyAdjacent.Any())
            {
                // we're adjacent to an enemy or there's nowhere to go
                return;
            }

            // move to be next to the nearest enemy using Dijkstra's algorithm
            PathFinder pathFinder = new PathFinder(map, this.Location);
            pathFinder.Calculate();

            var move = enemyAdjacent.Where(e => pathFinder.Moves.ContainsKey(e)) // reachable
                                    .OrderBy(e => e, new DistanceReadingOrderComparer(pathFinder.Distances)).FirstOrDefault();
            this.Location = move;
        }

        private void Attack(Map map)
        {
            // find adjacent enemy. if multiple, pick first in 'reading order' (up, left, right, down)
            IEnumerable<(int x, int y)> adjacent = this.Location.AdjacentTiles();
            var enemies = map.Players.Where(p => p.Type != this.Type && p.Alive).ToArray();

            foreach (var tile in adjacent.OrderBy(a => a.y).ThenBy(a => a.x))
            {
                Player enemy = enemies.FirstOrDefault(p => p.Location == tile);

                if (enemy != null)
                {
                    enemy.HP -= this.AP;
                    return;
                }
            }
        }
    }

    public enum PlayerType
    {
        E, G
    }

    public class PathFinder
    {
        private readonly Map map;
        private readonly (int x, int y) startLocation;

        public Dictionary<(int x, int y), int> Distances { get; }
        public Dictionary<(int x, int y), (int x, int y)> Moves { get; }

        public PathFinder(Map map, (int x, int y) startLocation)
        {
            this.map = map;
            this.startLocation = startLocation;
            this.Distances = new Dictionary<(int x, int y), int>();
            this.Moves = new Dictionary<(int x, int y), (int x, int y)>();
        }

        /// <summary>
        /// Calculates the distance from every available point to the starting point using Dijkstra's algorithm
        /// </summary>
        public void Calculate()
        {
            var visited = new HashSet<(int x, int y)>();
            var stack = new Stack<(int x, int y)>();

            stack.Push(this.startLocation);
            this.Distances[this.startLocation] = 0; // make sure we start here

            // generate a big depth-first list of valid moves
            while (stack.Any())
            {
                var current = stack.Pop();

                if (visited.Add(current)) // returns false if we already visited that tile
                {
                    var validMoves = current.ReachableTiles(this.map);
                    foreach ((int x, int y) move in validMoves)
                    {
                        // fan out from this location
                        stack.Push(move);
                        this.Distances[move] = int.MaxValue;
                    }
                }
            }

            // keep an ordered queue of which is closest
            var comparer = new DistanceReadingOrderComparer(this.Distances);
            var queue = new SortedSet<(int x, int y)>(visited, comparer);

            while (queue.Any())
            {
                // pop off the 'lowest' tile
                var current = queue.Min;
                int removed = queue.RemoveWhere(q => q.x == current.x && q.y == current.y);
                //Debug.Assert(removed == 1);

                // fan out from new tile, if there are any reachable tiles
                var validMoves = current.ReachableTiles(this.map);

                foreach ((int x, int y) adjacent in validMoves)
                {
                    if (this.Distances[current] == int.MaxValue)
                    {
                        // don't know a path back to the start point yet on which to build
                        break;
                    }

                    int currentDistance = this.Distances[adjacent];
                    int newDistance = this.Distances[current] + 1;

                    if (newDistance < currentDistance)
                    {
                        // found a shorter path
                        this.Distances[adjacent] = newDistance;
                        this.Moves[adjacent] = current;

                        // shuffle the current move back in in the right order
                        queue.Remove(adjacent);
                        queue.Add(adjacent);
                    }
                    else if (newDistance == currentDistance)
                    {
                        var currentMove = this.Moves[adjacent];

                        // same distance, decide which wins via 'reading order'
                        if (comparer.Compare(current, currentMove) < 0)
                        {
                            // new location has better 'reading order' than existing shortest one
                            this.Moves[adjacent] = current;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Orders two locations by their distance from a starting location and then by reading order: (up, left, right, down)
    /// </summary>
    public class DistanceReadingOrderComparer : IComparer<(int x, int y)>
    {
        // lookup of location to distance from starting point
        private readonly Dictionary<(int x, int y), int> distances;

        public DistanceReadingOrderComparer(Dictionary<(int x, int y), int> distances)
        {
            this.distances = distances;
        }

        public int Compare((int x, int y) first, (int x, int y) second)
        {
            int compare = this.distances[first].CompareTo(this.distances[second]);

            // shortest distance wins
            if (compare != 0)
            {
                return compare;
            }

            // same distance, order by y then x to get the order (up, left, right, down)
            compare = first.y.CompareTo(second.y);

            if (compare != 0)
            {
                return compare;
            }

            return first.x.CompareTo(second.x);
        }
    }
}
