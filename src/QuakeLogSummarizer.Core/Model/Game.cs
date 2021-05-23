using System.Collections.Generic;
using QuakeLogSummarizer.Core.GameEvents;

namespace QuakeLogSummarizer.Core.Model
{
    public sealed class Game
    {
        public int Index { get; set; }

        public int KillCount { get; set; }

        public IDictionary<int, IList<string>> PlayerMap { get; set; }

        public Game()
        {
            this.PlayerMap = new Dictionary<int, IList<string>>();
        }
    }
}
