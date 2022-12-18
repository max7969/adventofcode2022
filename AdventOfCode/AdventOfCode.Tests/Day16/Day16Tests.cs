using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day16Tests
    {
        private ITestOutputHelper _output;
        public Day16Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day16/Resources/test.txt";
            Day16 day = new Day16();

            // Act
            long result = day.Compute(filePath);

            // Assert
            result.Should().Be(1651);
        }


        [Fact]
        public void SolutionPart1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day16/Resources/input.txt";
            Day16 day = new Day16();

            // Act
            long result = day.Compute(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Test1Part2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day16/Resources/test.txt";
            Day16 day = new Day16();

            // Act
            long result = day.Compute2(filePath);

            // Assert
            result.Should().Be(1707);
        }

        [Fact]
        public void SolutionPart2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day16/Resources/input.txt";
            Day16 day = new Day16();

            // Act
            long result = day.Compute2(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }
    }
}

