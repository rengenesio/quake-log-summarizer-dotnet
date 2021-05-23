using System.Collections.Generic;
using QuakeLogSummarizer.Core.GameEvents;

namespace QuakeLogSummarizer.Core.Model
{
    public sealed class Game
    {
        public int Index { get; set; }

        public int KillCount { get; set; }

        public IDictionary<int, PlayerData> PlayerMap { get; set; }

        public Game()
        {
            this.PlayerMap = new Dictionary<int, PlayerData>();
        }
    }
}
