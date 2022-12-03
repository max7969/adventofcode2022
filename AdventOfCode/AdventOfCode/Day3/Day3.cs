using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day3
    {
        public int Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            int score = 0;
            foreach (var line in input)
            {
                var rucksackA = line.Substring(0, line.Length / 2);
                var rucksackB = line.Substring(line.Length / 2);

                foreach(char c in rucksackA.ToCharArray())
                {
                    if (rucksackB.Contains(c))
                    {
                        score += ((short)c) >= 97 ? ((short)c) - 96 : ((short)c) - 38;
                        break;
                    }
                }
            }

            return score;
        }

        public int Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            int score = 0;

            for (int i = 0; i < input.Count(); i = i+3)
            {
                var rucksackA = input[i];
                var rucksackB = input[i+1];
                var rucksackC = input[i+2];

                foreach (char c in rucksackA.ToCharArray())
                {
                    if (rucksackB.Contains(c) && rucksackC.Contains(c))
                    {
                        score += ((short)c) >= 97 ? ((short)c) - 96 : ((short)c) - 38;
                        break;
                    }
                }
            }
            return score;
        }
    }
}
