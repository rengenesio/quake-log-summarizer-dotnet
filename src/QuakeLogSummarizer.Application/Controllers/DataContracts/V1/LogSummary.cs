using System.Collections.Generic;
using QuakeLogSummarizer.Core.Model;

namespace QuakeLogSummarizer.Application.Controllers.DataContracts.V1
{
    /// <summary>
    /// A summary for all games contained in a log file.
    /// </summary>
    public sealed class LogSummary : Dictionary<string, GameSummary>
    {
        /// <summary />
        public LogSummary(IEnumerable<Game> gameList)
        {
            foreach (Game game in gameList)
            {
                Add($"game_{game.Index}", new GameSummary(game));
            }
        }
    }
}
