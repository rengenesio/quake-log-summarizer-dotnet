using System;
using System.Threading.Tasks;

namespace QuakeLogSummarizer.Infrastructure
{
    public interface ILogFileReader : IDisposable
    {
        void BeginReadJob(string logFileFullname);
        
        Task<string> ReadLogRecord();

        void Dispose();
    }
}