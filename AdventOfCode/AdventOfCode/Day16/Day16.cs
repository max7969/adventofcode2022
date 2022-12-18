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
        public class State
        {
            public string Key { get; set; }
            public List<string> SecondaryKey { get; set; } = new List<string>();
            public int Value { get; set; }
            public int Cost { get; set; }
            public int Turn { get; set; } = 1;
            public int TotalValue { get; set; }
            public List<string> NextState { get; set; }
            public double Heuristic { get; set; }
            public List<string> OpenValved { get; set; } = new List<string>();
            public string OpenNextTurn { get; set; } = string.Empty;
            public string ToString() =>
                Key + ";" + string.Join(",", OpenValved.OrderBy(x => x).ToList());
        }

        public class DoubleState
        {
            public State Human { get; set; }
            public State Elephant { get; set; }
            public int Cost { get; set; }
            public double Heuristic { get; set; }
            public int Value { get; set; }
            public int TotalValue { get; set; }
            public int Turn { get; set; } = 1;
            public List<string> Steps { get; set; } = new List<string>();
            public HashSet<string> JoinedOpenValves()
            {
                HashSet<string> valves = new HashSet<string>();
                Human.OpenValved.ForEach(x => valves.Add(x));
                Elephant.OpenValved.ForEach(x => valves.Add(x));
                return valves;
            }
            public string ToString() =>
                Human.Key + "," + Elephant.Key + ";" + String.Join(",", JoinedOpenValves().OrderBy(x => x).ToList());
        }

        public long Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            Dictionary<string, State> valves = ReadValves(input);
            List<string> interestingValves = valves.Values.Where(x => x.Value > 0).Select(x => x.Key).ToList();
            int maxPressure = valves.Values.Where(x => x.Value > 0).Select(x => x.Value).Sum();
            List<State> openList = new List<State>();
            openList.Add(valves["AA"]);
            HashSet<string> closedList = new HashSet<string>();
            State current = null;
            openList[0].Cost = maxPressure;

            while (openList.Any())
            {
                openList = openList.OrderBy(x => x.Heuristic).ToList();
                current = openList.First();
                openList.Remove(current);

                if (interestingValves.All(x => current.OpenValved.Contains(x)) || current.Turn == 30)
                {
                    break;
                }
                else
                {
                    foreach (var neighbour in current.NextState)
                    {
                        bool isValvedAlreadyOpen = current.OpenValved.Contains(neighbour);
                        bool isValvedInteresting = interestingValves.Contains(neighbour);
                        if (current.Turn + 1 <= 30)
                        {
                            var newCost = current.Cost + (maxPressure - current.Value);
                            var state = new State
                            {
                                Key = neighbour,
                                Cost = newCost,
                                Turn = current.Turn + 1,
                                Value = current.Value,
                                TotalValue = current.TotalValue + current.Value,
                                Heuristic = newCost + (maxPressure - current.Value),
                                NextState = valves[neighbour].NextState,
                                OpenValved = current.OpenValved.ToArray().ToList(),
                            };
                            if (!(openList.Where(x => x.ToString() == state.ToString()).Select(x => x.Cost).Any(x => x <= state.Cost)
                                || closedList.Contains(state.ToString())))
                            {
                                openList.Add(state);
                            }
                        }
                        if (!isValvedAlreadyOpen && isValvedInteresting && (current.Turn + 2) <= 30)
                        {
                            var otherNewCost = current.Cost + (maxPressure * 2 - current.Value * 2 - valves[neighbour].Value);
                            var otherState = new State
                            {
                                Key = neighbour,
                                Cost = otherNewCost,
                                Turn = current.Turn + 2,
                                Value = current.Value + valves[neighbour].Value,
                                TotalValue = current.TotalValue + current.Value * 2 + valves[neighbour].Value,
                                Heuristic = otherNewCost + (maxPressure - current.Value - valves[neighbour].Value),
                                NextState = valves[neighbour].NextState,
                                OpenValved = current.OpenValved.ToArray().Append(neighbour).ToList(),
                            };
                            if (!(openList.Where(x => x.ToString() == otherState.ToString()).Select(x => x.Cost).Any(x => x <= otherState.Cost)
                            || closedList.Contains(otherState.ToString())))
                            {
                                openList.Add(otherState);
                            }
                        }

                    }
                    closedList.Add(current.ToString());
                }
            }

            return (30 - current.Turn) * current.Value + current.TotalValue;
        }

        private static Dictionary<string, State> ReadValves(List<string> input)
        {
            Dictionary<string, State> valves = new Dictionary<string, State>();
            Regex regex = new Regex(@"Valve ([A-Z]+) has flow rate=([0-9]+); tunnel[s]* lead[s]* to valve[s]* ([A-Z\ \,]+)");

            foreach (var line in input)
            {
                Match match = regex.Match(line);
                State state = new State
                {
                    Key = match.Groups[1].Value,
                    SecondaryKey = new List<string> { match.Groups[1].Value, match.Groups[1].Value },
                    Value = int.Parse(match.Groups[2].Value),
                    NextState = match.Groups[3].Value.Split(",").Select(x => x.Trim()).ToList()
                };
                valves.Add(state.Key, state);
            }

            return valves;
        }

        public List<string> GetAllPossibleNextSteps(List<string> secondaryKeys, Dictionary<string, State> valves)
        {
            List<string> result = new List<string>();
            return result;
        }

        public long Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            Dictionary<string, State> valves = ReadValves(input);
            List<string> interestingValves = valves.Values.Where(x => x.Value > 0).Select(x => x.Key).ToList();
            int maxPressure = valves.Values.Where(x => x.Value > 0).Select(x => x.Value).Sum();
            List<DoubleState> openList = new List<DoubleState>();
            openList.Add(new DoubleState { Human = valves["AA"], Elephant = valves["AA"] });
            HashSet<string> closedList = new HashSet<string>();
            DoubleState current = null;
            openList[0].Cost = 0;

            while (openList.Any())
            {
                openList = openList.OrderBy(x => x.Heuristic).ToList();
                current = openList.First();
                openList.Remove(current);

                if (interestingValves.All(x => current.JoinedOpenValves().Contains(x)) || current.Turn == 26)
                {
                    break;
                }
                else
                {
                    if (current.Turn + 1 <= 26)
                    {
                        var nextHumanSteps = current.Human.NextState.ToList();
                        if (interestingValves.Contains(current.Human.Key) 
                            && !current.JoinedOpenValves().Contains(current.Human.Key) 
                            && current.Human.OpenNextTurn == string.Empty)
                        {
                            nextHumanSteps.Add(current.Human.Key);
                        }
                        foreach (var nextHumanStep in nextHumanSteps)
                        {
                            var nextElephantSteps = current.Elephant.NextState.ToList();
                            if (interestingValves.Contains(current.Elephant.Key) 
                                && !current.JoinedOpenValves().Contains(current.Elephant.Key) 
                                && current.Elephant.Key != nextHumanStep
                                && current.Elephant.OpenNextTurn == string.Empty)
                            {
                                nextElephantSteps.Add(current.Elephant.Key);
                            }
                            foreach (var nextElephantStep in nextElephantSteps)
                            {
                                var openedThisTurn = current.Human.Key == nextHumanStep ? valves[nextHumanStep].Value : 0;
                                openedThisTurn += current.Elephant.Key == nextElephantStep ?  valves[nextElephantStep].Value : 0;
                                var newCost = current.Cost + (maxPressure - current.Value - (openedThisTurn / 2));
                                var doubleState = new DoubleState
                                {
                                    Human = new State
                                    {
                                        Key = nextHumanStep,
                                        NextState = valves[nextHumanStep].NextState,
                                        OpenValved = current.Human.OpenNextTurn != string.Empty ? current.Human.OpenValved.ToArray().Append(current.Human.OpenNextTurn).ToList() : current.Human.OpenValved.ToArray().ToList(),
                                        OpenNextTurn = current.Human.Key == nextHumanStep ? nextHumanStep : string.Empty
                                    },
                                    Elephant = new State
                                    {
                                        Key = nextElephantStep,
                                        NextState = valves[nextElephantStep].NextState,
                                        OpenValved = current.Elephant.OpenNextTurn != string.Empty ? current.Elephant.OpenValved.ToArray().Append(current.Elephant.OpenNextTurn).ToList() : current.Elephant.OpenValved.ToArray().ToList(),
                                        OpenNextTurn = current.Elephant.Key == nextElephantStep ? nextElephantStep : string.Empty
                                    },
                                    Cost = newCost,
                                    Heuristic = newCost + (maxPressure - current.Value),
                                    Value = current.Value + openedThisTurn,
                                    TotalValue = current.TotalValue + current.Value,
                                    Turn = current.Turn + 1,
                                    Steps = current.Steps.ToArray().ToList()
                                };

                                if (current.Human.OpenNextTurn != string.Empty)
                                {

                                }
                                doubleState.Steps.Add(doubleState.ToString() + "+" + doubleState.Value + "+" + doubleState.TotalValue);
                                if (!(openList.Where(x => x.ToString() == doubleState.ToString()).Select(x => x.Cost).Any(x => x <= doubleState.Cost)
                                    || closedList.Contains(doubleState.ToString())))
                                {
                                    openList.Add(doubleState);
                                }
                            }
                        }

                    }
                    
                    closedList.Add(current.ToString());
                }
            }

            return (26 - current.Turn) * current.Value + current.TotalValue;
        }
    }
}
