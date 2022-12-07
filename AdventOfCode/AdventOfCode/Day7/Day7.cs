using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day7   
    {
        public class Element
        {
            public string Name { get; set; }
            public Element Parent { get; set; }
            public List<Element> Childs { get; set; } = new List<Element>();
            public bool IsFile { get; set; }
            public int Size { get; set; }
            public int TotalSize
            {
                get
                {
                    return IsFile ? Size : Childs.Select(x => x.TotalSize).Sum();
                }
            }

            public string FullPath
            {
                get
                {
                    if (Parent == null)
                    {
                        return "";
                    }
                    else
                    {
                        return Parent.FullPath + "/" + Name;
                    }
                }
            }
        }

        public int Compute(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            Dictionary<string, Element> folders = ExtractFolders(input);

            return folders.Values.Select(x => x.TotalSize).Where(x => x < 100000).Sum();
        }

        public int Compute2(string filePath)
        {
            var input = FileReader.GetFileContent(filePath).ToList();
            Dictionary<string, Element> folders = ExtractFolders(input);
            int space = 70000000;
            int need = 30000000;

            int occupied = folders["/"].TotalSize;
            int needToFree = need - (space - occupied);

            return folders.Values.Select(x => x.TotalSize).Where(x => x > needToFree).Min();
        }

        private static Dictionary<string, Element> ExtractFolders(List<string> input)
        {
            Dictionary<string, Element> folders = new Dictionary<string, Element>();
            folders.Add("/", new Element { Name = "/" });
            Element current = folders["/"];

            Regex rxCd = new Regex(@"cd ([a-z\./]+)");
            Regex rxFile = new Regex(@"([0-9]+) ([a-z\.]+)");
            Regex rxDir = new Regex(@"dir ([a-z]+)");

            foreach (var line in input)
            {
                if (rxCd.Match(line).Groups[1].Value == "..")
                {
                    current = current.Parent != null ? current.Parent : folders["/"];
                }
                else if (rxCd.Match(line).Success)
                {
                    if (rxCd.Match(line).Groups[1].Value == "/")
                    {
                        current = folders["/"];
                    }
                    else
                    {
                        current = folders[current.FullPath + "/" + rxCd.Match(line).Groups[1].Value];
                    }
                }
                else if (rxFile.Match(line).Success)
                {
                    current.Childs.Add(new Element
                    {
                        Name = rxFile.Match(line).Groups[2].Value,
                        Size = int.Parse(rxFile.Match(line).Groups[1].Value),
                        IsFile = true,
                        Parent = current
                    }); ;
                }
                else if (rxDir.Match(line).Success)
                {
                    Element dir = new Element
                    {
                        Name = rxDir.Match(line).Groups[1].Value,
                        Parent = current
                    };

                    if (!folders.Keys.Contains(dir.FullPath))
                    {
                        current.Childs.Add(dir);
                        folders.Add(dir.FullPath, dir);
                    }
                }
            }

            return folders;
        }
    }
}
