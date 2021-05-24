using System.Collections.Generic;

namespace QuakeLogSummarizer.Core.Model
{
    public sealed class PlayerData
    {
        // TODO: Avoid expose this data as property. It allows another class to change it.
        public IList<string> PlayerNameList { get; set; }

        // TODO: Avoid expose this data as property. It allows another class to change it.
        public int Score { get; set; }

        public PlayerData()
        {
            this.PlayerNameList = new List<string>();
        }
    }
}
