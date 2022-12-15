using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day15Tests
    {
        private ITestOutputHelper _output;
        public Day15Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day15/Resources/test.txt";
            Day15 day = new Day15();

            // Act
            long result = day.Compute(filePath, 10);

            // Assert
            result.Should().Be(26);
        }


        [Fact]
        public void SolutionPart1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day15/Resources/input.txt";
            Day15 day = new Day15();

            // Act
            long result = day.Compute(filePath, 2000000);

            // Result
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Test1Part2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day15/Resources/test.txt";
            Day15 day = new Day15();

            // Act
            long result = day.Compute2(filePath, 20);

            // Assert
            result.Should().Be(56000011);
        }

        [Fact]
        public void SolutionPart2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day15/Resources/input.txt";
            Day15 day = new Day15();

            // Act
            long result = day.Compute2(filePath, 4000000);

            // Result
            _output.WriteLine(result.ToString());
        }
    }
}

