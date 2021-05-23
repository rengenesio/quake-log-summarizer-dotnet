using System;
using AutoFixture;
using FluentAssertions;
using QuakeLogSummarizer.Core;
using QuakeLogSummarizer.Core.GameEvents;
using Xunit;

namespace QuakeLogSummarizer.UnitTest.Core
{
    public sealed class GameEventsProcessorTest
    {
        private readonly GameEventsProcessor _processor;
        private readonly Fixture _fixture;

        public GameEventsProcessorTest()
        {
            this._fixture = new Fixture();
            this._processor = new GameEventsProcessor();
        }

        [Fact]
        private void Process_When_WithoutEvents_Should_ContainsAnEmptyGameList()
        {
            // Assert
            this._processor.GameList.Should().BeEmpty();
        }

        [Fact]
        private void Process_When_NullEvent_Should_ThrowArgumentNullException()
        {
            // Act
            Action act = () => this._processor.Process(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        private void Process_When_InitGameEvent_Should_StartNewGame()
        {
            int expectedGameCount = this._fixture.Create<int>();
            for (int i = 0; i < expectedGameCount ; i++)
            {
                // Arrange
                IGameEvent initGameEvent = new InitGameEvent();

                // Act
                this._processor.Process(initGameEvent);
            }

            // Assert
            this._processor.GameList.Should().HaveCount(expectedGameCount);
        }
    }
}
