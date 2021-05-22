using AutoFixture;
using FluentAssertions;
using QuakeLogSummarizer.Core.GameEvents;
using QuakeLogSummarizer.Core.LogMessageParser;
using Xunit;

namespace QuakeLogSummarizer.UnitTest.Core.LogMessageParser
{
    public sealed class KillLogMessageParserTest : AbstractLogMessageParserTest<KillLogMessageParser>
    {
        [Fact]
        private void Parse_When_ValidKillLogMessage_Should_ReturnKillEvent()
        {
            // Arrange
            int expectedKillerId = base.Fixture.Create<int>();
            int expectedVictimId = base.Fixture.Create<int>();
            
            string logMessage = $"Kill: {expectedKillerId} {expectedVictimId} {base.Fixture.Create<int>()}: {base.Fixture.Create<string>()} killed {base.Fixture.Create<string>()} by {base.Fixture.Create<string>()}";

            // Act
            IGameEvent actual = base.Parser.Parse(logMessage);

            // Assert
            actual.Should().BeOfType<KillEvent>();
        }

        [Theory]
        [InlineData("Kill: ")]
        [InlineData("Kill: non-integer")]
        [InlineData("NotKill: 1")]
        private void Parse_When_NotKillLogMessage_Should_ReturnNull(string logMessage)
        {
            // Act
            IGameEvent actual = base.Parser.Parse(logMessage);

            // Assert
            actual.Should().BeNull();
        }
    }
}
