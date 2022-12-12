using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day12
    {
        public class Node
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Altitude { get; set; }
            public int Heuristique { get; set; }
            public int Cost { get; set; }
            public List<Node> Neighbours { get; set; } = new List<Node>();
            public string Coordinate => $"{X};{Y}";
            public Node Parent { get; set; }
        }

        public long Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            Dictionary<string, Node> nodes = new Dictionary<string, Node>();
            int iTarget = 0;
            int jTarget = 0;
            int iStart = 0;
            int jStart = 0;
            InitNodesDico(input, nodes, ref iTarget, ref jTarget, ref iStart, ref jStart);
            ComputeNeighbours(nodes);

            Node current = ComputeShortestPath(nodes, iTarget, jTarget, iStart, jStart);

            return current.Cost;
        }

        public long Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            Dictionary<string, Node> nodes = new Dictionary<string, Node>();
            int iTarget = 0;
            int jTarget = 0;
            int iStart = 0;
            int jStart = 0;
            InitNodesDico(input, nodes, ref iTarget, ref jTarget, ref iStart, ref jStart);
            ComputeNeighbours(nodes);

            List<long> values = new List<long>();
            foreach(var node in nodes.Values.Where(x => x.Altitude == 1))
            {
                var result = ComputeShortestPath(nodes, iTarget, jTarget, node.X, node.Y);
                if (result.X == iTarget && result.Y == jTarget)
                {
                    values.Add(result.Cost);
                }
            }

            return values.Min();
        }

        private static void ComputeNeighbours(Dictionary<string, Node> nodes)
        {
            foreach (var node in nodes.Values)
            {
                List<string> possibilities = new List<string> { $"{node.X - 1};{node.Y}", $"{node.X + 1};{node.Y}", $"{node.X};{node.Y - 1}", $"{node.X};{node.Y + 1}" };
                foreach (var possibility in possibilities)
                {
                    if (nodes.ContainsKey(possibility)
                    && nodes[possibility].Altitude <= node.Altitude + 1)
                    {
                        node.Neighbours.Add(nodes[possibility]);
                    }
                }
            }
        }

        private static void InitNodesDico(List<string> input, Dictionary<string, Node> nodes, ref int iTarget, ref int jTarget, ref int iStart, ref int jStart)
        {
            for (int i = 0; i < input.Count(); i++)
            {
                for (int j = 0; j < input[i].ToCharArray().Count(); j++)
                {
                    if ((int)input[i].ToCharArray()[j] == 'E')
                    {
                        iTarget = i;
                        jTarget = j;
                    }
                    else if ((int)input[i].ToCharArray()[j] == 'S')
                    {
                        iStart = i;
                        jStart = j;
                    }
                    var node = new Node
                    {
                        X = i,
                        Y = j,
                        Altitude = (int)input[i].ToCharArray()[j] == 'E' ? 26 : (int)input[i].ToCharArray()[j] == 'S' ? 1 : (int)input[i].ToCharArray()[j] - 96
                    };
                    nodes.Add(node.Coordinate, node);
                }
            }
        }

        private static Node ComputeShortestPath(Dictionary<string, Node> nodes, int iTarget, int jTarget, int iStart, int jStart)
        {
            HashSet<string> closedList = new HashSet<string>();
            List<Node> openList = new List<Node>();
            openList.Add(nodes[$"{iStart};{jStart}"]);
            Node current = null;
            while (openList.Count() > 0)
            {
                openList = openList.OrderBy(x => x.Heuristique).ToList();
                current = openList.First();
                openList.Remove(current);
                if (current.Coordinate == $"{iTarget};{jTarget}")
                {
                    break;
                }
                else
                {
                    foreach (var neighbour in current.Neighbours)
                    {
                        var cost = current.Cost + 1;
                        if (!(openList.Where(x => x.Coordinate == neighbour.Coordinate).Select(x => x.Cost).Any(x => x <= cost)
                            || closedList.Contains(neighbour.Coordinate)))
                        {
                            var node = new Node
                            {
                                X = neighbour.X,
                                Y = neighbour.Y,
                                Cost = cost,
                                Heuristique = cost + Math.Abs(neighbour.X - iTarget) + Math.Abs(neighbour.Y - jTarget),
                                Neighbours = nodes[neighbour.Coordinate].Neighbours,
                                Parent = current
                            };
                            openList.Add(node);
                        }
                    }
                    closedList.Add(current.Coordinate);
                }
            }

            return current;
        }
    }
}
