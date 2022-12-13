using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day13
    {
        public class Element
        {
            public string Key { get; set; }
            public List<Element> SubElements { get; set; } = new List<Element>();
            public int? Value { get; set; } = null;
            public (bool ordered, bool needNext) IsOrderedCompareTo(Element other)
            {
                if (Value != null && other.Value != null)
                {
                    return (Value < other.Value, Value == other.Value);
                } 
                else if (Value == null && other.Value == null)
                {
                    for (int i = 0; i < SubElements.Count; i++)
                    {
                        if (i > other.SubElements.Count - 1)
                        {
                            return (false, false);
                        }
                        var result = SubElements[i].IsOrderedCompareTo(other.SubElements[i]);
                        if (!result.needNext)
                        {
                            return (result.ordered, false);
                        }
                    }
                    if (SubElements.Count == other.SubElements.Count)
                    {
                        return (false, true);
                    }

                    return (true, false);
                }
                else
                {
                    if (Value == null)
                    {
                        return this.IsOrderedCompareTo(new Element { SubElements = new List<Element> { new Element { Value = other.Value } } });
                    } else
                    {
                        return new Element { SubElements = new List<Element> { new Element { Value = this.Value } } }.IsOrderedCompareTo(other);
                    }
                }
            }

            public string StringValue()
            {
                if (Value != null)
                {
                    return Value.ToString();
                } else
                {
                    return "[" + string.Join(",", SubElements.Select(x => x.StringValue())) + "]";
                }
            }
        }

        public class Pair
        {
            public Element Left { get; set; }
            public Element Right { get; set; }
        }

        public long Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            List<Pair> pairs = ExtractPairs(input);


            int sum = 0;
            for (int i = 0; i < pairs.Count; i++)
            {
                if (pairs[i].Left.IsOrderedCompareTo(pairs[i].Right).ordered)
                {
                    sum += i + 1;
                }
            }
            return sum;
        }

        public long Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            List<Pair> pairs = ExtractPairs(input);

            List<Element> elements = new List<Element>();

            elements.AddRange(pairs.Select(x => x.Left).ToList());
            elements.AddRange(pairs.Select(x => x.Right).ToList());
            elements.Add(ParseData("[[2]]"));
            elements.Add(ParseData("[[6]]"));

            elements.Sort((a, b) => a.IsOrderedCompareTo(b).ordered ? -1 : 1);

            var asString = elements.Select(x => x.StringValue()).ToList();
            return (asString.IndexOf("[[2]]") + 1) *(asString.IndexOf("[[6]]") +1);
        }

        private List<Pair> ExtractPairs(List<string> input)
        {
            List<Pair> pairs = new List<Pair>();
            Pair current = new Pair();
            foreach (var line in input)
            {
                if (line == String.Empty)
                {
                    pairs.Add(current);
                    current = new Pair();
                }
                else if (current.Left == null)
                {
                    current.Left = ParseData(line);
                }
                else
                {
                    current.Right = ParseData(line);
                }
            }
            pairs.Add(current);
            return pairs;
        }

        private Element ParseData(string line)
        {
            Dictionary<string, Element> Elements = new Dictionary<string, Element>();
            Regex regex = new Regex(@"\[([0-9,#]*)\]");
            int countElement = 0;
            while (line.Contains("["))
            {

                var matches = regex.Matches(line);
                foreach(Match match in matches)
                {
                    Element global = new Element();
                    if (match.Groups[1].Value != string.Empty)
                    {
                        var splits = match.Groups[1].Value.Split(",");
                        foreach (var split in splits)
                        {
                            if (split.StartsWith("#"))
                            {
                                global.SubElements.Add(Elements[split]);
                            }
                            else
                            {
                                Element nodeElement = new Element { Key = $"#{countElement++}", Value = int.Parse(split) };
                                Elements.Add(nodeElement.Key, nodeElement);
                                global.SubElements.Add(Elements[nodeElement.Key]);
                            }
                        }
                    }
                    global.Key = $"#{countElement++}";
                    Elements.Add(global.Key, global);
                    line = line.Replace(match.Groups[0].Value, global.Key);
                }
            }

            return Elements[line];
        }
    }
}
