using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Utils
{
    public class FileReader
    {
        public static IEnumerable<string> GetFileContent(string path)
        {
            return File.ReadAllLines(path).ToList();
        }
    }
}