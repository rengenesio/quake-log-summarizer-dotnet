using System.Collections.Generic;
using QuakeLogSummarizer.Core.Model;

namespace QuakeLogSummarizer.Application.Controllers.DataContracts.V1
{
    public sealed class LogSummary : Dictionary<string, GameSummary>
    {
        public LogSummary(IEnumerable<Game> gameList)
        {
            foreach (Game game in gameList)
            {
                Add($"game_{game.Index}", new GameSummary(game));
            }
        }
    }
}
