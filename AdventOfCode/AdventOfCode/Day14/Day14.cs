using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day14
    {
        public long Compute(string filePath, bool addFloor = false)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            HashSet<string> walls = ComputeWalls(input);
            HashSet<string> sands = new HashSet<string>();

            int currentX = 500;
            int currentY = 0;

            int minY = walls.Select(x => int.Parse(x.Split(",")[1])).Max();
            int minX = walls.Select(x => int.Parse(x.Split(",")[0])).Min();
            int maxX = walls.Select(x => int.Parse(x.Split(",")[0])).Max();

            if (addFloor)
            {
                walls = walls.Concat(ComputeWalls(new List<string> { $"{minX - minY},{minY + 2} -> {maxX + minY},{minY + 2}" })).ToHashSet<string>();
            }

            int voidY = walls.Select(x => int.Parse(x.Split(",")[1])).Max();

            while (currentY < voidY)
            {
                if (!walls.Contains($"{currentX},{currentY+1}") && !sands.Contains($"{currentX},{currentY + 1}"))
                {
                    currentY++;
                }
                else if (!walls.Contains($"{currentX -1},{currentY + 1}") && !sands.Contains($"{currentX-1},{currentY + 1}"))
                {
                    currentX--;
                    currentY++;
                }
                else if (!walls.Contains($"{currentX + 1},{currentY + 1}") && !sands.Contains($"{currentX + 1},{currentY + 1}"))
                {
                    currentX++;
                    currentY++;
                }
                else
                {
                    sands.Add($"{currentX},{currentY}");
                    if (currentX == 500 && currentY == 0)
                    {
                        break;
                    }
                    currentX = 500;
                    currentY = 0;
                }
            }

            return sands.Count();
        }

        private static HashSet<string> ComputeWalls(List<string> input)
        {
            HashSet<string> walls = new HashSet<string>();
            foreach (var line in input)
            {
                var split = line.Split(" -> ");
                for (int i = 0; i < split.Length - 1; i++)
                {
                    var start = split[i].Split(",");
                    var end = split[i + 1].Split(",");
                    var xMin = int.Parse(start[0]) < int.Parse(end[0]) ? int.Parse(start[0]) : int.Parse(end[0]);
                    var xMax = int.Parse(start[0]) > int.Parse(end[0]) ? int.Parse(start[0]) : int.Parse(end[0]);
                    var yMin = int.Parse(start[1]) < int.Parse(end[1]) ? int.Parse(start[1]) : int.Parse(end[1]);
                    var yMax = int.Parse(start[1]) > int.Parse(end[1]) ? int.Parse(start[1]) : int.Parse(end[1]);
                    for (int x = xMin; x <= xMax; x++)
                    {
                        for (int y = yMin; y <= yMax; y++)
                        {
                            walls.Add($"{x},{y}");
                        }
                    }
                }
            }
            return walls;
        }
    }
}
