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

        [Theory]
        // Some possible values that meets the log format:
        [InlineData(@"  1:23 Record with message",
                    @"Record with message")]
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
        [InlineData("Record: Without any prefix")]
        [InlineData("   Record: With 3 leading whitespaces without time")]
        [InlineData("1 2:34 Record: With invalid minutes portion in time")]
        [InlineData("1:23 Record: With less leading whitespaces than expected")]
        [InlineData("12:34 Record:With less leading whitespaces than expected")]
        [InlineData("   1:23 Record: With more leading whitespaces than expected")]
        [InlineData("  12:34 Record: With more leading whitespaces than expected")]
        [InlineData(" 123:45 Record: With more leading whitespaces than expected")]
        [InlineData("123:456 Record: With more digits than expected")]
        [InlineData("123:45Record: Without whitespace separetor before log message")]
        [InlineData("123:45_Record: Without whitespace separetor before log message")]
        [InlineData(" 12:34 56:78 Record: log at '12:34' was truncated and another record was logged after it")]
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
