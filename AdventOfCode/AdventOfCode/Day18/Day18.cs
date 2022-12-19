using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day18
    {
        public List<string> GetSides(int x, int y, int z)
        {
            return new List<string>
            {
                $"{x+1},{y},{z}",
                $"{x-1},{y},{z}",
                $"{x},{y+1},{z}",
                $"{x},{y-1},{z}",
                $"{x},{y},{z+1}",
                $"{x},{y},{z-1}"
            };
        }

        public long Compute(string filePath, long target = 1000000000000)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            HashSet<string> cubes = input.ToHashSet();

            int count = 0;
            foreach (var cube in cubes)
            {
                int sidesExposed = 6;
                var split = cube.Split(",").Select(x => int.Parse(x)).ToList();
                var sides = GetSides(split[0], split[1], split[2]);
                foreach (var side in sides)
                {
                    if (cubes.Contains(side))
                    {
                        sidesExposed--;
                    }
                }
                count += sidesExposed;
            }

            return count;
        }

        public class Node
        {
            public string Key { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
            public List<string> Neighbours { get; set; } = new List<string>();
        }

        public class State
        {
            public int Cost { get; set; } = 0;
            public double Heuristic { get; set; } = 0;
            public string Key { get; set; }
        }

        public long Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            HashSet<string> cubes = input.ToHashSet();
            List<(int x, int y, int z)> cubesCoordinates = cubes.Select(x => x.Split(",")).Select(x => (int.Parse(x[0]), int.Parse(x[1]), int.Parse(x[2]))).ToList();
            Dictionary<string, Node> nodes = CreateNodesMap(cubes, cubesCoordinates);

            string waterToReach = $"{cubesCoordinates.Select(x => x.x).Min() - 1},{cubesCoordinates.Select(x => x.y).Min() - 1},{cubesCoordinates.Select(x => x.z).Min() - 1}";
            Dictionary<string, bool> reachableSides = new Dictionary<string, bool>();
            int count = 0;
            foreach (var cube in cubes)
            {
                int sidesExposed = 6;
                var split = cube.Split(",").Select(x => int.Parse(x)).ToList();
                var sides = GetSides(split[0], split[1], split[2]);
                foreach (var side in sides)
                {
                    if (cubes.Contains(side))
                    {
                        sidesExposed--;
                    }
                    else if (reachableSides.ContainsKey(side))
                    {
                        if (!reachableSides[side])
                        {
                            sidesExposed--;
                        } 
                    }
                    else
                    {
                        var result = IsWaterReachable(side, waterToReach, nodes);

                        foreach (var closed in result.elements)
                        {
                            reachableSides.Add(closed, result.waterReachable);
                        }
                        if (!reachableSides[side])
                        {
                            sidesExposed--;
                        }
                    }
                }
                count += sidesExposed;
            }

            return count;
        }

        private (bool waterReachable, HashSet<string> elements) IsWaterReachable(string side, string waterToReach, Dictionary<string, Node> nodes)
        {
            List<State> openList = new List<State>();
            openList.Add(new State
            {
                Key = side,
                Cost = 0
            });
            HashSet<string> closedList = new HashSet<string>();
            State current = null;
            while (openList.Any())
            {
                openList = openList.OrderByDescending(x => x.Heuristic).ToList();
                current = openList.First();
                openList.Remove(current);

                if (current.Key == waterToReach)
                {
                    return (true, closedList);
                }
                else
                {
                    foreach(var node in nodes[current.Key].Neighbours)
                    {
                        State newState = new State
                        {
                            Key = node,
                            Cost = current.Cost + 1,
                            Heuristic = current.Cost + 1 + Math.Pow(Math.Abs(nodes[node].X - nodes[waterToReach].X) + Math.Abs(nodes[node].Y - nodes[waterToReach].Y) + Math.Abs(nodes[node].Z - nodes[waterToReach].Z), 10)
                        };
                        if (!(openList.Where(x => x.Key == newState.Key).Select(x => x.Cost).Any(x => x <= newState.Cost)
                                || closedList.Contains(newState.Key)))
                        {
                            openList.Add(newState);
                        }
                    }
                    closedList.Add(current.Key);
                }
            }
            return (false, closedList);
        }

        private Dictionary<string, Node> CreateNodesMap(HashSet<string> cubes, List<(int x, int y, int z)> cubesCoordinates)
        {
            Dictionary<string, Node> nodes = new Dictionary<string, Node>();
            var xMin = cubesCoordinates.Select(x => x.x).Min() - 1;
            var xMax = cubesCoordinates.Select(x => x.x).Max() + 1;
            var yMin = cubesCoordinates.Select(x => x.y).Min() - 1;
            var yMax = cubesCoordinates.Select(x => x.y).Max() + 1;
            var zMin = cubesCoordinates.Select(x => x.z).Min() - 1;
            var zMax = cubesCoordinates.Select(x => x.z).Max() + 1;
            for (int i = xMin; i <= xMax; i++)
            {
                for (int j = yMin; j <= yMax; j++)
                {
                    for (int k = zMin; k <= zMax; k++)
                    {
                        var sides = GetSides(i, j, k);
                        var node = new Node
                        {
                            Key = $"{i},{j},{k}",
                            X = i,
                            Y = j,
                            Z = k
                        };
                        foreach (var side in sides)
                        {
                            if (!side.StartsWith((xMin - 1).ToString()) && !side.StartsWith((xMax + 1).ToString())
                                && !side.Contains("," + (yMin - 1) + ",") && !side.Contains("," + (yMax + 1) + ",")
                                && !side.EndsWith((zMin - 1).ToString()) && !side.EndsWith((zMax + 1).ToString())
                                && !cubes.Contains(side))
                            {
                                node.Neighbours.Add(side);
                            }
                        }
                        if (!cubes.Contains(node.Key))
                        {
                            nodes.Add(node.Key, node);
                        }
                    }
                }
            }
            return nodes;
        }
    }
}
