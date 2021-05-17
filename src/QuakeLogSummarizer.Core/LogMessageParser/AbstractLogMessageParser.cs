using System;
using System.Text.RegularExpressions;
using NullGuard;
using QuakeLogSummarizer.Core.Extensions;

namespace QuakeLogSummarizer.Core.LogMessageParser
{
    public abstract class AbstractLogMessageParser<TEvent>
    {
        private static Regex MessageFormatRegex;

        protected abstract string LogMessageFormat { get; }

        public AbstractLogMessageParser()
        {
            MessageFormatRegex ??= this.LogMessageFormat.ToRegex();
        }

        [return: AllowNull]
        public TEvent Parse(string logMessage)
        {
            TEvent parsedEvent = default(TEvent);

            Match match = MessageFormatRegex.Match(logMessage);
            if (match.Success)
            {
                parsedEvent = this.BuildEvent(match);
            }

            return parsedEvent;
        }

        protected abstract TEvent BuildEvent(Match match);
    }
}
