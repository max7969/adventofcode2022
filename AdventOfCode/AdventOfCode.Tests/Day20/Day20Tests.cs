using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day20Tests
    {
        private ITestOutputHelper _output;
        public Day20Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day20/Resources/test.txt";
            Day20 day = new Day20();

            // Act
            long result = day.Compute(filePath);

            // Assert
            result.Should().Be(3);
        }


        [Fact]
        public void SolutionPart1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day20/Resources/input.txt";
            Day20 day = new Day20();

            // Act
            long result = day.Compute(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Test1Part2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day20/Resources/test.txt";
            Day20 day = new Day20();

            // Act
            long result = day.Compute(filePath, 811589153, 10);

            // Assert
            result.Should().Be(1623178306);
        }

        [Fact]
        public void SolutionPart2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day20/Resources/input.txt";
            Day20 day = new Day20();

            // Act
            long result = day.Compute(filePath, 811589153, 10);

            // Result
            _output.WriteLine(result.ToString());
        }
    }
}

