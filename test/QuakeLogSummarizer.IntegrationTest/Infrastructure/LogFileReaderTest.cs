using System;
using System.Collections.Generic;
using System.IO;
using AutoFixture;
using FluentAssertions;
using QuakeLogSummarizer.Infrastructure;
using Xunit;

namespace QuakeLogSummarizer.IntegrationTest.Infrastructure
{
    public sealed class LogFileReaderTest
    {
        private readonly LogFileReader _reader;
        private readonly Fixture _fixture;

        public LogFileReaderTest()
        {
            this._fixture = new Fixture();
            this._reader = new LogFileReader();
        }

        [Fact]
        private void BeginReadJob_When_StreamIsNull_Should_ThrowArgumentNullException()
        {
            // Act
            Action act = () => this._reader.BeginReadJob(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        private void BeginReadJob_When_StreamIsValid_Should_NotThrow()
        {
            // Arrange
            Stream stream = new MemoryStream();

            // Act
            Action act = () => this._reader.BeginReadJob(stream);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        private void ReadLogRecord_When_BeginReadJobNotExecuted_Should_ThrowInvalidOperationException()
        {
            // Act
            Action act = () => this._reader.ReadLogRecordAsync().GetAwaiter().GetResult();

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>();
        }

        [Fact]
        private void ReadLogRecord_When_EmptyStream_Should_ReturnNull()
        {
            // Arrange
            Stream stream = new MemoryStream();
            this._reader.BeginReadJob(stream);

            // Act
            string actual = this._reader.ReadLogRecordAsync().GetAwaiter().GetResult();

            // Assert
            actual.Should().BeNull();
        }

        [Fact]
        private void ReadLogRecord_When_StreamHasRecords_Should_ReturnAllRecords()
        {
            using (Stream stream = new MemoryStream())
            using (StreamWriter streamWriter = new StreamWriter(stream))
            {
                // Arrange
                IEnumerable<string> recordList = this._fixture.CreateMany<string>();

                foreach (string record in recordList)
                {
                    streamWriter.WriteLine(record);
                }
                
                streamWriter.Flush();
                stream.Seek(0, SeekOrigin.Begin);

                this._reader.BeginReadJob(stream);

                IList<string> actualList = new List<string>();
                string actual;

                // Act
                while ((actual = this._reader.ReadLogRecordAsync().GetAwaiter().GetResult()) != null)
                {
                    actualList.Add(actual);
                }

                // Assert
                actualList.Should().BeEquivalentTo(recordList);
            }
        }

        private string CreateTempFile(IEnumerable<string> recordList = null)
        {
            string fileFullname = Path.GetTempFileName();

            if (recordList != null)
            {
                
            }

            return fileFullname;
        }
    }
}
