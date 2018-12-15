using System;

namespace AdventOfCode
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Solver for Day 15
    /// </summary>
    public class Day15
    {
        public int Part1(string[] input)
        {
            (Tile[,] map, Dictionary<char, Player> players) = ParseMap(input);
            int round = 0;

            while (players.Values.Any(p => p.Alive))
            {
                var inPlay = players.Values.Where(p => p.Alive).OrderBy(p => p.Y).ThenBy(p => p.X).ToArray();

                foreach (var player in inPlay)
                {
                    if (!player.Alive)
                    {
                        // died mid-round
                        continue;
                    }

                    // if in range, attack
                    var inRange = player.GetAdjacent(inPlay)
                                        .OrderBy(p => p.HP)
                                        .ThenBy(p => p.Y) // TODO: check this is correct for reading order
                                        .ThenBy(p => p.X)
                                        .FirstOrDefault();

                    if (inRange != null)
                    {
                        // attack
                        inRange.HP -= 3;

                        continue; // turn ends
                    }
                    else
                    {
                        // find path to each alive enemy
                        var paths = players.Values
                                           .Where(p => p.Type != player.Type && p.Alive)
                                           .Select(enemy => player.FindPath(enemy, map))
                                           .Where(p => p != null)
                                           .OrderBy(p => p.Length);

                        // move along the shortest path
                        var path = paths.FirstOrDefault();

                        if (path == null)
                        {
                            // can't move
                            continue;
                        }

                        player.X = path[0].X;
                        player.Y = path[0].Y;
                    }
                }
            }

            return round * players.Values.Where(p => p.Alive).Select(p => p.HP).Sum();
        }

        private static (Tile[,] map, Dictionary<char, Player> players) ParseMap(string[] input)
        {
            char nextId = 'A';
            var map = new Tile[input[0].Length, input.Length];
            var players = new Dictionary<char, Player>();

            // initial parse
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    char tile = input[y][x];

                    if (tile != 'G' && tile != 'E')
                    {
                        map[x, y] = new Tile(tile, x, y);
                        continue;
                    }

                    // parse out the actor
                    var cart = new Player(nextId++, tile, x, y);
                    players[cart.Id] = cart;

                    map[x, y] = new Tile('.', x, y);
                }
            }

            return (map, players);
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

    public static class PositionExtensions
    {
        public static bool IsAdjacent(this IHavePosition @this, IHavePosition other)
        {
            return @this.X == other.X     && @this.Y == other.Y - 1 ||
                   @this.X == other.X     && @this.Y == other.Y + 1 ||
                   @this.X == other.X - 1 && @this.Y == other.Y     ||
                   @this.X == other.X + 1 && @this.Y == other.Y;
        }

        public static ICollection<T> GetAdjacent<T>(this T @this, ICollection<T> others) where T : IHavePosition
        {
            return others.Where(o => o.IsAdjacent(@this)).OrderBy(o => o.Y).ThenBy(o => o.X).ToArray();
        }

        public static int ManhattanDistance(this IHavePosition @this, IHavePosition other)
        {
            return Math.Abs(@this.X - other.X) + Math.Abs(@this.Y - other.Y);
        }

        public static Tile[] FindPath(this IHavePosition start, IHavePosition end, Tile[,] map)
        {
            // TODO
        }
    }

    public class Player : IHavePosition
    {
        public char Id { get; set; }

        public PlayerType Type { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int HP { get; set; } = 300;

        public bool Alive => this.HP > 0;

        public Player(char id, char tile, int x, int y)
        {
            this.Id = id;
            this.Type = tile == 'E' ? PlayerType.E : PlayerType.G;
            this.X = x;
            this.Y = y;
        }
    }

    public class Tile : IHavePosition
    {
        public int X { get; set; }

        public int Y { get; set; }

        public bool IsWall => this.tile == '#';

        private readonly char tile;

        public Tile(char tile, int x, int y)
        {
            this.tile = tile;
            this.X = x;
            this.Y = y;
        }

        public bool IsMoveable(ICollection<Player> players)
        {
            return !this.IsWall && !this.ContainsPlayer(players);
        }

        public bool ContainsPlayer(ICollection<Player> players)
        {
            return players.Any(p => p.X == this.X && p.Y == this.Y);
        }
    }

    public enum PlayerType
    {
        E, G
    }

    public interface IHavePosition
    {
        int X { get; set; }
        int Y { get; set; }
    }
}
