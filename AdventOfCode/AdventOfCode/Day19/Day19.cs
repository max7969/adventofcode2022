using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day19
    {
        public class Rules
        {
            public int OreCost { get; set; }
            public int ClayCost { get; set; }
            public List<int> ObsidianCost { get; set; } = new List<int>();
            public List<int> GeodeCost { get; set; } = new List<int>();
        }

        public class State
        {
            public int Ore { get; set; }
            public int Clay { get; set; }
            public int Obsidian { get; set; }
            public int Geode { get; set; }
            public int OreRobot { get; set; }
            public int ClayRobot { get; set; }
            public int ObsidianRobot { get; set; }
            public int GeodeRobot { get; set; }
            public State Parent { get; set; }
        }

        public long Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            Regex regex = new Regex(@"Blueprint ([0-9]+): Each ore robot costs ([0-9]+) ore\. Each clay robot costs ([0-9]+) ore. Each obsidian robot costs ([0-9]+) ore and ([0-9]+) clay\. Each geode robot costs ([0-9]+) ore and ([0-9]+) obsidian\.");
            List<Rules> rules = new List<Rules>();
            foreach (var line in input)
            {
                Match match = regex.Match(line);
                rules.Add(new Rules
                {
                    OreCost = int.Parse(match.Groups[2].Value),
                    ClayCost = int.Parse(match.Groups[3].Value),
                    ObsidianCost = new List<int> { int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value) },
                    GeodeCost = new List<int> { int.Parse(match.Groups[6].Value), int.Parse(match.Groups[7].Value) }
                });
            }

            List<State> states = new List<State>();
            State initialState = new State { OreRobot = 1 };
            states.Add(initialState);

            for (int i=0;i<24;i++)
            {
                List<State> newStates = new List<State>();
                foreach (var state in states)
                {
                    newStates.AddRange(GetNextStates(state, rules[1]));
                }
                int maxClay = newStates.Select(x => x.ClayRobot).Max();
                int maxObsidian = newStates.Select(x => x.ObsidianRobot).Max();
                int maxGeode = newStates.Select(x => x.GeodeRobot).Max();
                if (maxGeode > 0)
                {
                    newStates = newStates.Where(x => x.GeodeRobot >= maxGeode).ToList();
                }
                if (maxObsidian > 0)
                {
                    newStates = newStates.Where(x => x.ObsidianRobot > 0).ToList();
                    var maxStackObsidian = newStates.Where(x => x.ObsidianRobot == maxObsidian).Select(x => x.Obsidian).Max();
                    var maxStackAlone = newStates.Select(x => x.Obsidian).Max();
                    var maxRobotObsidian = newStates.Where(x => x.Obsidian == maxStackAlone).Select(x => x.ObsidianRobot).Max();
                    if (maxRobotObsidian == maxObsidian)
                    {
                        newStates = newStates.Where(x => x.ObsidianRobot == maxObsidian && x.Obsidian == maxStackAlone).ToList();
                    }
                }
                if (maxClay > 0)
                {
                    newStates = newStates.Where(x => x.ClayRobot > 0).ToList();
                }

                states.Clear();
                states.AddRange(newStates);
            }

            return states.Select(x => x.Geode).Max();
        }

        public static List<State> GetNextStates(State state, Rules rules)
        {
            List<State> states = new List<State>();

            int maxBuyOreRobot = state.Ore / rules.OreCost;
            for (int i = 0; i <= maxBuyOreRobot; i++)
            {
                int oreAmount = state.Ore - i * rules.OreCost;
                int maxBuyClayRobot = oreAmount / rules.ClayCost;
                for (int j= 0; j <= maxBuyClayRobot; j++)
                {
                    int oreAmount2 = state.Ore - i * rules.OreCost - j * rules.ClayCost;
                    int maxBuyObsidianRobot = new List<int> { oreAmount2 / rules.ObsidianCost[0], state.Clay / rules.ObsidianCost[1] }.Min();
                    for (int k=0; k<= maxBuyObsidianRobot; k++)
                    {
                        int oreAmount3 = state.Ore - i * rules.OreCost - j * rules.ClayCost - k * rules.ObsidianCost[0];
                        int maxBuyGeodeRobot = new List<int> { oreAmount3 / rules.GeodeCost[0], state.Obsidian / rules.GeodeCost[1] }.Min();
                        for (int l=0; l<= maxBuyGeodeRobot; l++)
                        {
                            State newState = new State
                            {
                                OreRobot = state.OreRobot + i,
                                ClayRobot = state.ClayRobot + j,
                                ObsidianRobot = state.ObsidianRobot + k,
                                GeodeRobot = state.GeodeRobot + l,
                                Ore = state.Ore - i * rules.OreCost - j * rules.ClayCost - k * rules.ObsidianCost[0] - l * rules.GeodeCost[0] + state.OreRobot,
                                Clay = state.Clay - k * rules.ObsidianCost[1] + state.ClayRobot,
                                Obsidian = state.Obsidian - l * rules.GeodeCost[1] + state.ObsidianRobot,
                                Geode = state.Geode + state.GeodeRobot
                            };
                            states.Add(newState);
                        }
                    }
                }
            }
            return states;
        }

        
        public long Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            return 0;
        }
    }
}
