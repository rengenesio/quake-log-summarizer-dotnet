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
            KillEvent expectedKillEvent = base.Fixture.Create<KillEvent>();
            
            string logMessage = $"Kill: {expectedKillEvent.KillerId} {expectedKillEvent.VictimId} {base.Fixture.Create<int>()}: {base.Fixture.Create<string>()} killed {base.Fixture.Create<string>()} by {base.Fixture.Create<string>()}";

            // Act
            IGameEvent actual = base.Parser.Parse(logMessage);

            // Assert
            actual.Should().BeEquivalentTo(expectedKillEvent);
        }

        [Theory]
        [InlineData("Kill: ")]
        [InlineData("Kill: 1")]
        [InlineData("Kill: 1 2")]
        [InlineData("Kill: 1 2 3")]
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
