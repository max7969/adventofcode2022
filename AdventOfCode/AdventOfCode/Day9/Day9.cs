using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day9   
    {
        public class Coordinate
        {
            public int X { get; set; }
            public int Y { get; set; }
            public string Position => $"{X},{Y}";
            public void Move (Coordinate move)
            {
                X = X + move.X;
                Y = Y + move.Y;
            }
            public int ManhattanDistance(Coordinate coord)
            {
                return Math.Abs(coord.Y - Y) + Math.Abs(coord.X - X); 
            }
        }
       
        public int Compute(string filePath, int size = 2)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            var fullTail = new List<Coordinate>();
            for (int i=0; i<size; i++)
            {
                fullTail.Add(new Coordinate { X = 0, Y = 0 });
            }
            HashSet<string> passedTailPosition = new HashSet<string> { fullTail[size - 1].Position };
            Dictionary<string, Coordinate> directions = new Dictionary<string, Coordinate>
            {
                { "R", new Coordinate { X = 0, Y = 1 } },
                { "L", new Coordinate { X = 0, Y = -1 } },
                { "U", new Coordinate { X = -1, Y = 0 } },
                { "D", new Coordinate { X = 1, Y = 0 } }
            };

            foreach (var line in input)
            {
                var direction = line.Split(" ")[0];
                var moves = int.Parse(line.Split(" ")[1]);
                for (int i = 0; i < moves; i++)
                {
                    fullTail[0].Move(directions[direction]);
                    for (int j = 1; j < size; j++)
                    {
                        if (fullTail[j].ManhattanDistance(fullTail[j -1]) >= 3 && fullTail[j].X != fullTail[j - 1].X && fullTail[j].Y != fullTail[j - 1].Y
                            || fullTail[j].ManhattanDistance(fullTail[j - 1]) == 2 && (fullTail[j].X == fullTail[j - 1].X || fullTail[j].Y == fullTail[j - 1].Y))
                        {
                            fullTail[j].X += fullTail[j - 1].X - fullTail[j].X != 0 ? (fullTail[j - 1].X - fullTail[j].X) / Math.Abs(fullTail[j - 1].X - fullTail[j].X) : 0;
                            fullTail[j].Y += fullTail[j - 1].Y - fullTail[j].Y != 0 ? (fullTail[j - 1].Y - fullTail[j].Y) / Math.Abs(fullTail[j - 1].Y - fullTail[j].Y) : 0;
                        }
                    }
                    passedTailPosition.Add(fullTail[size - 1].Position);
                }

            }

            return passedTailPosition.Count();
        }

      
    }
}
