using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day17
    {
        public class Form
        {
            public List<(long x, long y)> Coordinates { get; set; } = new List<(long x, long y)>();

            public void MoveHorizontaly(HashSet<string> map, long move)
            {
                var newCoordinates = Coordinates.Select(x => (x.x, x.y + move)).ToList<(long x, long y)>();

                if (newCoordinates.Any(x => x.y <= 0 || x.y >= 8))
                {
                    return;
                }
                foreach (var coordinate in newCoordinates)
                {
                    if (map.Contains($"{coordinate.x};{coordinate.y}"))
                    {
                        return;
                    }
                }
                Coordinates = newCoordinates;
            }

            public bool MoveDown(HashSet<string> map)
            {
                var newCoordinates = Coordinates.Select(x => (x.x - 1, x.y)).ToList<(long x, long y)>();
                if (newCoordinates.Any(x => x.x <= 0))
                {
                    return false;
                }
                foreach (var coordinate in newCoordinates)
                {
                    if (map.Contains($"{coordinate.x};{coordinate.y}"))
                    {
                        return false;
                    }
                }
                Coordinates = newCoordinates;
                return true;
            }
        }

        public Form GetNewForm(long turn, long x)
        {
            if (turn % 5 == 1)
            {
                return new Form { Coordinates = new List<(long x, long y)> { (x, 3), (x, 4), (x, 5), (x, 6) } };
            }
            else if (turn % 5 == 2)
            {
                return new Form { Coordinates = new List<(long x, long y)> { (x, 4), (x + 1, 3), (x + 1, 4), (x + 1, 5), (x + 2, 4) } };
            }
            else if (turn % 5 == 3)
            {
                return new Form { Coordinates = new List<(long x, long y)> { (x, 3), (x, 4), (x, 5), (x + 1, 5), (x + 2, 5) } };
            }
            else if (turn % 5 == 4)
            {
                return new Form { Coordinates = new List<(long x, long y)> { (x, 3), (x + 1, 3), (x + 2, 3), (x + 3, 3) } };
            }
            else
            {
                return new Form { Coordinates = new List<(long x, long y)> { (x, 3), (x + 1, 3), (x, 4), (x + 1, 4) } };
            }
        }

        public long Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            long[] moves = input[0].ToCharArray().Select(x => x == '<' ? -1L : 1L).ToArray();
            long highestPos = 0;
            long indexMoves = 0;
            HashSet<string> map = new HashSet<string>();

            for (long turn = 1; turn < 2023; turn++)
            {
                Form newForm = GetNewForm(turn, highestPos + 4);
                bool isMoving = true;
                while (isMoving)
                {
                    newForm.MoveHorizontaly(map, moves[indexMoves % moves.Length]);
                    indexMoves++;
                    isMoving = newForm.MoveDown(map);
                }
                foreach(var coordinate in newForm.Coordinates)
                {
                    map.Add($"{coordinate.x};{coordinate.y}");
                }
                highestPos = map.Select(x => long.Parse(x.Split(";")[0])).Max();
            }
            return highestPos;
        }

        public string BuildConfig(HashSet<string> map, long form, long indexMoves)
        {
            List<(long x, long y)> config = new List<(long x, long y)>();
            for (int i=1;i<=7;i++)
            {
                var test = map.Select(x => x.Split(";").Select(y => long.Parse(y)).ToList()).Where(x => x[1] == i).ToList();
                if (test.Any())
                {
                    config.Add((test.Select(x => x[0]).Max(), i));
                } else
                {
                    config.Add((0, i));
                }
            }
            var xMin = config.Min(x => x.x);
            config = config.Select(x => (x.x - xMin, x.y)).ToList();
            return String.Join(";", config.Select(x => $"{x.x},{x.y}")) + "|" + form + "|" + indexMoves;
        }

        public long Compute2(string filePath, long target = 1000000000000)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            long[] moves = input[0].ToCharArray().Select(x => x == '<' ? -1L : 1L).ToArray();
            long highestPos = 0;
            long indexMoves = 0;
            HashSet<string> map = new HashSet<string>();
            Dictionary<string, (long turn, long high)> pastConfig = new Dictionary<string, (long turn, long high)>();

            for (long turn = 1; turn < 100000000; turn++)
            {
                Form newForm = GetNewForm(turn, highestPos + 4);
                bool isMoving = true;
                while (isMoving)
                {
                    newForm.MoveHorizontaly(map, moves[indexMoves % moves.Length]);
                    indexMoves++;
                    isMoving = newForm.MoveDown(map);
                }
                foreach (var coordinate in newForm.Coordinates)
                {
                    map.Add($"{coordinate.x};{coordinate.y}");
                }
                highestPos = map.Select(x => long.Parse(x.Split(";")[0])).Max();

                if (map.Count > 1000)
                {
                    map = map.TakeLast(1000).ToHashSet();
                }
                string config = BuildConfig(map, turn % 5, indexMoves % moves.Length);
                if (pastConfig.ContainsKey(config))
                {
                    var compareConfig = pastConfig[config];
                    var highnessBeforeRepetition = compareConfig.high;
                    var startRepetition = target - compareConfig.turn;
                    var numberOfStrictRepetition = startRepetition / (turn - compareConfig.turn);
                    var repetitionIncrease = highestPos - compareConfig.high;
                    var rest = startRepetition % (turn - compareConfig.turn);
                    var growth = numberOfStrictRepetition * repetitionIncrease;
                    var test = pastConfig.Values.Where(x => x.turn == compareConfig.turn + rest).Single();
                    return highnessBeforeRepetition + growth + (test.high - compareConfig.high);
                } 
                else
                {
                    pastConfig.Add(config, (turn, highestPos));
                }
                
            }
            return highestPos;
        }
    }
}
