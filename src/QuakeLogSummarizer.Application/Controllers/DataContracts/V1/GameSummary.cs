using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using QuakeLogSummarizer.Core.Converters;
using QuakeLogSummarizer.Core.Model;

namespace QuakeLogSummarizer.Application.Controllers.DataContracts.V1
{
    public sealed class GameSummary
    {
        [JsonPropertyName("total_kills")]
        public int TotalKills { get; }

        [JsonPropertyName("players")]
        public IEnumerable<string> Players { get; }

        // NOTE: The serialized score on JSON output has a Dictionary<string, int> format (key is the player name and value is his final score).
        // NOTE: But more than one player may have the same name and a Dictionary<string, int> doesn't allow duplicated keys.
        // NOTE: So a key/value pair list with a custom JSON serializer is used instead.
        [JsonPropertyName("kills")]
        [JsonConverter(typeof(KeyValuePairListConverter))]
        public IEnumerable<(string PlayerName, int Kills)> Kills { get; }

        public GameSummary(Game game)
        {
            TotalKills = game.KillCount;
            Players = game.PlayerMap.Values.Select(p => p.PlayerNameList.Last());
            Kills = game.PlayerMap.Values.ToList()
                                .Select(p => (p.PlayerNameList.Last(), p.Score));
        }
    }
}
