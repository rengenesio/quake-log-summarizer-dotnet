﻿using System.Text.RegularExpressions;
using QuakeLogSummarizer.Core.GameEvents;

namespace QuakeLogSummarizer.Core.LogMessageParser
{
    public sealed class InitGameLogMessageParser : AbstractLogMessageParser
    {
        protected override string LogMessageFormat => "InitGame: %s";

        protected override InitGameEvent BuildEvent(Match match)
        {
            return new InitGameEvent();
        }
    }
}