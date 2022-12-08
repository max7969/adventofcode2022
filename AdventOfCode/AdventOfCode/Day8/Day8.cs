using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day8   
    {
       
        public int Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            Dictionary<string, int> result = ExtractResult(input);

            return result.Values.Sum();
        }

        public int Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            Dictionary<string, int> result = ExtractResult(input, true);

            return result.Values.Max();
        }

        private static Dictionary<string, int> ExtractResult(List<string> input, bool scenicMode = false)
        {
            var square = input.Select(x => x.ToCharArray().Select(y => int.Parse(y.ToString())).ToList()).ToList();
            Dictionary<string, int> grid = new Dictionary<string, int>();
            Dictionary<string, int> result = new Dictionary<string, int>();

            for (int i = 0; i < square.Count(); i++)
            {
                for (int j = 0; j < square[i].Count(); j++)
                {
                    grid.Add($"{i},{j}", square[i][j]);
                    result.Add($"{i},{j}", scenicMode ? 1 : 0);
                }
            }

            int iMax = square.Count() - 1;
            int jMax = square[0].Count() - 1;

            for (int i = 0; i <= iMax; i++)
            {
                for (int j = 0; j <= jMax; j++)
                {
                    List<List<string>> sides = new List<List<string>>
                    {
                        Enumerable.Range(0, i > 0 ? i : 0).Select(x => $"{x},{j}").ToList(),
                        Enumerable.Range(i + 1 < iMax ? i + 1 : iMax, iMax - i).Select(x => $"{x},{j}").ToList(),
                        Enumerable.Range(0, j > 0 ? j : 0).Select(x => $"{i},{x}").ToList(),
                        Enumerable.Range(j+ 1 < jMax ? j + 1 : jMax, jMax - j).Select(x => $"{i},{x}").ToList()
                    };

                    if (scenicMode)
                    {
                        for (int k = 0; k < sides.Count; k++)
                        {
                            int countView = 0;
                            foreach (var spot in (k == 0 || k == 2) ? sides[k].OrderByDescending(x => x).ToList() : sides[k])
                            {
                                if (grid[spot] < grid[$"{i},{j}"])
                                {
                                    countView++;
                                }
                                else
                                {
                                    countView++;
                                    break;
                                }
                            }
                            result[$"{i},{j}"] = result[$"{i},{j}"] * countView;
                        }
                    }
                    else
                    {
                        foreach (var side in sides.OrderBy(x => x.Count()))
                        {
                            if (side.Count() == 0 || side.Select(x => grid[x]).All(x => x < grid[$"{i},{j}"]))
                            {
                                result[$"{i},{j}"] = 1;
                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
