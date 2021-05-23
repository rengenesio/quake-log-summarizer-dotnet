using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using QuakeLogSummarizer.Core.GameEvents;
using QuakeLogSummarizer.Core.LogMessageParser;
using QuakeLogSummarizer.Core.Model;

namespace QuakeLogSummarizer.Core
{
    public sealed class LogSummarizer
    {
        private readonly ILogMessageExtractor _logMessageExtractor;
        private readonly IEnumerable<ILogMessageParser> _logMessageParserList;

        public LogSummarizer(ILogMessageExtractor logMessageExtractor, IEnumerable<ILogMessageParser> logMessageParserList)
        {
            this._logMessageExtractor = logMessageExtractor;
            this._logMessageParserList = logMessageParserList;
        }

        public async Task<IEnumerable<Game>> Summarize(string logFileFullname)
        {
            GameEventsProcessor gameEventsProcessor = new GameEventsProcessor();

            using (Stream stream = new FileStream(logFileFullname, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(stream))
            {
                string logRecord;
                while ((logRecord = await reader.ReadLineAsync()) != null)
                {
                    string logMessage = this._logMessageExtractor.Extract(logRecord);

                    if (logMessage == null)
                    {
                        continue;
                    }

                    IGameEvent gameEvent = this._logMessageParserList.Select(p => p.Parse(logMessage))
                        .SingleOrDefault(e => e != null);

                    if (gameEvent != null)
                    {
                        gameEventsProcessor.Process(gameEvent);
                    }
                }
            }

            return gameEventsProcessor.GameList;
        }
    }
}
