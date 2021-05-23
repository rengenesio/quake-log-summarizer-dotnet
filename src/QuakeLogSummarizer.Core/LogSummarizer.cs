using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuakeLogSummarizer.Core.GameEvents;
using QuakeLogSummarizer.Core.LogMessageParser;
using QuakeLogSummarizer.Core.Model;

namespace QuakeLogSummarizer.Core
{
    public sealed class LogSummarizer
    {
        private readonly ILogMessageExtractor _logMessageExtractor;
        private readonly IEnumerable<ILogMessageParser> _logMessageParserList;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public LogSummarizer(ILogMessageExtractor logMessageExtractor, IEnumerable<ILogMessageParser> logMessageParserList, IServiceScopeFactory serviceScopeFactory)
        {
            this._logMessageExtractor = logMessageExtractor;
            this._logMessageParserList = logMessageParserList;
            this._serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<IEnumerable<Game>> Summarize(string logFileFullname)
        {
            using IServiceScope scope = this._serviceScopeFactory.CreateScope();
            using Stream stream = new FileStream(logFileFullname, FileMode.Open, FileAccess.Read);
            using StreamReader reader = new StreamReader(stream);

            GameEventsProcessor gameEventsProcessor = scope.ServiceProvider.GetRequiredService<GameEventsProcessor>();

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

            return gameEventsProcessor.GameList;
        }
    }
}
