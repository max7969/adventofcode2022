using System.IO;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day4Tests
    {
        private ITestOutputHelper _output;
        public Day4Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day4/Resources/test.txt";
            Day4 day = new Day4();

            // Act
            int result = day.Compute(filePath).includes;

            // Assert
            result.Should().Be(2);
        }


        [Fact]
        public void SolutionPart1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day4/Resources/input.txt";
            Day4 day = new Day4();

            // Act
            int result = day.Compute(filePath).includes;

            // Result
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Test1Part2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day4/Resources/test.txt";
            Day4 day = new Day4();

            // Act
            int result = day.Compute(filePath).overlaps;

            // Assert
            result.Should().Be(4);
        }


        [Fact]
        public void SolutionPart2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day4/Resources/input.txt";
            Day4 day = new Day4();

            // Act
            int result = day.Compute(filePath).overlaps;

            // Result
            _output.WriteLine(result.ToString());
        }
    }
}

