using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day11Tests
    {
        private ITestOutputHelper _output;
        public Day11Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day11/Resources/test.txt";
            Day11 day = new Day11();

            // Act
            long result = day.Compute(filePath);

            // Assert
            result.Should().Be(10605);
        }


        [Fact]
        public void SolutionPart1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day11/Resources/input.txt";
            Day11 day = new Day11();

            // Act
            long result = day.Compute(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Test1Part2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day11/Resources/test.txt";
            Day11 day = new Day11();

            // Act
            long result = day.Compute(filePath, 10000, true);

            // Assert
            result.Should().Be(2713310158);
        }

        [Fact]
        public void SolutionPart2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day11/Resources/input.txt";
            Day11 day = new Day11();

            // Act
            long result = day.Compute(filePath, 10000, true);

            // Result
            _output.WriteLine(result.ToString());
        }
    }
}

