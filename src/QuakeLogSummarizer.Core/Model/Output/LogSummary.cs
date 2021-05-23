using System.Collections.Generic;

namespace QuakeLogSummarizer.Core.Model.Output
{
    public sealed class LogSummary : Dictionary<string, GameSummary>
    {
        public LogSummary(IEnumerable<Game> gameList)
        {
            foreach (Game game in gameList)
            {
                this.Add($"game_{game.Index}", new GameSummary(game));
            }
        }
    }
}
