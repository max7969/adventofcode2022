using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using AdventOfCode.Utils;

namespace AdventOfCode
{
    public class Day20
    {

        public class Node
        {
            public long Value { get; set; }
        }
       
        public long Compute(string filePath, long decryptionKey = 1, int mixTimes = 1)
        {
            var input = FileReader.GetFileContent(filePath).ToList();

            Dictionary<long, Node> nodes =  new Dictionary<long, Node>();
            long index = 0;
            long indexOfZero = 0;
            foreach (var line in input)
            {
                long value = long.Parse(line);
                if (value == 0)
                {
                    indexOfZero = index;
                }
                nodes.Add(index, new Node { Value = value * decryptionKey });
                index++;
            }
            
            List<long> indexes = new List<long>(nodes.Keys.ToList());
            for(int j=0; j<mixTimes; j++)
            {
                for (int i = 0; i < nodes.Keys.Count(); i++)
                {
                    var indexMoving = nodes.Keys.ToList()[i];
                    var currentPos = indexes.IndexOf(indexMoving);
                    long a = (currentPos + nodes[indexMoving].Value);
                    long b = indexes.Count() - 1;
                    var newPos = ((a % b) + b) % b;
                    var indexesWithoutMoving = indexes.ToList();
                    indexesWithoutMoving.RemoveAt(currentPos);
                    var firstPart = indexesWithoutMoving.SkipLast(indexes.Count() - (int)newPos - 1).ToList();
                    var lastPart = indexesWithoutMoving.Skip((int)newPos).ToList();
                    firstPart.Add(indexMoving);
                    firstPart.AddRange(lastPart);
                    indexes = firstPart.ToList();
                }
            }

            long sum = 0;
            var currentZeroPos = (int)indexes.IndexOf(indexOfZero);
            for (int i=1;i<=3;i++)
            {
                sum += nodes[indexes[(currentZeroPos + i * 1000) % indexes.Count()]].Value;
            }
            return sum;
        }
    }
}
