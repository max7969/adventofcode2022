using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day16
    {
        public class Valve
        {
            public string Key { get; set; }
            public int Value { get; set; }
            public List<(string key, int cost)> Neighbours { get; set; }
        }

        public class ValveState
        {
            public Valve Valve { get; set; }
            public int Cost { get; set; }
            public int Heuristic { get; set; }
            public int SumValue { get; set; }
            public int Turn { get; set; }
            public int Pressure { get; set; }
            public List<string> Opened { get; set; } = new List<string>();
            public string ToString() => $"{Valve.Key}:{string.Join(",",Opened.OrderBy(x => x).ToList())}:{Turn}";
        }

        public long Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            Dictionary<string, Valve> valves = ReadValves(input);
            List<Valve> interestingValves = valves.Values.Where(x => x.Value > 0).ToList();
            List<(string start, string end)> interestingPaths = new List<(string start, string end)>();

            Dictionary<string, Valve> simpleValves = new Dictionary<string, Valve>();
            foreach (var valve in interestingValves.Append(new Valve { Key = "AA" }))
            {
                Valve simpleValve = new Valve
                {
                    Key = valve.Key,
                    Value = valve.Value,
                    Neighbours = new List<(string key, int cost)>()
                };

                foreach (var destinationValve in interestingValves)
                {
                    if (destinationValve.Key != valve.Key)
                    {
                        simpleValve.Neighbours.Add((destinationValve.Key, GetLowestCost(valve.Key, destinationValve.Key, valves)));
                    }
                }
                simpleValves.Add(simpleValve.Key, simpleValve);
            }

            return OptimizeOpening("AA", simpleValves);
        }

        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        public int OptimizeOpening(string start, Dictionary<string, Valve> valves)
        {
            List<ValveState> openList = new List<ValveState>();
            int maxPressure = valves.Sum(x => x.Value.Value);
            openList.Add(
                new ValveState
                {
                    Valve = valves[start]
                });
            HashSet<string> closedList = new HashSet<string>();
            List<ValveState> possibles = new List<ValveState>();
            ValveState current = null;
            while (openList.Any())
            {

                openList = openList.OrderBy(x => x.Heuristic).ToList();
                current = openList.First();

                openList.Remove(current);

                openList = openList.Where(x => x.Turn <= current.Turn || x.Pressure > current.Pressure).ToList();


                if (current.Turn == 30)
                {
                    break;
                }
                else
                {
                    if (true)
                    {
                        var newCost = current.Cost + (maxPressure - current.SumValue) * (30 - current.Turn);
                        var newValveState = new ValveState
                        {
                            Valve = new Valve { Key = "##" },
                            Cost = newCost,
                            Heuristic = newCost,
                            Turn = 30,
                            SumValue = current.SumValue,
                            Opened = current.Opened.ToArray().ToList(),
                            Pressure = current.Pressure + (30 - current.Turn) * current.SumValue
                        };
                        if (!(openList.Where(x => x.ToString() == newValveState.ToString()).Select(x => x.Cost).Any(x => x <= newValveState.Cost)
                            || closedList.Contains(newValveState.ToString())))
                        {
                            openList.Add(newValveState);
                        }
                    }
                    foreach (var valve in current.Valve.Neighbours)
                    {
                        if (!current.Opened.Contains(valve.key))
                        {
                            var newCost = current.Cost + (maxPressure - current.SumValue) * valve.cost;
                            var newValveState = new ValveState
                            {
                                Valve = valves[valve.key],
                                Cost = newCost,
                                Heuristic = newCost + (30 - (current.Turn + valve.cost)),
                                Turn = current.Turn + valve.cost,
                                SumValue = current.SumValue + valves[valve.key].Value,
                                Opened = current.Opened.ToArray().Append(valve.key).ToList(),
                                Pressure = current.Pressure + valve.cost * current.SumValue
                            };
                            if (!(openList.Where(x => x.ToString() == newValveState.ToString()).Select(x => x.Cost).Any(x => x <= newValveState.Cost)
                                || closedList.Contains(newValveState.ToString())))
                            {
                                openList.Add(newValveState);
                            }
                        }
                    }
                    closedList.Add(current.ToString());
                }
            }
            return current.Pressure;
        }

        public int GetLowestCost(string start, string end, Dictionary<string, Valve> valves)
        {
            List<ValveState> openList = new List<ValveState>();
            openList.Add(
                new ValveState
                {
                    Valve = valves[start]
                });
            HashSet<string> closedList = new HashSet<string>();
            ValveState current = null;
            while (openList.Any())
            {
                openList.OrderBy(x => x.Cost);
                current = openList.First();
                openList.Remove(current);

                if (current.Valve.Key == end)
                {
                    return current.Cost + 1;
                }
                else
                {
                    foreach (var valve in valves[current.Valve.Key].Neighbours)
                    {
                        var newValveState = new ValveState
                        {
                            Valve = valves[valve.key],
                            Cost = current.Cost + 1
                        };
                        if (!(openList.Where(x => x.Valve.Key == newValveState.Valve.Key).Select(x => x.Cost).Any(x => x <= newValveState.Cost)
                                   || closedList.Contains(newValveState.Valve.Key)))
                        {
                            openList.Add(newValveState);
                        }
                    }
                }
            }
            return 0;
        }


        private static Dictionary<string, Valve> ReadValves(List<string> input)
        {
            Dictionary<string, Valve> valves = new Dictionary<string, Valve>();
            Regex regex = new Regex(@"Valve ([A-Z]+) has flow rate=([0-9]+); tunnel[s]* lead[s]* to valve[s]* ([A-Z\ \,]+)");

            foreach (var line in input)
            {
                Match match = regex.Match(line);
                Valve valve = new Valve
                {
                    Key = match.Groups[1].Value,
                    Value = int.Parse(match.Groups[2].Value),
                    Neighbours = match.Groups[3].Value.Split(",").Select(x => x.Trim()).Select(x => (x, 1)).ToList()
                };
                valves.Add(valve.Key, valve);
            }

            return valves;
        }
    }
}
