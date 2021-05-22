using System.Text.RegularExpressions;
using QuakeLogSummarizer.Core.GameEvents;

namespace QuakeLogSummarizer.Core.LogMessageParser
{
    public sealed class ClientConnectLogMessageParser : AbstractLogMessageParser
    {
        protected override string LogMessageFormat => "ClientConnect: %i";

        protected override ClientConnectEvent BuildEvent(Match match)
        {
            return new ClientConnectEvent()
            {
                PlayerId = int.Parse(match.Groups[1].ToString())
            };
        }
    }
}
