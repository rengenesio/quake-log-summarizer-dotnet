using AutoFixture;
using FluentAssertions;
using QuakeLogSummarizer.Core.GameEvents;
using QuakeLogSummarizer.Core.LogMessageParser;
using Xunit;

namespace QuakeLogSummarizer.UnitTest.Core.LogMessageParser
{
    public sealed class ClientConnectLogMessageParserTest : AbstractLogMessageParserTest<ClientConnectLogMessageParser, ClientConnectEvent>
    {
        [Fact]
        private void Parse_When_ValidClientConnectLogMessage_Should_ReturnClientConnectEvent()
        {
            // Arrange
            int expectedPlayerId = base.Fixture.Create<int>();
            string logMessage = $"ClientConnect: {expectedPlayerId}";

            // Act
            ClientConnectEvent actual = base.Parser.Parse(logMessage);

            // Assert
            actual.PlayerId.Should().Be(expectedPlayerId);
        }

        [Theory]
        [InlineData("ClientConnect: ")]
        [InlineData("ClientConnect: non-integer")]
        [InlineData("NotClientConnect: 1")]
        private void Parse_When_NotClientConnectLogMessage_Should_ReturnNull(string logMessage)
        {
            // Act
            ClientConnectEvent actual = base.Parser.Parse(logMessage);

            // Assert
            actual.Should().BeNull();
        }
    }
}
