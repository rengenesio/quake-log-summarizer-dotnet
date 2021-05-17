using System;
using AutoFixture;
using FluentAssertions;
using QuakeLogSummarizer.Core.GameEvents;
using QuakeLogSummarizer.Core.LogMessageParser;
using Xunit;

namespace QuakeLogSummarizer.UnitTest.Core.LogMessageParser
{
    public sealed class ClientConnectLogMessageParserTest
    {
        private readonly ClientConnectLogMessageParser _parser;
        private readonly IFixture _fixture;

        public ClientConnectLogMessageParserTest()
        {
            this._fixture = new Fixture();

            this._parser = new ClientConnectLogMessageParser();
        }

        [Fact]
        private void Parse_When_NullLogMessage_Should_ThrowArgumentNullException()
        {
            // Act
            Action act = () => this._parser.Parse(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        private void Parse_When_ValidClientConnectLogMessage_Should_ReturnClientConnectEvent()
        {
            // Arrange
            int expectedPlayerId = this._fixture.Create<int>();
            string logMessage = $"ClientConnect: {expectedPlayerId}";

            // Act
            ClientConnectEvent actual = this._parser.Parse(logMessage);

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
            ClientConnectEvent actual = this._parser.Parse(logMessage);

            // Assert
            actual.Should().BeNull();
        }
    }
}
