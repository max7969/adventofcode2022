using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day1
    {
        public int Compute(string filePath, int topCount = 1)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            List<int> elfes = new List<int>();
            int current = 0;
            foreach (var line in input)
            {
                if (line == string.Empty)
                {
                    elfes.Add(current);
                    current = 0;
                } 
                else
                {
                    current += int.Parse(line);
                }
            }
            elfes.Add(current);

            return elfes.OrderByDescending(x => x).Take(topCount).Sum();
        }



    }
}
