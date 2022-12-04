using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day4
    {
        public class Elfe
        {
            public int Start { get; set; }
            public int End { get; set; }

            public bool IsIncluding(Elfe elfe)
            {
                return elfe.Start >= Start && elfe.End <= End;
            }

            public bool IsOverlapping(Elfe elfe)
            {
                return (elfe.End >= Start && elfe.End <= End) || (elfe.Start >= Start && elfe.Start <= End);
            }
        }
        public (int includes, int overlaps) Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            List<(Elfe e1, Elfe e2)> couples = new List<(Elfe e1, Elfe e2)>();
            foreach(var line in input)
            {
                var split = line.Split(",");
                couples.Add( 
                    (new Elfe { Start = int.Parse(split[0].Split("-")[0]), End = int.Parse(split[0].Split("-")[1]) },
                     new Elfe { Start = int.Parse(split[1].Split("-")[0]), End = int.Parse(split[1].Split("-")[1]) })
                );
            }

            return (couples.Count(x => x.e1.IsIncluding(x.e2) || x.e2.IsIncluding(x.e1)),
                    couples.Count(x => x.e1.IsOverlapping(x.e2) || x.e2.IsOverlapping(x.e1)));
        }
    }
}
