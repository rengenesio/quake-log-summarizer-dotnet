using System.Text.RegularExpressions;
using NullGuard;
using QuakeLogSummarizer.Core.Extensions;
using QuakeLogSummarizer.Core.GameEvents;

namespace QuakeLogSummarizer.Core.LogMessageParser
{
    public sealed class InitGameLogMessageParser
    {
        private readonly Regex _messageFormatRegex;

        public InitGameLogMessageParser()
        {
            this._messageFormatRegex = "InitGame: %s".ToRegex();
        }

        [return: AllowNull]
        public InitGameEvent Parse(string logMessage)
        {
            InitGameEvent parsedEvent = null;

            Match match = this._messageFormatRegex.Match(logMessage);
            if (match.Success)
            {
                parsedEvent = new InitGameEvent();
            }

            return parsedEvent;
        }
    }
}
