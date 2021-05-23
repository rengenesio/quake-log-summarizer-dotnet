﻿using System;
using System.IO;
using System.Threading.Tasks;
using NullGuard;

namespace QuakeLogSummarizer.Infrastructure
{
    public sealed class LogFileReader : IDisposable
    {
        private Stream _stream;
        private StreamReader _streamReader;

        public void BeginReadJob(string logFileFullname)
        {
            this._stream = new FileStream(logFileFullname, FileMode.Open, FileAccess.Read);
            this._streamReader = new StreamReader(this._stream);
        }

        [return: AllowNull]
        public async Task<string> ReadLogRecord()
        {
            return await this._streamReader.ReadLineAsync();
        }

        public void Dispose()
        {
            this._streamReader.Dispose();
            this._stream.Dispose();
        }
    }
}
