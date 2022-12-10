using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day10Tests
    {
        private ITestOutputHelper _output;
        public Day10Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day10/Resources/test.txt";
            Day10 day = new Day10();

            // Act
            int result = day.Compute(filePath);

            // Assert
            result.Should().Be(13140);
        }


        [Fact]
        public void SolutionPart1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day10/Resources/input.txt";
            Day10 day = new Day10();

            // Act
            int result = day.Compute(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Test1Part2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day10/Resources/test.txt";
            Day10 day = new Day10();

            // Act
            List<string> result = day.Compute2(filePath);

            // Assert
            result[0].Should().Be("##..##..##..##..##..##..##..##..##..##..");
            result[1].Should().Be("###...###...###...###...###...###...###.");
            result[2].Should().Be("####....####....####....####....####....");
            result[3].Should().Be("#####.....#####.....#####.....#####.....");
            result[4].Should().Be("######......######......######......####");
            result[5].Should().Be("#######.......#######.......#######.....");
        }

        [Fact]
        public void SolutionPart2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day10/Resources/input.txt";
            Day10 day = new Day10();

            // Act
            List<string> result = day.Compute2(filePath);

            // Result
            foreach (var line in result)
            {
                _output.WriteLine(line);
            }
        }
    }
}

