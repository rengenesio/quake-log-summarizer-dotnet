using System.Text.RegularExpressions;
using QuakeLogSummarizer.Core.GameEvents;

namespace QuakeLogSummarizer.Core.LogMessageParser
{
    public sealed class KillLogMessageParser : AbstractLogMessageParser
    {
        /// <inheritdoc />
        protected override string LogMessageFormat => "Kill: %i %i %i: %s killed %s by %s";

        protected override KillEvent BuildEvent(Match match)
        {
            return new KillEvent()
            {
                KillerId = int.Parse(match.Groups[1].ToString()),
                VictimId = int.Parse(match.Groups[2].ToString())
            };
        }
    }
}
