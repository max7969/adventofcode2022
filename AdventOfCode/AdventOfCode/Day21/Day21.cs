using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day21
    {
        public long Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            Dictionary<string, string> monkeys = new Dictionary<string, string>();

            foreach (var line in input)
            {
                monkeys.Add(line.Split(": ")[0], line.Split(": ")[1]);
            }

            Regex onlyOperations = new Regex(@"^[0-9\(\)\+\/\-\*\ ]*$");
            List<(Regex rx, Func<long, long, long> op)> operations = new List<(Regex rx, Func<long, long, long> op)>
            {
                (new Regex(@"^([\-0-9]+) \* ([\-0-9]+)$"), (a, b) => a * b),
                (new Regex(@"^([\-0-9]+) \/ ([\-0-9]+)$"), (a, b) => a / b),
                (new Regex(@"^([\-0-9]+) \- ([\-0-9]+)$"), (a, b) => a - b),
                (new Regex(@"^([\-0-9]+) \+ ([\-0-9]+)$"), (a, b) => a + b)
            };

            Regex monkeyKey = new Regex(@"([a-z]{4})");
            string root = monkeys["root"];
            root = Reduce(monkeys, onlyOperations, operations, monkeyKey, root);

            return long.Parse(root);
        }

        private static string Reduce(Dictionary<string, string> monkeys, Regex onlyOperations, List<(Regex rx, Func<long, long, long> op)> operations, Regex monkeyKey, string root)
        {
            while (!onlyOperations.IsMatch(root))
            {
                var matches = monkeyKey.Matches(root);
                foreach (Match match in matches)
                {
                    root = root.Replace(match.Value, $"({monkeys[match.Value]})");
                }
            }
            root = $"({root})";
            string newRoot = root;
            Regex numericalOperation = new Regex(@"\(([0-9\*\+\-\/\ ]+)\)");
            do
            {
                root = newRoot;
                var matches = numericalOperation.Matches(root);
                foreach (Match match in matches)
                {
                    string value = match.Groups[1].Value;
                    foreach (var op in operations)
                    {
                        Match matchOp = op.rx.Match(value);
                        if (matchOp.Success)
                        {
                            value = op.op(long.Parse(matchOp.Groups[1].Value), long.Parse(matchOp.Groups[2].Value)).ToString();
                            break;
                        }
                    }
                    newRoot = root.Replace(match.Value, value);
                    break;
                }
            } while (root.Contains("(") && newRoot != root);

            return root;
        }

        public long Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            Dictionary<string, string> monkeys = new Dictionary<string, string>();

            foreach (var line in input)
            {
                monkeys.Add(line.Split(": ")[0], line.Split(": ")[1]);
            }

            Regex onlyOperations = new Regex(@"^[0-9\(\)\+\/\-\*\ x]*$");
            List<(Regex rx, Func<long, long, long> op)> operations = new List<(Regex rx, Func<long, long, long> op)>
            {
                (new Regex(@"^([\-0-9]+) \* ([\-0-9]+)$"), (a, b) => a * b),
                (new Regex(@"^([\-0-9]+) \/ ([\-0-9]+)$"), (a, b) => a / b),
                (new Regex(@"^([\-0-9]+) \- ([\-0-9]+)$"), (a, b) => a - b),
                (new Regex(@"^([\-0-9]+) \+ ([\-0-9]+)$"), (a, b) => a + b)
            };

            List<(Regex rx, Func<long, long, long> op)> reduceOperations = new List<(Regex rx, Func<long, long, long> op)>
            {
                (new Regex(@"^([0-9]+) \* ([0-9\(\)\+\/\-\*\ x]+)$"), (a, b) => a / b),
                (new Regex(@"^([0-9\(\)\+\/\-\*\ x]+) \* ([0-9]+)$"), (a, b) => a / b),
                (new Regex(@"^([0-9]+) \/ ([0-9\(\)\+\/\-\*\ x]+)$"), (a, b) => b / a),
                (new Regex(@"^([0-9\(\)\+\/\-\*\ x]+) \/ ([0-9]+)$"), (a, b) => a * b),
                (new Regex(@"^([0-9]+) \- ([0-9\(\)\+\/\-\*\ x]+)$"), (a, b) => -(a - b)),
                (new Regex(@"^([0-9\(\)\+\/\-\*\ x]+) \- ([0-9]+)$"), (a, b) => a + b),
                (new Regex(@"^([0-9]+) \+ ([0-9\(\)\+\/\-\*\ x]+)$"), (a, b) => a - b),
                (new Regex(@"^([0-9\(\)\+\/\-\*\ x]+) \+ ([0-9]+)$"), (a, b) => a - b)
            };

            Regex monkeyKey = new Regex(@"([a-z]{4})");
            string rootPart1 = monkeys["root"].Split(" + ")[0];
            string rootPart2 = monkeys["root"].Split(" + ")[1];
            monkeys["humn"] = "x";

            rootPart1 = Reduce(monkeys, onlyOperations, operations, monkeyKey, rootPart1);
            rootPart2 = Reduce(monkeys, onlyOperations, operations, monkeyKey, rootPart2);

            long solution = new Regex(@"^[0-9]*$").IsMatch(rootPart1) ? long.Parse(rootPart1) : long.Parse(rootPart2);
            string toReduce = new Regex(@"^[0-9]*$").IsMatch(rootPart1) ? rootPart2 : rootPart1;

            while (toReduce != "x")
            {
                if (toReduce.StartsWith("(") && toReduce.EndsWith(")"))
                {
                    toReduce = toReduce.Substring(1, toReduce.Length - 2);
                } else
                {
                    foreach (var reduceOperation in reduceOperations)
                    {
                        Match match = reduceOperation.rx.Match(toReduce);
                        if (match.Success)
                        {
                            if (new Regex(@"^[0-9]*$").IsMatch(match.Groups[1].Value))
                            {
                                solution = reduceOperation.op(solution, long.Parse(match.Groups[1].Value));
                                toReduce = match.Groups[2].Value;
                            }
                            else
                            {
                                solution = reduceOperation.op(solution, long.Parse(match.Groups[2].Value));
                                toReduce = match.Groups[1].Value;
                            }
                            break;
                        }
                    }
                }
            }

            return (long)solution;
        }
    }
}
