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
        /// <summary>
        /// Play the game until one team loses
        /// </summary>
        /// <param name="input">Input</param>
        /// <param name="elfPower">Attack points of elves (defaults to 3 for part 1)</param>
        /// <returns>The score of the winning team</returns>
        public int Part1(string[] input, int elfPower = 3)
        {
            var map = new Map(input, elfPower);
            var round = 0;

            while (true)
            {
                if (StartRound(map))
                {
                    // only increment if the entire round completed fully
                    round++;

                    PrintState(input, map, "After", round);
                }
                else
                {
                    // TODO: There's an off-by-one error in here which makes the sample input fail because it stops 1 round too soon
                    PrintState(input, map, "Finished on", round);
                    break;
                }
            }

            return round * map.Players.Where(p => p.Alive).Select(p => p.HP).Sum();
        }

        /// <summary>
        /// Ratchet up the power of the elves until all goblins are killed without any elves being killed
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Elves' score at the end of a perfect game</returns>
        public int Part2(string[] input)
        {
            int power = 4;
            int result;

            do
            {
                try
                {
                    result = this.Part1(input, power++);
                }
                catch (InvalidOperationException e)
                {
                    Debug.WriteLine(e.Message);
                    result = -1;
                }
            } while (result == -1); // mmmmmmmm error codes

            return result;
        }

        /// <summary>
        /// Diagnostics for use during debugging
        /// </summary>
        private static void PrintState(string[] input, Map map, string messagePrefix, int round)
        {
            if (!Debugger.IsAttached)
            {
                return;
            }

            Debug.WriteLine($"{messagePrefix} round {round}:");

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

        /// <summary>
        /// Start a new round
        /// </summary>
        /// <param name="map">Map</param>
        /// <returns>Whether the game should continue</returns>
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

                // try and move to an enemy, which does nothing if we are already at one or there's no path to one
                player.Move(map);

                // attack!
                // TODO: Check if anyone is left if we just killed someone
                player.Attack(map);
            }

            return players.Where(p => p.Alive).Select(p => p.Type).Distinct().Count() > 1;
        }
    }

    /// <summary>
    /// Represents a game map
    /// </summary>
    public class Map
    {
        private readonly int elfPower;

        /// <summary>
        /// Lookup of tile location to whether it can be occupied (i.e. not a wall)
        /// </summary>
        public Dictionary<(int x, int y), bool> Tiles { get; }

        /// <summary>
        /// All players in the game
        /// </summary>
        public List<Player> Players { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Map"/> class.
        /// </summary>
        /// <param name="input">Input to parse</param>
        /// <param name="elfPower">How many attack points elves have in this game</param>
        public Map(string[] input, int elfPower)
        {
            this.elfPower = elfPower;
            this.Tiles = new Dictionary<(int x, int y), bool>(input.Length * input.Length);
            this.Players = new List<Player>();
            this.ParseInput(input);
        }

        /// <summary>
        /// Parse the input to a valid map and player collection
        /// </summary>
        /// <param name="input"></param>
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
                        var player = new Player(tile, x, y, tile == 'E' ? this.elfPower : 3);
                        this.Players.Add(player);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Represents either an Elf or a Goblin
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Player type
        /// </summary>
        public PlayerType Type { get; set; }

        /// <summary>
        /// Player location
        /// </summary>
        public (int x, int y) Location { get; set; }

        /// <summary>
        /// Attack points (damage done to enemy)
        /// </summary>
        public int AP { get; }

        /// <summary>
        /// Hit point (health)
        /// </summary>
        public int HP { get; private set; } = 200;

        /// <summary>
        /// Is this player still alive?
        /// </summary>
        public bool Alive => this.HP > 0;

        /// <summary>
        /// Initialises a new instances of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="tile">Tile on which the player begins the game</param>
        /// <param name="x">X co-ordinate</param>
        /// <param name="y">Y co-ordinate</param>
        /// <param name="ap">Attack points</param>
        public Player(char tile, int x, int y, int ap)
        {
            this.Type = tile == 'E' ? PlayerType.E : PlayerType.G;
            this.Location = (x, y);
            this.AP = ap;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{this.Type}: {this.Location}, HP: {this.HP}";
        }

        /// <summary>
        /// Take one step towards the nearest enemy
        /// </summary>
        /// <param name="map">Map</param>
        /// <remarks>
        /// Doesn't move if either we're already next to an enemy or there is no path to an enemy
        /// </remarks>
        public void Move(Map map)
        {
            var thisAdjacent = this.Location.AdjacentTiles().ToHashSet();

            // find locations next to all alive enemies
            var enemies = map.Players.Where(p => p.Type != this.Type && p.Alive).ToArray();
            var enemyLocations = enemies.Select(e => e.Location).ToHashSet();
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

        /// <summary>
        /// Attack an adjacent enemy
        /// </summary>
        /// <param name="map">Map</param>
        /// <returns>Whether an enemy was killed</returns>
        /// <remarks>
        /// If there are no adjacent enemies, nothing happens
        /// If there are multiple, pick lower HP first, with tie break in 'reading order' (up, left, right, down)
        /// </remarks>
        public bool Attack(Map map)
        {
            var adjacent = this.Location.AdjacentTiles().ToHashSet();
            var enemies = map.Players.Where(p => p.Type != this.Type && p.Alive);
            var enemy = enemies.Where(e => adjacent.Contains(e.Location))
                               .OrderBy(e => e.HP)
                               .ThenBy(e => e.Location.y)
                               .ThenBy(e => e.Location.x)
                               .FirstOrDefault();

            if (enemy == null)
            {
                return false;
            }

            enemy.HP -= this.AP;

            // in part 2, stop the first time an Elf dies. Doesn't affect part 1 because Elf AP == 3
            if (!enemy.Alive && enemy.Type == PlayerType.E && enemy.AP > 3)
            {
                throw new InvalidOperationException($"Elf died at power {enemy.AP}");
            }

            // indicate if we killed an enemy because that can end the round
            return !enemy.Alive;
        }
    }

    /// <summary>
    /// Extension methods for working with locations
    /// </summary>
    public static class LocationExtensions
    {
        /// <summary>
        /// Return the adjacent tiles to this location in 'reading order'
        /// </summary>
        /// <param name="location">Location</param>
        /// <returns>Adjacent tiles</returns>
        public static IEnumerable<(int x, int y)> AdjacentTiles(this (int x, int y) location)
        {
            (int x, int y) = location;
            yield return (x, y - 1); // up
            yield return (x - 1, y); // left
            yield return (x + 1, y); // right
            yield return (x, y + 1); // down
        }

        /// <summary>
        /// Return the reachable adjacent tiles from the current location (i.e. those that are still on the map
        /// and don't contain a wall or a player)
        /// </summary>
        /// <param name="location">Location</param>
        /// <param name="map">Map</param>
        /// <returns>Valid adjacent tiles</returns>
        public static ICollection<(int x, int y)> ReachableTiles(this (int x, int y) location, Map map)
        {
            return location.AdjacentTiles()
                           .Where(l => map.Tiles.ContainsKey(l) && map.Tiles[l]) // must be valid on the map
                           .Where(l => !map.Players.Any(p => p.Alive && p.Location.x == l.x && p.Location.y == l.y)) // not occupied by alive player
                           .ToArray();
        }
    }

    /// <summary>
    /// Helper class to help find a path from one location to another
    /// </summary>
    public class PathFinder
    {
        private readonly Map map;

        public PathFinder(Map map)
        {
            this.map = map;
        }

        /// <summary>
        /// From a start location, find the shortest path to any of the given end locations and then return
        /// a new location which is one step towards that end location
        /// </summary>
        /// <param name="startLocation">Start location</param>
        /// <param name="endLocations">Allowed end locations</param>
        /// <returns>One step towards the closest end location, or current location if that's not possible</returns>
        /// <remarks>
        /// If two end locations are the same distance from the start location then the tie is broken in 'reading order'
        /// </remarks>
        public (int x, int y) FindNewLocation((int x, int y) startLocation, ICollection<(int x, int y)> endLocations)
        {
            // create a collection of locations to check along with their distance from the start location so far
            var toVisit = new Deque<((int x, int y) location, int depth)>();
            toVisit.AddToBack((startLocation, 0));

            // keep track of where we've already visited
            var visited = new HashSet<(int x, int y)>();

            // map of each tile : parent tile and depth
            var lookup = new Dictionary<(int x, int y), ((int x, int y) parent, int depth)>();

            // bread-first search to find valid moves
            while (toVisit.Any())
            {
                (var current, var distance) = toVisit.RemoveFromFront();

                // from the current location, try and branch out to all valid adjacent locations (if there are any)
                foreach ((int x, int y) next in current.ReachableTiles(this.map))
                {
                    if (!lookup.ContainsKey(next) || lookup[next].depth > distance + 1)
                    {
                        // we've found a shorter path to next (or the only path so far)
                        lookup[next] = (current, distance + 1);
                    }

                    bool alreadyVisited = visited.Contains(next);
                    bool alreadyQueued = toVisit.Any(v => v.location == next);

                    if (!alreadyVisited && !alreadyQueued)
                    {
                        // queue this location up for checking
                        toVisit.AddToBack((next, distance + 1));
                    }
                }

                // make sure we don't visit this location again
                visited.Add(current);
            }

            // lookup now contains every cell that can be reached and the chain to that cell can be derived by following the lookup back to the start point

            List<(int x, int y)> shortestPaths = endLocations.Where(lookup.ContainsKey) // only reachable locations
                                                             .MinBy(e => lookup[e].depth) // closest first
                                                             .OrderBy(p => p.y) // ordered by 'reading order'
                                                             .ThenBy(p => p.x)
                                                             .ToList();

            var selected = shortestPaths.FirstOrDefault();

            if (selected == default)
            {
                // can't get to a valid location, stay where we are
                return startLocation;
            }

            // now that we know the destination, walk backwards to the start to find which tile to step on to
            while (lookup[selected].depth > 1)
            {
                selected = lookup[selected].parent;
            }

            return selected;
        }
    }

    /// <summary>
    /// Elf = E, Goblin = G
    /// </summary>
    public enum PlayerType
    {
        E, G
    }
}
