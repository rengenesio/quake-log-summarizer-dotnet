using System.Collections.Generic;
using QuakeLogSummarizer.Core.GameEvents;

namespace QuakeLogSummarizer.Core.Model
{
    public sealed class Game
    {
        // TODO: Avoid expose this data as property. It allows another class to change it.
        public int Index { get; set; }

        // TODO: Avoid expose this data as property. It allows another class to change it.
        public int KillCount { get; set; }

        // TODO: Avoid expose this data as property. It allows another class to change it.
        public IDictionary<int, PlayerData> PlayerMap { get; set; }

        public Game()
        {
            this.PlayerMap = new Dictionary<int, PlayerData>();
        }
    }
}
