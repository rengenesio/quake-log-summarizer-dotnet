using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using QuakeLogSummarizer.Core.Converters;
using QuakeLogSummarizer.Core.Model;

namespace QuakeLogSummarizer.Application.Controllers.DataContracts.V1
{
    /// <summary>
    /// A summary for a single game.
    /// </summary>
    public sealed class GameSummary
    {
        /// <summary>
        /// Total kills in the game (suicides and world kills are included).
        /// </summary>
        [JsonPropertyName("total_kills")]
        public int TotalKills { get; }

        /// <summary>
        /// List of players that joined the match (the last player's name in a game).
        /// </summary>
        [JsonPropertyName("players")]
        public IEnumerable<string> Players { get; }

        // NOTE: The serialized score on JSON output has a Dictionary<string, int> format (key is the player name and value is his final score).
        // NOTE: But more than one player may have the same name and a Dictionary<string, int> doesn't allow duplicated keys.
        // NOTE: So a key/value pair list with a custom JSON serializer is used instead.
        /// <summary>
        /// The final players score. This object is indexed by the last player's name in a game.
        /// </summary>
        [JsonPropertyName("kills")]
        [JsonConverter(typeof(KeyValuePairListConverter))]
        public IEnumerable<(string PlayerName, int Kills)> Kills { get; }

        /// <summary />
        public GameSummary(Game game)
        {
            TotalKills = game.KillCount;
            Players = game.PlayerMap.Values.Select(p => p.PlayerNameList.Last());
            Kills = game.PlayerMap.Values.ToList()
                                .Select(p => (p.PlayerNameList.Last(), p.Score));
        }
    }
}
