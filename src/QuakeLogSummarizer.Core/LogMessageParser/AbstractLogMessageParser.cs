using System.Text.RegularExpressions;
using NullGuard;
using QuakeLogSummarizer.Core.Extensions;
using QuakeLogSummarizer.Core.GameEvents;

namespace QuakeLogSummarizer.Core.LogMessageParser
{
    public abstract class AbstractLogMessageParser : ILogMessageParser
    {
        private readonly Regex _messageFormatRegex;

        protected abstract string LogMessageFormat { get; }

        public AbstractLogMessageParser()
        {
            this._messageFormatRegex = this.LogMessageFormat.ToRegex();
        }

        [return: AllowNull]
        public IGameEvent Parse(string logMessage)
        {
            IGameEvent parsedEvent = null;

            Match match = this._messageFormatRegex.Match(logMessage);
            if (match.Success)
            {
                parsedEvent = this.BuildEvent(match);
            }

            return parsedEvent;
        }

        protected abstract IGameEvent BuildEvent(Match match);
    }
}
