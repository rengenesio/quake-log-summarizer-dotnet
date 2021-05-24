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
        private void BeginReadJob_When_FileNotExists_Should_ThrowFileNotFoundException()
        {
            // Act
            Action act = () => this._reader.BeginReadJob(this._fixture.Create<string>());

            // Assert
            act.Should().ThrowExactly<FileNotFoundException>();
        }

        [Fact]
        private void BeginReadJob_When_FileExists_Should_NotThrow()
        {
            // Arrange
            string fileFullname = this.CreateTempFile();

            // Act
            Action act = () => this._reader.BeginReadJob(fileFullname);

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
        private void ReadLogRecord_When_EmptyFile_Should_ReturnNull()
        {
            // Arrange
            string fileFullname = this.CreateTempFile();
            this._reader.BeginReadJob(fileFullname);

            // Act
            string actual = this._reader.ReadLogRecordAsync().GetAwaiter().GetResult();

            // Assert
            actual.Should().BeNull();
        }

        [Fact]
        private void ReadLogRecord_When_FileHasRecords_Should_ReturnAllRecords()
        {
            // Arrange
            IEnumerable<string> recordList = this._fixture.CreateMany<string>();
            string fileFullname = this.CreateTempFile(recordList);

            this._reader.BeginReadJob(fileFullname);
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

        private string CreateTempFile(IEnumerable<string> recordList = null)
        {
            string fileFullname = Path.GetTempFileName();

            if (recordList != null)
            {
                using (Stream stream = new FileStream(fileFullname, FileMode.Open, FileAccess.Write))
                using (StreamWriter streamWriter = new StreamWriter(stream))
                {
                    foreach (string record in recordList)
                    {
                        streamWriter.WriteLine(record);
                    }
                }
            }

            return fileFullname;
        }
    }
}
