using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day6
    {
        public int Compute(string filePath, int size = 4)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            var splittedLine = input[0].ToCharArray();

            for(int i = 0; i < splittedLine.Length; i++)
            {
                if (splittedLine.Where((v, j) => j >= i && j <= i + size - 1).GroupBy(i => i).Count() == size)
                {
                    return i + size;
                }
            }

            return 0;
        }
    }
}
