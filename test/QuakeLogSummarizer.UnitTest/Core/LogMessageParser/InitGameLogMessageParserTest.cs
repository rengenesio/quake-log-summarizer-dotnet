using System;
using AutoFixture;
using FluentAssertions;
using QuakeLogSummarizer.Core.GameEvents;
using QuakeLogSummarizer.Core.LogMessageParser;
using Xunit;

namespace QuakeLogSummarizer.UnitTest.Core.LogMessageParser
{
    public sealed class InitGameLogMessageParserTest
    {
        private readonly InitGameLogMessageParser _parser;

        public InitGameLogMessageParserTest()
        {
            this._parser = new InitGameLogMessageParser();
        }

        [Fact]
        private void Parse_When_NullLogMessage_Should_ThrowArgumentNullException()
        {
            // Act
            Action act = () => this._parser.Parse(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData(@"InitGame: ")]
        [InlineData(@"InitGame: game data")]
        [InlineData(@"InitGame: \sv_floodProtect\1\sv_maxPing\0\sv_minPing\0\sv_maxRate\10000\sv_minRate\0\sv_hostname\Code Miner Server\g_gametype\0\sv_privateClients\2\sv_maxclients\16\sv_allowDownload\0\dmflags\0\fraglimit\20\timelimit\15\g_maxGameClients\0\capturelimit\8\version\ioq3 1.36 linux-x86_64 Apr 12 2009\protocol\68\mapname\q3dm17\gamename\baseq3\g_needpass\0")]
        private void Parse_When_ValidInitGameLogMessage_Should_ReturnInitGameEvent(string logMessage)
        {
            // Act
            InitGameEvent actual = this._parser.Parse(logMessage);

            // Assert
            actual.Should().BeOfType<InitGameEvent>();
        }

        [Theory]
        [InlineData("InitGame:")]
        [InlineData("NotInitGame: 1")]
        private void Parse_When_NotClientConnectLogMessage_Should_ReturnNull(string logMessage)
        {
            // Act
            InitGameEvent actual = this._parser.Parse(logMessage);

            // Assert
            actual.Should().BeNull();
        }
    }
}
