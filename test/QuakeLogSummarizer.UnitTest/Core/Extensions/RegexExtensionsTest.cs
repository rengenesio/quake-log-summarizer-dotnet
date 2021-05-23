using System.Text.RegularExpressions;
using AutoFixture;
using FluentAssertions;
using QuakeLogSummarizer.Core.Extensions;
using Xunit;

namespace QuakeLogSummarizer.UnitTest.Extensions
{
    public sealed class RegexExtensionsTest
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        private void ToRegex_When_FormatStringNotContainsFormatIdentifier_Should_ReturnRegexWithoutCapture()
        {
            // Arrange
            string formatString = "Test string without any format specifier.";

            // Act
            Regex actual = formatString.ToRegex();

            // Assert
            actual.IsMatch(formatString).Should().BeTrue();
        }

        [Fact]
        private void ToRegex_When_FormatStringContainsIntegerFormatSpecifier_Should_ExtractIntegerValue()
        {
            // Arrange
            int expectedInteger = this._fixture.Create<int>();
            string formatString = "Test string containing the integer %i as formatted value.";
            string formattedString = $"Test string containing the integer {expectedInteger} as formatted value.";

            // Act
            Regex actual = formatString.ToRegex();

            // Assert
            actual.Match(formattedString).Groups[1].ToString()
                .Should().Be(expectedInteger.ToString());
        }

        [Fact]
        private void ToRegex_When_FormatStringContainsStringFormatSpecifier_Should_ExtractStringValue()
        {
            // Arrange
            string expectedString = this._fixture.Create<string>();
            string formatString = "Test string containing the string %s as formatted value.";
            string formattedString = $"Test string containing the string {expectedString} as formatted value.";

            // Act
            Regex actual = formatString.ToRegex();

            // Assert
            actual.Match(formattedString).Groups[1].ToString()
                .Should().Be(expectedString);
        }

        [Fact]
        private void ToRegex_When_FormatStringContainsStringAndIntFormatSpecifiers_Should_ExtractBothValues()
        {
            // Arrange
            int expectedInteger = this._fixture.Create<int>();
            string expectedString = this._fixture.Create<string>();
            string formatString = "Test string containing %i %s as formatted values.";
            string formattedString = $"Test string containing {expectedInteger} {expectedString} as formatted values.";

            // Act
            Regex actual = formatString.ToRegex();

            // Assert
            Match match = actual.Match(formattedString);
            match.Groups[1].ToString().Should().Be(expectedInteger.ToString());
            match.Groups[2].ToString().Should().Be(expectedString);
        }

        [Fact]
        private void ToRegex_When_NotAppendEndOfLine_Should_ReturnRegexWithoutEndOfLine()
        {
            // Arrange
            string formatString = "Test string without any format specifier";
            string testString = "Test string without any format specifier to test the string above without end of line.";

            // Act
            Regex actual = formatString.ToRegex(false);

            // Assert
            actual.IsMatch(testString).Should().BeTrue();
        }
    }
}
