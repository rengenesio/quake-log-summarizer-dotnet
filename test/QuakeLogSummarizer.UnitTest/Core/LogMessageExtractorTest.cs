using System;
using FluentAssertions;
using QuakeLogSummarizer.Core;
using Xunit;

namespace QuakeLogSummarizer.UnitTest
{
    public sealed class LogMessageExtractorTest
    {
        private readonly LogMessageExtractor _extractor;

        public LogMessageExtractorTest()
        {
            this._extractor = new LogMessageExtractor();
        }

        [Fact]
        private void Extract_When_NullLogRecord_Should_ThrowArgumentNullException()
        {
            // Act
            Action act = () => this._extractor.Extract(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        // Some possible values that meets the log format:
        [InlineData(@"  1:23 Case: record with message",
                    @"Case: record with message")]
        [InlineData(@"   1:23 Case: previous record was truncated after first whitespace but record at 1:23 is ok",
                    @"Case: previous record was truncated after first whitespace but record at 1:23 is ok")]
        [InlineData(@"   1:23  4:56 Case: record from 1:23 was truncated after time but record from 4:56 is ok",
                    @"Case: record from 1:23 was truncated after time but record from 4:56 is ok")]
        [InlineData(@"   1:23 Case: record truncated  4:56 Case: previous record was truncated but this is ok",
                    @"Case: previous record was truncated but this is ok")]
        [InlineData(@"   1:23 Case: record truncated  4:56 Case: another record truncated  7:89 Case: valid record after two truncated records",
                    @"Case: valid record after two truncated records")]
        // G_LogPrintf used formats:
        [InlineData(@"  1:23 ------------------------------------------------------------",
                    @"------------------------------------------------------------")]
        [InlineData(@"  1:23 ClientUserinfoChanged: %i %s",
                    @"ClientUserinfoChanged: %i %s")]
        [InlineData(@"  1:23 ClientConnect: %i",
                    @"ClientConnect: %i")]
        [InlineData(@"  1:23 ClientBegin: %i",
                    @"ClientBegin: %i")]
        [InlineData(@"  1:23 ClientDisconnect: %i",
                    @"ClientDisconnect: %i")]
        [InlineData(@"  1:23 Exit: %s",
                    @"Exit: %s")]
        [InlineData(@"  1:23 InitGame: %s",
                    @"InitGame: %s")]
        [InlineData(@"  1:23 Item: %i %s",
                    @"Item: %i %s")]
        [InlineData(@"  1:23 Kill: %i %i %i: %s killed %s by %s",
                    @"Kill: %i %i %i: %s killed %s by %s")]
        [InlineData(@"  1:23 ShutdownGame:",
                    @"ShutdownGame:")]
        [InlineData(@"  1:23 Warmup:",
                    @"Warmup:")]
        [InlineData(@"  1:23 red:%i  blue:%i",
                    @"red:%i  blue:%i")]
        [InlineData(@"  1:23 say: %s: %s",
                    @"say: %s: %s")]
        [InlineData(@"  1:23 sayteam: %s: %s",
                    @"sayteam: %s: %s")]
        [InlineData(@"  1:23 score: %i  ping: %i  client: %i %s",
                    @"score: %i  ping: %i  client: %i %s")]
        [InlineData(@"  1:23 tell: %s to %s: %s",
                    @"tell: %s to %s: %s")]
        [InlineData(@"  1:23 vtell: %s to %s: %s",
                    @"vtell: %s to %s: %s")]
        // Real log examples:
        [InlineData(@"  0:00 ------------------------------------------------------------",
                    "------------------------------------------------------------")]
        [InlineData(@"  0:00 InitGame: \sv_floodProtect\1\sv_maxPing\0\sv_minPing\0\sv_maxRate\10000\sv_minRate\0\sv_hostname\Code Miner Server\g_gametype\0\sv_privateClients\2\sv_maxclients\16\sv_allowDownload\0\dmflags\0\fraglimit\20\timelimit\15\g_maxGameClients\0\capturelimit\8\version\ioq3 1.36 linux - x86_64 Apr 12 2009\protocol\68\mapname\q3dm17\gamename\baseq3\g_needpass\0",
                    @"InitGame: \sv_floodProtect\1\sv_maxPing\0\sv_minPing\0\sv_maxRate\10000\sv_minRate\0\sv_hostname\Code Miner Server\g_gametype\0\sv_privateClients\2\sv_maxclients\16\sv_allowDownload\0\dmflags\0\fraglimit\20\timelimit\15\g_maxGameClients\0\capturelimit\8\version\ioq3 1.36 linux - x86_64 Apr 12 2009\protocol\68\mapname\q3dm17\gamename\baseq3\g_needpass\0")]
        [InlineData(@" 20:37 ClientBegin: 2",
                    @"ClientBegin: 2")]
        [InlineData(@"981:06 ClientConnect: 2",
                    @"ClientConnect: 2")]
        private void Extract_When_LogRecordHasValidFormat_Should_ReturnExpectedLogMessage(string logRecord, string expected)
        {
            // Act
            string actual = this._extractor.Extract(logRecord);

            // Assert
            actual.Should().Be(expected);
        }

        [Theory]
        [InlineData("Record: without time prefix")]
        [InlineData("   Record: with 3 leading whitespaces without time")]
        [InlineData("   :12 Record: without minutes portion of time")]
        [InlineData("1 2:34 Record: with invalid minutes portion in time")]
        [InlineData("1:23 Record: with less leading whitespaces than expected")]
        [InlineData("12:34 Record: with less leading whitespaces than expected")]
        [InlineData("123:456 Record: with more digits than expected")]
        [InlineData("123:45Record: without whitespace separator before log message")]
        [InlineData("123:45_Record: without whitespace separator before log message")]
        [InlineData(" 12:34 Record without any tag (letters sequence before a colon)")]
        [InlineData(" 12:34 ")] // Record truncated and without other record logged after it
        private void Extract_When_LogRecordHasInvalidFormat_Should_ReturnNull(string logRecord)
        {
            // Act
            string actual = this._extractor.Extract(logRecord);

            // Assert
            actual.Should().BeNull();
        }
    }
}
