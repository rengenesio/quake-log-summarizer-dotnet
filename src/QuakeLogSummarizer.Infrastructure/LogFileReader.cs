using System;
using System.IO;
using System.Threading.Tasks;
using NullGuard;

namespace QuakeLogSummarizer.Infrastructure
{
    public sealed class LogFileReader : ILogFileReader
    {
        private StreamReader _streamReader;

        public void BeginReadJob(Stream fileStream)
        {
            this._streamReader = new StreamReader(fileStream);
        }

        [return: AllowNull]
        public async Task<string> ReadLogRecordAsync()
        {
            if(this._streamReader == null)
            {
                throw new InvalidOperationException("Stream is not initialized. Method 'BeginReadJob' should be executed before read log records.");
            }

            return await this._streamReader.ReadLineAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            this._streamReader.Dispose();
        }
    }
}
