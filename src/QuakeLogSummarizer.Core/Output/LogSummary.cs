using System.Collections.Generic;
using System.Linq;
using QuakeLogSummarizer.Core.Model;

namespace QuakeLogSummarizer.Core.Output
{
    public sealed class LogSummary : Dictionary<string, dynamic>
    {
        public LogSummary(IEnumerable<Game> gameList)
        {
            foreach (Game game in gameList)
            {
                this.Add($"game_{game.Index}", new
                {
                    total_kills = game.KillCount,
                    players = game.PlayerMap.Values.Select(p => p.First())
                });
            }
        }
    }
}
