using System.IO;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day5Tests
    {
        private ITestOutputHelper _output;
        public Day5Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day5/Resources/test.txt";
            Day5 day = new Day5();

            // Act
            string result = day.Compute(filePath);

            // Assert
            result.Should().Be("CMZ");
        }


        [Fact]
        public void SolutionPart1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day5/Resources/input.txt";
            Day5 day = new Day5();

            // Act
            string result = day.Compute(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Test1Part2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day5/Resources/test.txt";
            Day5 day = new Day5();

            // Act
            string result = day.Compute(filePath, true);

            // Assert
            result.Should().Be("MCD");
        }


        [Fact]
        public void SolutionPart2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day5/Resources/input.txt";
            Day5 day = new Day5();

            // Act
            string result = day.Compute(filePath, true);

            // Result
            _output.WriteLine(result.ToString());
        }
    }
}

