using System;
using System.IO;
using System.Threading.Tasks;
using QuakeLogSummarizer.Core.LogMessageParser;

namespace QuakeLogSummarizer.Core
{
    public sealed class LogSummarizer
    {
        private readonly ILogMessageExtractor _logMessageExtractor;
        private readonly InitGameLogMessageParser _initGameLogMessageParser;

        public LogSummarizer(ILogMessageExtractor logMessageExtractor, InitGameLogMessageParser initGameLogMessageParser)
        {
            this._logMessageExtractor = logMessageExtractor;
            this._initGameLogMessageParser = initGameLogMessageParser;
        }

        public async Task Summarize(string logFileFullname)
        {
            ulong gameCount = 0;

            using (Stream stream = new FileStream(logFileFullname, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(stream))
            {
                string logRecord;
                while ((logRecord = await reader.ReadLineAsync()) != null)
                {
                    string logMessage = this._logMessageExtractor.Extract(logRecord);

                    if(this._initGameLogMessageParser.Parse(logMessage) != null)
                    {
                        gameCount++;
                    }
                }
            }

            Console.WriteLine(gameCount);
        }
    }
}
