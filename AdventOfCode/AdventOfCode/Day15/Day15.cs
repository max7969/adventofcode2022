using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day15
    {
        public class Coordinate
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        public class Duo
        {
            public Coordinate Sensor { get; set; }
            public Coordinate Beacon { get; set; }
            public int DistanceBetween() =>
                ManhattanDistance(Sensor, Beacon);
            public List<Coordinate> Corners()
            {
                return new List<Coordinate> {
                    new Coordinate { X = Sensor.X, Y = Sensor.Y - DistanceBetween() },
                    new Coordinate { X = Sensor.X - DistanceBetween(), Y = Sensor.Y },
                    new Coordinate { X = Sensor.X + DistanceBetween(), Y = Sensor.Y },
                    new Coordinate { X = Sensor.X, Y = Sensor.Y + DistanceBetween() }
                };
            }
            public (int xMin, int xMax) SpecificLine(int y)
            {
                var xMin = Sensor.X - DistanceBetween() + ManhattanDistance(Sensor, new Coordinate { X = Sensor.X, Y = y });
                var xMax = Sensor.X + DistanceBetween() - ManhattanDistance(Sensor, new Coordinate { X = Sensor.X, Y = y });
                return (xMin, xMax);
            }
        }

        public static int ManhattanDistance(Coordinate a, Coordinate b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        public long Compute(string filePath, int yLine)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            List<Duo> duos = ExtractDuos(input);
            List<(int xMin, int xMax)> lines = new List<(int xMin, int xMax)>();

            foreach (var duo in duos)
            {
                if (yLine >= duo.Corners()[0].Y && yLine <= duo.Corners()[3].Y)
                {
                    lines.Add(duo.SpecificLine(yLine));
                }
            }

            MergeLines(lines);

            var sum = 0;
            foreach (var line in lines)
            {
                var sensorOnLine = duos.Select(x => x.Sensor).Where(x => x.Y == yLine && x.X >= line.xMin && x.X <= line.xMax).Count();
                var beaconOnLine = duos.Select(x => x.Beacon).Where(x => x.Y == yLine && x.X >= line.xMin && x.X <= line.xMax).Select(x => $"{x.X},{x.Y}").ToHashSet().Count();
                sum += (Math.Abs(line.xMin - line.xMax) + 1) - sensorOnLine - beaconOnLine;
            }
            return sum;
        }

        public long Compute2(string filePath, int size)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            List<Duo> duos = ExtractDuos(input);
            long y = 0;
            long x = 0;
            for (int i = 0; i < size; i++)
            {
                List<(int xMin, int xMax)> lines = new List<(int xMin, int xMax)>();
                foreach (var duo in duos)
                {
                    if (i >= duo.Corners()[0].Y && i <= duo.Corners()[3].Y)
                    {
                        lines.Add(duo.SpecificLine(i));
                    }
                }
                MergeLines(lines);
                if (lines.Count() == 2)
                {
                    y = i;
                    x = lines.Select(x => x.xMin).Max() - 1;
                    break;
                }
            }
            return x * 4000000 + y;
        }

        private static void MergeLines(List<(int xMin, int xMax)> lines)
        {
            bool continueCycle = false;
            do
            {
                continueCycle = false;
                for (int i = 0; i < lines.Count; i++)
                {
                    for (int j = 0; j < lines.Count; j++)
                    {
                        if (i != j)
                        {
                            var res = Join(lines[i], lines[j]);
                            if (res.Count == 1)
                            {
                                lines.RemoveAt(i > j ? i : j);
                                lines.RemoveAt(i > j ? j : i);
                                lines.Add(res[0]);
                                continueCycle = true;
                                break;
                            }
                        }
                    }
                    if (continueCycle)
                    {
                        break;
                    }
                }

            } while (continueCycle);
        }
        
        public static List<(int xMin, int xMax)> Join((int xMin, int xMax) a, (int xMin, int xMax) b)
        {
            List<(int xMin, int xMax)> lines = new List<(int xMin, int xMax)>();
            if (a.xMax < b.xMin || b.xMax < a.xMin)
            {
                lines.Add(a);
                lines.Add(b);
            }
            else if (a.xMin >= b.xMin && a.xMax <= b.xMax)
            {
                lines.Add(b);
            }
            else if (b.xMin >= a.xMin && b.xMax <= a.xMax)
            {
                lines.Add(a);
            }
            else if (a.xMin <= b.xMin && a.xMax >= b.xMin)
            {
                lines.Add((a.xMin, b.xMax));
            }
            else if (b.xMin <= a.xMin && b.xMax >= a.xMin)
            {
                lines.Add((b.xMin, a.xMax));
            }
            return lines;
        }

        public static List<Duo> ExtractDuos(List<string> input)
        {
            List<Duo> duos = new List<Duo>();
            Regex regex = new Regex(@"Sensor at x=([0-9\-]*), y=([0-9\-]*): closest beacon is at x=([0-9\-]*), y=([0-9\-]*)");
            foreach (var line in input)
            {
                Match match = regex.Match(line);
                duos.Add(new Duo
                {
                    Sensor = new Coordinate
                    {
                        X = int.Parse(match.Groups[1].Value),
                        Y = int.Parse(match.Groups[2].Value)
                    },
                    Beacon = new Coordinate
                    {
                        X = int.Parse(match.Groups[3].Value),
                        Y = int.Parse(match.Groups[4].Value)
                    }
                });
            }
            return duos;
        }
    }
}
