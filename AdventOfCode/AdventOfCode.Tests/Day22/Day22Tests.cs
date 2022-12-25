using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day22Tests
    {
        private ITestOutputHelper _output;
        public Day22Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day22/Resources/test.txt";
            Day22 day = new Day22();

            // Act
            long result = day.Compute(filePath);

            // Assert
            result.Should().Be(6032);
        }


        [Fact]
        public void SolutionPart1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day22/Resources/input.txt";
            Day22 day = new Day22();

            // Act
            long result = day.Compute(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Test1Part2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day22/Resources/test.txt";
            Day22 day = new Day22();

            // Act
            long result = day.Compute2(filePath);

            // Assert
            result.Should().Be(301);
        }

        [Fact]
        public void SolutionPart2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day22/Resources/input.txt";
            Day22 day = new Day22();

            // Act
            long result = day.Compute2(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }
    }
}

