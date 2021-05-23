using System.Text.RegularExpressions;
using QuakeLogSummarizer.Core.Extensions;
using QuakeLogSummarizer.Core.GameEvents;

namespace QuakeLogSummarizer.Core.LogMessageParser
{
    public sealed class ClientUserInfoChangedLogMessageParser : AbstractLogMessageParser
    {
        private readonly Regex _extractPlayerNameRegex = "n\\\\%s\\\\t".ToRegex(appendEndOfLine: false);

        /// <inheritdoc />
        protected override string LogMessageFormat => "ClientUserinfoChanged: %i %s";

        protected override ClientUserInfoChangedEvent BuildEvent(Match match)
        {
            string playerInfo = match.Groups[2].ToString();
            string playerName = this._extractPlayerNameRegex.Match(playerInfo).Groups[1].ToString();

            return new ClientUserInfoChangedEvent()
            {
                PlayerId = int.Parse(match.Groups[1].ToString()),
                PlayerName = playerName
            };
        }
    }
}
