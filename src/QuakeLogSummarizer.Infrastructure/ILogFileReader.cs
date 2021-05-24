using System;
using System.IO;
using System.Threading.Tasks;

namespace QuakeLogSummarizer.Infrastructure
{
    public interface ILogFileReader : IDisposable
    {
        void BeginReadJob(Stream fileStream);
        
        Task<string> ReadLogRecordAsync();
    }
}