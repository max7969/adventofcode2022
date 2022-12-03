using System.IO;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode.Tests
{
    public class Day3Tests
    {
        private ITestOutputHelper _output;
        public Day3Tests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test1Part1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day3/Resources/test.txt";
            Day3 day = new Day3();

            // Act
            int result = day.Compute(filePath);

            // Assert
            result.Should().Be(157);
        }


        [Fact]
        public void SolutionPart1()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day3/Resources/input.txt";
            Day3 day = new Day3();

            // Act
            int result = day.Compute(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }

        [Fact]
        public void Test1Part2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day3/Resources/test.txt";
            Day3 day = new Day3();

            // Act
            int result = day.Compute2(filePath);

            // Assert
            result.Should().Be(70);
        }


        [Fact]
        public void SolutionPart2()
        {
            // Arrange 
            string filePath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/Day3/Resources/input.txt";
            Day3 day = new Day3();

            // Act
            int result = day.Compute2(filePath);

            // Result
            _output.WriteLine(result.ToString());
        }
    }
}

