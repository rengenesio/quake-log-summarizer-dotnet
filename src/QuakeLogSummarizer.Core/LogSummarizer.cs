using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using QuakeLogSummarizer.Core.GameEvents;
using QuakeLogSummarizer.Core.LogMessageParser;
using QuakeLogSummarizer.Core.Model;
using QuakeLogSummarizer.Infrastructure;

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
            using ILogFileReader reader = scope.ServiceProvider.GetRequiredService<ILogFileReader>();

            IGameEventsProcessor gameEventsProcessor = scope.ServiceProvider.GetRequiredService<IGameEventsProcessor>();

            string logRecord;
            reader.BeginReadJob(logFileFullname);
            while ((logRecord = await reader.ReadLogRecord()) != null)
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
