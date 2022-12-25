using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day22
    {
        public class Person
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int OrientationX { get; set; }
            public int OrientationY { get; set; }

            public int Score()
            {
                int valueOrientation =  OrientationX != 0 ? (OrientationX == 1 ? 1 : 3) : (OrientationY == 1 ? 0 : 2);
                return X * 1000 + Y * 4 + valueOrientation;
            }
        }

        public long Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            Dictionary<string, string> map = new Dictionary<string, string>();
            Person person = new Person { OrientationX = 0, OrientationY = 1 };
            InitMapAndPerson(input, map, person);
            string orders = input.Last();

            while (orders != string.Empty)
            {
                if (orders.StartsWith("R"))
                {
                    orders = RotateR(person, orders);
                }
                else if (orders.StartsWith("L"))
                {
                    orders = RotateL(person, orders);
                }
                else
                {
                    Regex rx = new Regex(@"^([0-9]+)");
                    Match match = rx.Match(orders);
                    var value = int.Parse(match.Captures[0].Value);
                    orders = orders[value.ToString().Length..];
                    if (person.OrientationX == 0)
                    {
                        var interesting = new Dictionary<string, string>(map.Where(x => x.Key.StartsWith($"{person.X},")));
                        int maxY = interesting.Keys.Select(x => int.Parse(x.Split(",")[1])).Max();
                        int minY = interesting.Keys.Select(x => int.Parse(x.Split(",")[1])).Min();

                        for (int i = 1; i <= value; i++)
                        {
                            var testY = 0;
                            if (person.OrientationY < 0)
                            {
                                testY = person.Y + person.OrientationY == minY - 1 ? maxY : person.Y + person.OrientationY;
                            }
                            else
                            {
                                testY = person.Y + person.OrientationY == maxY + 1 ? minY : person.Y + person.OrientationY;
                            }

                            if (interesting.ContainsKey($"{person.X},{testY}") && interesting[$"{person.X},{testY}"] == ".")
                            {
                                person.Y = testY;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        var interesting = new Dictionary<string, string>(map.Where(x => x.Key.EndsWith($",{person.Y}")));
                        int maxX = interesting.Keys.Select(x => int.Parse(x.Split(",")[0])).Max();
                        int minX = interesting.Keys.Select(x => int.Parse(x.Split(",")[0])).Min();

                        for (int i = 1; i <= value; i++)
                        {
                            var testX = 0;
                            if (person.OrientationX < 0)
                            {
                                testX = person.X + person.OrientationX == minX - 1 ? maxX : person.X + person.OrientationX;
                            }
                            else
                            {
                                testX = person.X + person.OrientationX == maxX + 1 ? minX : person.X + person.OrientationX;
                            }

                            if (interesting.ContainsKey($"{testX},{person.Y}") && interesting[$"{testX},{person.Y}"] == ".")
                            {
                                person.X = testX;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return person.Score();
        }


        public long Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            Dictionary<string, string> map = new Dictionary<string, string>();
            Person person = new Person { OrientationX = 0, OrientationY = 1 };
            InitMapAndPerson(input, map, person);
            string orders = input.Last();

            while (orders != string.Empty)
            {
                if (orders.StartsWith("R"))
                {
                    orders = RotateR(person, orders);
                }
                else if (orders.StartsWith("L"))
                {
                    orders = RotateL(person, orders);
                }
                else
                {
                }
            }
            return 0;
        }

        private static string RotateL(Person person, string orders)
        {
            if (person.OrientationX == 0 && person.OrientationY == 1)
            {
                person.OrientationX = -1;
                person.OrientationY = 0;
            }
            else if (person.OrientationX == 0 && person.OrientationY == -1)
            {
                person.OrientationX = 1;
                person.OrientationY = 0;
            }
            else if (person.OrientationX == 1 && person.OrientationY == 0)
            {
                person.OrientationX = 0;
                person.OrientationY = 1;
            }
            else if (person.OrientationX == -1 && person.OrientationY == 0)
            {
                person.OrientationX = 0;
                person.OrientationY = -1;
            }
            orders = orders[1..];
            return orders;
        }

        private static string RotateR(Person person, string orders)
        {
            if (person.OrientationX == 0 && person.OrientationY == 1)
            {
                person.OrientationX = 1;
                person.OrientationY = 0;
            }
            else if (person.OrientationX == 0 && person.OrientationY == -1)
            {
                person.OrientationX = -1;
                person.OrientationY = 0;
            }
            else if (person.OrientationX == 1 && person.OrientationY == 0)
            {
                person.OrientationX = 0;
                person.OrientationY = -1;
            }
            else if (person.OrientationX == -1 && person.OrientationY == 0)
            {
                person.OrientationX = 0;
                person.OrientationY = 1;
            }
            orders = orders[1..];
            return orders;
        }

        private static void InitMapAndPerson(List<string> input, Dictionary<string, string> map, Person person)
        {
            for (int i = 0; i < input.Count(); i++)
            {
                if (input[i] == String.Empty)
                {
                    break;
                }
                for (int j = 0; j < input[i].ToCharArray().Count(); j++)
                {
                    if (map.Count() == 0)
                    {
                        person.X = i + 1;
                        person.Y = j + 1;
                    }

                    if (input[i].ToCharArray()[j] != ' ')
                    {
                        map.Add($"{i + 1},{j + 1}", input[i].ToCharArray()[j].ToString());
                    }
                }
            }
        }
    }
}
