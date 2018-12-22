using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    using Utilities;

    /// <summary>
    /// Solver for Day 13
    /// </summary>
    public class Day13
    {
        public ((int x, int y) firstCollision, (int x, int y) remaining) Solve(string[] input)
        {
            char nextId = 'A';
            var carts = new Dictionary<char, Cart>();
            char[,] tracks = new char[input[0].Length, input.Length];

            // initial parse
            input.ForEachChar((x, y, tile) =>
            {
                if (tile != '<' && tile != '>' && tile != '^' && tile != 'v')
                {
                    tracks[x, y] = tile;
                    return;
                }

                // parse out the cart
                var cart = new Cart(nextId++, tile, x, y);
                carts[cart.Id] = cart;

                tracks[x, y] = cart.Direction == Direction.Left || cart.Direction == Direction.Right ? '-' : '|';
            });

            Cart firstCollision = null;

            while (carts.Count > 1)
            {
                // simulation
                foreach (Cart cart in carts.Values.OrderBy(c => c.Y).ThenBy(c => c.X))
                {
                    // check if turn required for corner or intersection
                    char tile = tracks[cart.X, cart.Y];
                    cart.CheckCorner(tile);
                    cart.CheckIntersection(tile);

                    // move
                    cart.Move();

                    // check for a collisions
                    var collision = carts.Values.FirstOrDefault(c => c.X == cart.X && c.Y == cart.Y && c != cart);

                    if (collision != default)
                    {
                        firstCollision = firstCollision ?? collision;
                        carts.Remove(cart.Id);

                        if (carts.Count > 1)
                        {
                            // just in case we were left with 2 and then they collided and destroyed each other to leave none
                            carts.Remove(collision.Id);
                        }
                    }
                }
            }

            Cart remaining = carts.First().Value;

            return ((firstCollision.X, firstCollision.Y), (remaining.X, remaining.Y));
        }
    }

    public enum Direction
    {
        Up, Right, Down, Left
    }

    public enum TurnDirection
    {
        Left, Straight, Right
    }

    public class Cart
    {
        public char Id { get; }

        public Direction Direction { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        private TurnDirection nextTurn = TurnDirection.Left;

        public Cart(char id, char direction, int x, int y)
        {
            this.Id = id;
            this.X = x;
            this.Y = y;

            switch (direction)
            {
                case '^':
                    this.Direction = Direction.Up;
                    break;
                case 'v':
                    this.Direction = Direction.Down;
                    break;
                case '<':
                    this.Direction = Direction.Left;
                    break;
                case '>':
                    this.Direction = Direction.Right;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), "Invalid direction");
            }
        }

        public void Move()
        {
            switch (this.Direction)
            {
                case Direction.Up:
                    this.Y--;
                    break;
                case Direction.Right:
                    this.X++;
                    break;
                case Direction.Down:
                    this.Y++;
                    break;
                case Direction.Left:
                    this.X--;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void CheckCorner(char corner)
        {
            if (corner != '/' && corner != '\\')
            {
                return;
            }

            // TODO: there's some fancy thing you can do here where you treat the direction as a number
            // and add/subtract then mod 4 to get the next direction, but this was quick

            switch (corner)
            {
                case '/':
                    switch (this.Direction)
                    {
                        case Direction.Up:
                            this.Direction = Direction.Right;
                            break;
                        case Direction.Right:
                            this.Direction = Direction.Up;
                            break;
                        case Direction.Down:
                            this.Direction = Direction.Left;
                            break;
                        case Direction.Left:
                            this.Direction = Direction.Down;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case '\\':
                    switch (this.Direction)
                    {
                        case Direction.Up:
                            this.Direction = Direction.Left;
                            break;
                        case Direction.Right:
                            this.Direction = Direction.Down;
                            break;
                        case Direction.Down:
                            this.Direction = Direction.Right;
                            break;
                        case Direction.Left:
                            this.Direction = Direction.Up;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
            }
        }

        public void CheckIntersection(char next)
        {
            if (next != '+')
            {
                return;
            }

            // TODO: there's some fancy thing you can do here where you treat the direction as a number
            // and add/subtract then mod 4 to get the next direction, but this was quick

            switch (this.nextTurn)
            {
                case TurnDirection.Left:
                    switch (this.Direction)
                    {
                        case Direction.Up:
                            this.Direction = Direction.Left;
                            break;
                        case Direction.Right:
                            this.Direction = Direction.Up;
                            break;
                        case Direction.Down:
                            this.Direction = Direction.Right;
                            break;
                        case Direction.Left:
                            this.Direction = Direction.Down;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    this.nextTurn = TurnDirection.Straight;
                    break;
                case TurnDirection.Straight:
                    this.nextTurn = TurnDirection.Right;
                    break;
                case TurnDirection.Right:
                    switch (this.Direction)
                    {
                        case Direction.Up:
                            this.Direction = Direction.Right;
                            break;
                        case Direction.Right:
                            this.Direction = Direction.Down;
                            break;
                        case Direction.Down:
                            this.Direction = Direction.Left;
                            break;
                        case Direction.Left:
                            this.Direction = Direction.Up;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    this.nextTurn = TurnDirection.Left;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}