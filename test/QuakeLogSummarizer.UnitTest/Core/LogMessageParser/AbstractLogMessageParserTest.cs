using System;
using AutoFixture;
using FluentAssertions;
using QuakeLogSummarizer.Core.LogMessageParser;
using Xunit;

namespace QuakeLogSummarizer.UnitTest.Core.LogMessageParser
{
    public abstract class AbstractLogMessageParserTest<TLogMessageParser, TEvent>
        where TLogMessageParser : AbstractLogMessageParser<TEvent>, new()
    {
        protected IFixture Fixture { get; }
        
        protected TLogMessageParser Parser { get; }

        public AbstractLogMessageParserTest()
        {
            this.Fixture = new Fixture();

            this.Parser = new TLogMessageParser();
        }

        [Fact]
        protected void Parse_When_NullLogMessage_Should_ThrowArgumentNullException()
        {
            // Act
            Action act = () => this.Parser.Parse(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
