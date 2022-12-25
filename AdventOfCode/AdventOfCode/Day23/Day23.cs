using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day23
    {
        public class Coordinate
        {
            public int X { get; set; }
            public int Y { get; set; }
            public string ToString => $"{X},{Y}";
        }

        public class Elfe
        {
            public Coordinate Coord { get; set; }
            public Coordinate NextPos { get; set; }
            public List<Coordinate> GetAround() =>
                new List<Coordinate>
                {
                    new Coordinate { X = Coord.X - 1, Y = Coord.Y - 1 },
                    new Coordinate { X = Coord.X - 1, Y = Coord.Y },
                    new Coordinate { X = Coord.X - 1, Y = Coord.Y + 1 },
                    new Coordinate { X = Coord.X, Y = Coord.Y - 1 },
                    new Coordinate { X = Coord.X, Y = Coord.Y + 1 },
                    new Coordinate { X = Coord.X + 1, Y = Coord.Y - 1 },
                    new Coordinate { X = Coord.X + 1, Y = Coord.Y },
                    new Coordinate { X = Coord.X + 1, Y = Coord.Y + 1 }
                };
        }

        public class Move
        {
            public List<Coordinate> Coords { get; set; }
            public List<Coordinate> PositionsToCheck(Coordinate coord)
            {
                return Coords.Select(x => new Coordinate { X = x.X + coord.X, Y = x.Y + coord.Y }).ToList();
            }
            public Coordinate MoveTo(Coordinate coord)
            {
                return new Coordinate { X = Coords[1].X + coord.X, Y = Coords[1].Y + coord.Y };
            }
        }


        public long Compute(string filePath, int turn = 10)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            List<Elfe> elfes = new List<Elfe>();
            Queue<Move> moves = new Queue<Move>();
            moves.Enqueue(new Move { Coords = new List<Coordinate> { new Coordinate { X = -1, Y = -1 }, new Coordinate { X = -1, Y = 0 }, new Coordinate { X = -1, Y = 1 } } });
            moves.Enqueue(new Move { Coords = new List<Coordinate> { new Coordinate { X = 1, Y = -1 }, new Coordinate { X = 1, Y = 0 }, new Coordinate { X = 1, Y = 1 } } });
            moves.Enqueue(new Move { Coords = new List<Coordinate> { new Coordinate { X = -1, Y = -1 }, new Coordinate { X = 0, Y = -1 }, new Coordinate { X = 1, Y = -1 } } });
            moves.Enqueue(new Move { Coords = new List<Coordinate> { new Coordinate { X = -1, Y = 1 }, new Coordinate { X = 0, Y = 1 }, new Coordinate { X = 1, Y = 1 } } });

            for (int i=0;i <input.Count;i++)
            {
                for (int j=0;j <input[i].Length;j++)
                {
                    if (input[i].ToCharArray()[j] == '#')
                    {
                        elfes.Add(new Elfe { Coord = new Coordinate { X = i, Y = j } });
                    }
                }
            }

            for (int i=0;i<turn;i++)
            {
                List<Move> movesToTry = moves.ToList();
                moves.Enqueue(moves.Dequeue());
                List<string> elfesPositions = elfes.Select(x => x.Coord.ToString).ToList();

                foreach (var elfe in elfes)
                {
                    elfe.NextPos = elfe.Coord;
                    if (elfe.GetAround().Select(x => x.ToString).Any(x => elfesPositions.Contains(x)))
                    {
                        foreach(var move in movesToTry)
                        {
                            if (move.PositionsToCheck(elfe.Coord).Select(x => x.ToString).All(x => !elfesPositions.Contains(x)))
                            {
                                elfe.NextPos = move.MoveTo(elfe.Coord);
                                break;
                            }
                        }
                    }
                }
                List<string> elfesNextPositions = elfes.Select(x => x.NextPos.ToString).ToList();
                foreach (var elfe in elfes)
                {
                    if (elfesNextPositions.Count(x => x == elfe.NextPos.ToString) > 1)
                    {
                        elfe.NextPos = elfe.Coord;
                    }
                }
                foreach (var elfe in elfes)
                {
                    elfe.Coord = elfe.NextPos;
                }
            }

            var minY = elfes.Min(x => x.Coord.Y);
            var maxY = elfes.Max(x => x.Coord.Y) + 1;
            var minX = elfes.Min(x => x.Coord.X);
            var maxX = elfes.Max(x => x.Coord.X) + 1;

            return (Math.Abs(maxX - minX) * Math.Abs(maxY - minY)) - elfes.Count();
        }


        public long Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            List<Elfe> elfes = new List<Elfe>();
            Queue<Move> moves = new Queue<Move>();
            moves.Enqueue(new Move { Coords = new List<Coordinate> { new Coordinate { X = -1, Y = -1 }, new Coordinate { X = -1, Y = 0 }, new Coordinate { X = -1, Y = 1 } } });
            moves.Enqueue(new Move { Coords = new List<Coordinate> { new Coordinate { X = 1, Y = -1 }, new Coordinate { X = 1, Y = 0 }, new Coordinate { X = 1, Y = 1 } } });
            moves.Enqueue(new Move { Coords = new List<Coordinate> { new Coordinate { X = -1, Y = -1 }, new Coordinate { X = 0, Y = -1 }, new Coordinate { X = 1, Y = -1 } } });
            moves.Enqueue(new Move { Coords = new List<Coordinate> { new Coordinate { X = -1, Y = 1 }, new Coordinate { X = 0, Y = 1 }, new Coordinate { X = 1, Y = 1 } } });

            for (int i = 0; i < input.Count; i++)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    if (input[i].ToCharArray()[j] == '#')
                    {
                        elfes.Add(new Elfe { Coord = new Coordinate { X = i, Y = j } });
                    }
                }
            }

            int turn = 1;
            while (true) 
            {
                List<Move> movesToTry = moves.ToList();
                moves.Enqueue(moves.Dequeue());
                HashSet<string> elfesPositions = elfes.Select(x => x.Coord.ToString).ToHashSet();

                foreach (var elfe in elfes)
                {
                    elfe.NextPos = elfe.Coord;
                    if (elfe.GetAround().Select(x => x.ToString).Any(x => elfesPositions.Contains(x)))
                    {
                        foreach (var move in movesToTry)
                        {
                            if (move.PositionsToCheck(elfe.Coord).Select(x => x.ToString).All(x => !elfesPositions.Contains(x)))
                            {
                                elfe.NextPos = move.MoveTo(elfe.Coord);
                                break;
                            }
                        }
                    }
                }
                List<string> elfesNextPositions = elfes.Select(x => x.NextPos.ToString).ToList();

                if (elfesNextPositions.Count() != elfesNextPositions.ToHashSet().Count())
                {
                    foreach (var elfe in elfes)
                    {
                        if (elfesNextPositions.Count(x => x == elfe.NextPos.ToString) > 1)
                        {
                            elfe.NextPos = elfe.Coord;
                        }
                    }
                }

                if (elfes.All(x => x.Coord.ToString == x.NextPos.ToString))
                {
                    break;
                }
                else
                {
                    turn++;
                    var count = elfes.Count(x => x.Coord.ToString != x.NextPos.ToString);
                    foreach (var elfe in elfes)
                    {
                        elfe.Coord = elfe.NextPos;
                    }
                }
            }

            return turn;
        }
    }
}
