using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Language;
using QuakeLogSummarizer.Core;
using QuakeLogSummarizer.Core.GameEvents;
using QuakeLogSummarizer.Core.LogMessageParser;
using QuakeLogSummarizer.Core.Model;
using QuakeLogSummarizer.Infrastructure;
using Xunit;

namespace QuakeLogSummarizer.UnitTest.Core
{
    public sealed class LogSummarizerTest
    {
        private readonly LogSummarizer _summarizer;
        private readonly Mock<ILogMessageExtractor> _logMessageExtractorMock;
        private readonly Mock<ILogFileReader> _logFileReaderMock;
        private readonly Mock<IGameEventsProcessor> _gameEventsProcessorMock;
        private readonly List<Mock<ILogMessageParser>> _logMessageParserMockList;
        private readonly Fixture _fixture;

        public LogSummarizerTest()
        {
            this._fixture = new Fixture();

            this._logMessageExtractorMock = new Mock<ILogMessageExtractor>();
            this._logFileReaderMock = new Mock<ILogFileReader>();
            this._gameEventsProcessorMock = new Mock<IGameEventsProcessor>();

            this._logMessageParserMockList = new List<Mock<ILogMessageParser>>()
            {
                new Mock<ILogMessageParser>()
            };

            this._gameEventsProcessorMock.SetupGet(p => p.GameList)
                .Returns(new List<Game>());

            Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(p => p.GetService(typeof(ILogFileReader)))
                .Returns(this._logFileReaderMock.Object);

            serviceProviderMock.Setup(p => p.GetService(typeof(IGameEventsProcessor)))
                .Returns(this._gameEventsProcessorMock.Object);

            Mock<IServiceScope> serviceScopeMock = new Mock<IServiceScope>();
            serviceScopeMock.SetupGet(s => s.ServiceProvider)
                .Returns(serviceProviderMock.Object);

            Mock<IServiceScopeFactory> serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            serviceScopeFactoryMock.Setup(f => f.CreateScope())
                .Returns(serviceScopeMock.Object);

            this._summarizer = new LogSummarizer(_logMessageExtractorMock.Object, this._logMessageParserMockList.Select(m => m.Object), serviceScopeFactoryMock.Object);
        }

        [Fact]
        private void Summarize_When_NullFileName_Should_ThrowArgumentNullException()
        {
            // Act
            Action act = () => _summarizer.Summarize(null).GetAwaiter().GetResult();

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        private void Summarize_When_EmptyLogFile_Should_NotProcessAnyEvent()
        {
            // Act
            IEnumerable<Game> actual = this._summarizer.Summarize(new Fixture().Create<string>()).GetAwaiter().GetResult();

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        private void Summarize_Should_ReadLogRecordFromFileAndExtractLogMessageAndParseLogMessageAndProcessParsedEvent()
        {
            // Arrange
            string logRecord = this._fixture.Create<string>();
            string logMessage = this._fixture.Create<string>();
            IGameEvent expectedGameEvent = Mock.Of<IGameEvent>();

            this.SetupSequenceLogFileReader(logRecord);
            this._logMessageExtractorMock.Setup(e => e.Extract(logRecord))
                .Returns(logMessage);

            this._logMessageParserMockList.ForEach(p => p.Setup(p => p.Parse(logMessage))
                .Returns(expectedGameEvent));

            // Act
            IEnumerable<Game> actual = this._summarizer.Summarize(new Fixture().Create<string>()).GetAwaiter().GetResult();

            // Assert
            this._gameEventsProcessorMock.Verify(p => p.Process(expectedGameEvent), Times.Once);
        }

        [Fact]
        private void Summarize_When_MoreThanOneParserReturnsEvent_Should_ThrowInvalidOperationException()
        {
            // Arrange
            this._logMessageParserMockList.Add(new Mock<ILogMessageParser>());
            this._logMessageParserMockList.ForEach(p => p.Setup(p => p.Parse(It.IsAny<string>()))
                .Returns(Mock.Of<IGameEvent>()));

            this.SetupSequenceLogFileReader(this._fixture.Create<string>());
            this._logMessageExtractorMock.Setup(e => e.Extract(It.IsAny<string>()))
                .Returns(this._fixture.Create<string>());

            // Act
            Action act = () => this._summarizer.Summarize(new Fixture().Create<string>()).GetAwaiter().GetResult();

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>();
        }

        private void SetupSequenceLogFileReader(params string[] logRecords)
        {
            ISetupSequentialResult<Task<string>> sequence = this._logFileReaderMock.SetupSequence(r => r.ReadLogRecord());
            foreach (string logRecord in logRecords)
            {
                sequence = sequence.ReturnsAsync(logRecord);
            }
            
            sequence.ReturnsAsync((string)null);
        }
    }
}
