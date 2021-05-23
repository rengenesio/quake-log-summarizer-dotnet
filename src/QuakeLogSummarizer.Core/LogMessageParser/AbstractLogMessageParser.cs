using System.Text.RegularExpressions;
using NullGuard;
using QuakeLogSummarizer.Core.Extensions;
using QuakeLogSummarizer.Core.GameEvents;

namespace QuakeLogSummarizer.Core.LogMessageParser
{
    public abstract class AbstractLogMessageParser : ILogMessageParser
    {
        private readonly Regex _messageFormatRegex;

        /// <summary>
        /// The format string used as argument on G_LogPrintf functions.
        /// Refer to 'G_LogPrintf' function references at Quake III source code https://github.com/id-Software/Quake-III-Arena
        /// </summary>
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

        /// <summary>
        /// Builds an <see cref="IGameEvent"/> based on matches found using a regex based on <see cref="LogMessageFormat"/>.
        /// </summary>
        /// <param name="match">Regex match.</param>
        /// <returns>A concrete event containing information parsed from log message.</returns>
        protected abstract IGameEvent BuildEvent(Match match);
    }
}
