using AutoFixture;
using FluentAssertions;
using QuakeLogSummarizer.Core.GameEvents;
using QuakeLogSummarizer.Core.LogMessageParser;
using Xunit;

namespace QuakeLogSummarizer.UnitTest.Core.LogMessageParser
{
    public sealed class ClientUserInfoChangedLogMessageParserTest : AbstractLogMessageParserTest<ClientUserInfoChangedLogMessageParser>
    {
        [Fact]
        private void Parse_When_ValidClientConnectLogMessage_Should_ReturnClientConnectEvent()
        {
            // Arrange
            int expectedPlayerId = base.Fixture.Create<int>();
            string expectedPlayerName = base.Fixture.Create<string>();
            string logMessage = @$"ClientUserinfoChanged: {expectedPlayerId} n\{expectedPlayerName}\t\\0\model\xian/default\hmodel\xian/default\g_redteam\\g_blueteam\\c1\4\c2\5\hc\100\w\0\l\0\tt\0\tl\0";

            // Act
            IGameEvent actual = base.Parser.Parse(logMessage);

            // Assert
            actual.Should().BeOfType<ClientUserInfoChangedEvent>()
                .Which.Should().Match<ClientUserInfoChangedEvent>(e => e.PlayerId == expectedPlayerId &&
                                                                       e.PlayerName == expectedPlayerName);
        }

        [Theory]
        [InlineData("ClientUserinfoChanged: ")]
        [InlineData("ClientUserinfoChanged: non-integer")]
        [InlineData("NotClientUserinfoChanged: 1")]
        private void Parse_When_NotClientConnectLogMessage_Should_ReturnNull(string logMessage)
        {
            // Act
            IGameEvent actual = base.Parser.Parse(logMessage);

            // Assert
            actual.Should().BeNull();
        }
    }
}
