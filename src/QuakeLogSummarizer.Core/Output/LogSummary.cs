using System.Collections.Generic;
using QuakeLogSummarizer.Core.Model;

namespace QuakeLogSummarizer.Core.Output
{
    public sealed class LogSummary : Dictionary<string, object>
    {
        public LogSummary(IEnumerable<Game> gameList)
        {
            foreach(Game game in gameList)
            {
                this.Add($"game_{game.Index}", new { });
            }
        }
    }
}
