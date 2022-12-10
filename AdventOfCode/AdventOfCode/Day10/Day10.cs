using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day10
    {
        public int Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            int value = 1;
            int sum = 0;
            int cycle = 0;
            foreach (var line in input)
            {
                if (line == "noop")
                {
                    cycle ++;
                    sum += GetValueToAdd(cycle, value);
                } else
                {
                    cycle++;
                    sum += GetValueToAdd(cycle, value);
                    cycle++;
                    sum += GetValueToAdd(cycle, value);
                    value += int.Parse(line.Split(" ")[1]);
                }
                
            }

            return sum;
        }

        public List<string> Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            List<string> result = new List<string>();
            int value = 1;
            int cycle = 0;

            string current = "";
            foreach (var line in input)
            {
                if (line == "noop")
                {
                    ComputeCycle(result, value, ref cycle, ref current);
                }
                else
                {
                    ComputeCycle(result, value, ref cycle, ref current);
                    ComputeCycle(result, value, ref cycle, ref current);
                    value += int.Parse(line.Split(" ")[1]);
                }

            }
            return result;
        }

        private static void ComputeCycle(List<string> result, int value, ref int cycle, ref string current)
        {
            cycle++;
            current += Draw(cycle - 1, value);
            if (current.Length == 40)
            {
                result.Add(current);
                current = "";
            }
        }

        public static string Draw(int cycle, int value)
        {
            if ((new List<int> { value -1, value, value + 1 }).Contains(cycle - (cycle / 40) * 40))
            {
                return "#";
            }
            return ".";
        }

        public static int GetValueToAdd(int cycle, int value)
        {
            if ((new List<int> { 20, 60, 100, 140, 180, 220 }).Contains(cycle))
            {
                return cycle * value;
            }
            return 0;
        }
    }
}
