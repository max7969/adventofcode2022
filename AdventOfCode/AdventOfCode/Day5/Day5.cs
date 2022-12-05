using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day5
    {
        Dictionary<int, Stack<string>> stacks = new Dictionary<int, Stack<string>>();

        public string Compute(string filePath, bool superCrane = false)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            foreach (var line in input)
            {
                if (line == string.Empty)
                {
                    break;
                }
                var split = Split(line + " ", 4);

                for (int i = 0; i < split.Count(); i++)
                {
                    if (!stacks.Keys.Contains(i + 1))
                    {
                        stacks.Add(i + 1, new Stack<string>());
                    }
                    if (split[i] != string.Empty && split[i].Contains("["))
                    {
                        stacks[i + 1].Push(new Regex(@"\[([A-Z])\] ").Match(split[i]).Groups[1].Value);
                    }
                }
            }
            stacks = stacks.ToDictionary(x => x.Key, x => new Stack<string>(x.Value.ToList()));

            Regex rx = new Regex("move ([0-9]+) from ([0-9]+) to ([0-9]+)");

            foreach (var line in input)
            {
                if (line.StartsWith("move"))
                {
                    var match = rx.Match(line);
                    int moves = int.Parse(match.Groups[1].Value);
                    int from = int.Parse(match.Groups[2].Value);
                    int to = int.Parse(match.Groups[3].Value);

                    if (superCrane)
                    {
                        var list = new List<string>();
                        for (int i = 0; i < moves; i++)
                        {
                            var pop = stacks[from].Pop();
                            list.Add(pop);
                        }
                        list.Reverse();
                        foreach (var element in list)
                        {
                            stacks[to].Push(element);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < moves; i++)
                        {
                            var pop = stacks[from].Pop();
                            stacks[to].Push(pop);
                        }
                    }
                }
            }

            return String.Join("", stacks.Select(x => x.Value.Pop()).ToList());
        }

        static List<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize)).ToList();
        }
    }
}
