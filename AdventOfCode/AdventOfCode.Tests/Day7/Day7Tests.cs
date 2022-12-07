using System.IO;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day7Tests
    {
        private ITestOutputHelper _output;
        public Day7Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day7/Resources/test.txt";
            Day7 day = new Day7();

            // Act
            int result = day.Compute(filePath);

            // Assert
            result.Should().Be(95437);
        }


        [Fact]
        public void SolutionPart1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day7/Resources/input.txt";
            Day7 day = new Day7();

            // Act
            int result = day.Compute(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Test1Part2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day7/Resources/test.txt";
            Day7 day = new Day7();

            // Act
            int result = day.Compute2(filePath);

            // Assert
            result.Should().Be(24933642);
        }


        [Fact]
        public void SolutionPart2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day7/Resources/input.txt";
            Day7 day = new Day7();

            // Act
            int result = day.Compute2(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }
    }
}

