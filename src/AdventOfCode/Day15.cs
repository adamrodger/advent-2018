namespace AdventOfCode
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using MoreLinq;
    using Nito.Collections;

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

                    Debug.WriteLine($"End of round {round}:\n");
                    PrintState(input, map);
                }
                else
                {
                    // TODO: There's an off-by-one error in here which makes the sample input fail
                    Debug.WriteLine($"Finished at {round}:\n");
                    PrintState(input, map);
                    break;
                }
            }

            return round * map.Players.Where(p => p.Alive).Select(p => p.HP).Sum();
        }

        private static void PrintState(string[] input, Map map)
        {
            if (!Debugger.IsAttached)
            {
                return;
            }

            // print end of round
            for (int y = 0; y < input.Length; y++)
            {
                input[y] = input[y].Replace('G', '.').Replace('E', '.');

                var playersOnRow = map.Players.Where(p => p.Location.y == y && p.Alive).OrderBy(p => p.Location.x).ToArray();

                for (int x = 0; x < input[y].Length; x++)
                {
                    var player = playersOnRow.FirstOrDefault(p => p.Location.x == x);

                    Debug.Write(player?.Type.ToString() ?? input[y][x].ToString());
                }

                Debug.Write(" ");
                Debug.WriteLine(string.Join<Player>(", ", playersOnRow));
            }

            Debug.WriteLine("\n");
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
                           .OrderBy(l => l.y)
                           .ThenBy(l => l.x)
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
            return $"{this.Type}: {this.Location}, HP: {this.HP}";
        }

        private void Move(Map map)
        {
            var thisAdjacent = this.Location.AdjacentTiles().ToArray();

            // find locations next to all alive enemies
            var enemies = map.Players.Where(p => p.Type != this.Type && p.Alive).ToArray();
            var enemyLocations = enemies.Select(e => e.Location).ToArray();
            var enemyAdjacent = enemies.SelectMany(enemy => enemy.Location.ReachableTiles(map)).ToHashSet();

            if (thisAdjacent.Any(a => enemyLocations.Contains(a)) || !enemyAdjacent.Any())
            {
                // we're adjacent to an enemy or all enemies are blocked in
                return;
            }

            // move one step closer to nearest enemy
            PathFinder pathFinder = new PathFinder(map);
            this.Location = pathFinder.FindNewLocation(this.Location, enemyAdjacent);
        }

        private void Attack(Map map)
        {
            // find adjacent enemy. if multiple, pick lower HP first, with tie break in 'reading order' (up, left, right, down)
            var adjacent = this.Location.AdjacentTiles().ToHashSet();
            var enemies = map.Players.Where(p => p.Type != this.Type && p.Alive).ToArray();
            var enemy = enemies.Where(e => adjacent.Contains(e.Location))
                               .OrderBy(e => e.HP)
                               .ThenBy(e => e.Location.y)
                               .ThenBy(e => e.Location.x)
                               .FirstOrDefault();

            if (enemy != null)
            {
                enemy.HP -= this.AP;
                return;
            }
        }
    }

    public class PathFinder
    {
        private readonly Map map;

        public PathFinder(Map map)
        {
            this.map = map;
        }

        public (int x, int y) FindNewLocation((int x, int y) startLocation, ICollection<(int x, int y)> endLocations)
        {
            var toVisit = new Deque<((int x, int y) location, int depth)>();
            toVisit.AddToBack((startLocation, 0));

            var occupied = this.map.Players.Where(p => p.Alive).Select(p => p.Location).ToHashSet();
            var visited = new HashSet<(int x, int y)>();

            // map of each tile : parent tile and depth
            var lookup = new Dictionary<(int x, int y), ((int x, int y) parent, int depth)>();

            // bread-first search to find valid moves
            while (toVisit.Any())
            {
                (var current, var distance) = toVisit.RemoveFromFront();

                foreach ((int x, int y) next in current.ReachableTiles(this.map))
                {
                    if (!this.map.Tiles[next] || occupied.Contains(next))
                    {
                        // can't move here - it's a wall or another player

                        // TODO: can't happen because of the ReachableTiles call above
                        continue;
                    }

                    if (!lookup.ContainsKey(next) || lookup[next].depth > distance + 1)
                    {
                        // we've found a shorter path to next (or the only path so far)
                        lookup[next] = (current, distance + 1);
                    }

                    if (visited.Contains(next))
                    {
                        // already checked this location
                        continue;
                    }

                    if (toVisit.All(v => v.location != next))
                    {
                        // not already queued this location for checking, queue at 1 deeper than current
                        toVisit.AddToBack((next, distance + 1));
                    }
                }

                visited.Add(current);
            }

            // lookup now contains every cell that can be reached and the chain to that cell can be derived by following the lookup back to the start point

            List<(int x, int y)> shortestPaths = endLocations.Where(lookup.ContainsKey) // only reachable locations
                                                             .MinBy(e => lookup[e].depth) // closest first
                                                             .OrderBy(p => p.y) // ordered by 'reading order'
                                                             .ThenBy(p => p.x)
                                                             .ToList();

            if (!shortestPaths.Any())
            {
                // can't get to a valid location, stay where we are
                return startLocation;
            }

            var selected = shortestPaths.First();

            // now that we know the the destination, walk backwards to the start
            while (lookup[selected].depth > 1)
            {
                selected = lookup[selected].parent;
            }

            return selected;
        }
    }

    public enum PlayerType
    {
        E, G
    }
}
