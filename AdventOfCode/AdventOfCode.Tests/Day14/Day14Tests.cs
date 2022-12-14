using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day14Tests
    {
        private ITestOutputHelper _output;
        public Day14Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day14/Resources/test.txt";
            Day14 day = new Day14();

            // Act
            long result = day.Compute(filePath);

            // Assert
            result.Should().Be(24);
        }


        [Fact]
        public void SolutionPart1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day14/Resources/input.txt";
            Day14 day = new Day14();

            // Act
            long result = day.Compute(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Test1Part2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day14/Resources/test.txt";
            Day14 day = new Day14();

            // Act
            long result = day.Compute(filePath, true);

            // Assert
            result.Should().Be(93);
        }

        [Fact]
        public void SolutionPart2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day14/Resources/input.txt";
            Day14 day = new Day14();

            // Act
            long result = day.Compute(filePath, true);

            // Result
            _output.WriteLine(result.ToString());
        }
    }
}

