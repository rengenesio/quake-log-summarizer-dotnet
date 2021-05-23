using System.Collections.Generic;
using QuakeLogSummarizer.Core.GameEvents;

namespace QuakeLogSummarizer.Core.Model
{
    public sealed class PlayerData
    {
        public IList<string> PlayerNameList { get; set; }

        public int Score { get; set; }

        public PlayerData()
        {
            this.PlayerNameList = new List<string>();
        }
    }
}
