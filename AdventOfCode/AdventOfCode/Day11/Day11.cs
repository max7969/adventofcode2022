using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day11
    {
        public static readonly Dictionary<string, Func<long, long, long>> Operations = new Dictionary<string, Func<long, long, long>>
        {
            { "*", (long a, long b) => a * b },
            { "+", (long a, long b) => a + b }
        };

        public class Monkey
        {
            public List<long> Items { get; set; } = new List<long>();
            public string Operation { get; set; }
            public int Test { get; set; }
            public int TrueMonkey { get; set; }
            public int FalseMonkey { get; set; }
            public long CountInspection { get; set; }
            public bool IsComplex { get; set; }
            public (int throwTo, long itemValue) Inspect(long item, long modulus = 1)
            {
                var realOperation = Operation.Replace("old", item.ToString()).Trim();

                long result = 0;
                if (realOperation.Contains("*"))
                {
                    result = Operations["*"](long.Parse(realOperation.Split("*")[0]), long.Parse(realOperation.Split("*")[1]));
                } else
                {
                    result = Operations["+"](long.Parse(realOperation.Split("+")[0]), long.Parse(realOperation.Split("+")[1]));
                }
                result = IsComplex ? result % modulus : result / 3L; 
                CountInspection++;
                if (result % Test == 0)
                {
                    return (TrueMonkey, result);
                }
                return (FalseMonkey, result);
            }

            public string GetState() => Items.Count().ToString();
            public string GetCompleteState() => string.Join(",", Items);
        }

        public long Compute(string filePath, int rounds = 20, bool isComplex = false)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            List<Monkey> monkeys = ReadMonkeys(input, isComplex);
            List<string> states = new List<string>();
            List<string> completeStates = new List<string>();

            long modulus = monkeys.Select(x => x.Test).Aggregate((a, b) => a * b);

            for (int i = 0; i < rounds; i++)
            {
                foreach (var monkey in monkeys)
                {
                    foreach (var item in monkey.Items)
                    {
                        var result = monkey.Inspect(item, modulus);
                        monkeys[result.throwTo].Items.Add(result.itemValue);
                    }
                    monkey.Items.Clear();
                }
            }
            monkeys = monkeys.OrderByDescending(x => x.CountInspection).ToList();
            return monkeys[0].CountInspection * monkeys[1].CountInspection;
        }

        private static List<Monkey> ReadMonkeys(List<string> input, bool isComplex)
        {
            List<Monkey> monkeys = new List<Monkey>();

            Regex rxOperation = new Regex(@"Operation: new = ([0-9a-z\*\+\-\/\ ]+)");
            Regex rxTest = new Regex(@"Test: divisible by ([0-9]+)");
            Regex rxTrue = new Regex(@" If true: throw to monkey ([0-9]+)");
            Regex rxFalse = new Regex(@" If false: throw to monkey ([0-9]+)");

            foreach (var line in input)
            {
                if (line.StartsWith("Monkey"))
                {
                    monkeys.Add(new Monkey() { IsComplex = isComplex });
                }
                else if (line.Contains("Starting items: "))
                {
                    monkeys.Last().Items = line.Split("Starting items: ")[1].Split(",").Select(x => long.Parse(x)).ToList();
                }
                else if (rxOperation.IsMatch(line))
                {
                    monkeys.Last().Operation = rxOperation.Match(line).Groups[1].Value;
                }
                else if (rxTest.IsMatch(line))
                {
                    monkeys.Last().Test = int.Parse(rxTest.Match(line).Groups[1].Value);
                }
                else if (rxTrue.IsMatch(line))
                {
                    monkeys.Last().TrueMonkey = int.Parse(rxTrue.Match(line).Groups[1].Value);
                }
                else if (rxFalse.IsMatch(line))
                {
                    monkeys.Last().FalseMonkey = int.Parse(rxFalse.Match(line).Groups[1].Value);
                }
            }

            return monkeys;
        }
    }
}
