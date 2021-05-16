using System.Text.RegularExpressions;

namespace QuakeLogSummarizer.Core
{
    /// <summary>
    /// Logged messages are joined with a time prefix to be recorded to the log file.
    /// This prefix format specifier is: '%3i:%i%i '. This class removes the time prefix from a log record.
    /// Log records may be truncated in log file (e.g: due to a server crash). Since a truncated record doesn't contain a line ending,
    /// a line read from log file may contain two (or more) records. The truncated records will be ignored
    /// </summary>
    /// <remarks>
    /// Reference: 'G_LogPrintf' function at Quake III source code https://github.com/id-Software/Quake-III-Arena/blob/master/code/game/g_main.c
    /// </remarks>
    public sealed class LogMessageExtractor : ILogMessageExtractor
    {
        // Extracts a message from the record using the following regex:
        //   - '^.*' - greedy match to anything at the input start (removes truncated records)
        //   - '(?=.{3}:)' - positive lookahead to ensure the input starts with 3 characters before the first colon
        //   - '(?: *\d+:)' - the first 3 characters should be zero or more whitespaces followed by at least one digit (the minutes portion of time)
        //   - '\d{2} ' - two digits and a whitespace after the first colon (the seconds portion of time and a separator after the logged time)
        //   - '((?:[a-zA-Z]+:.*)|(?:-+))' - captures everything after the time separator (should be only letters (at least one) followed by a colon; or a hyphen sequence)
        //     - This portion was based on all arguments used on 'G_LogPrintf' functions (from Quake III source code):
        //       "------------------------------------------------------------"
        //       "ClientUserinfoChanged: %i %s"
        //       "ClientConnect: %i"
        //       "ClientBegin: %i"
        //       "ClientDisconnect: %i"
        //       "Exit: %s"
        //       "InitGame: %s"
        //       "Item: %i %s"
        //       "Kill: %i %i %i: %s killed %s by %s"
        //       "ShutdownGame:"
        //       "Warmup:"
        //       "red:%i blue:%i"
        //       "say: %s: %s"
        //       "sayteam: %s: %s"
        //       "score: %i ping: %i client: %i %s"
        //       "tell: %s to %s: %s"
        //       "vtell: %s to %s: %s"
        private readonly Regex _extractMessageDataRegex = new Regex(@"^.*(?=.{3}:)(?: *\d+:)\d{2} ((?:[a-zA-Z]+:.*)|(?:-+))");

        /// <returns>
        /// The logged message data without the time prefix or 'null' if <paramref name="logRecord"/> doesn't match the expected format.
        /// </returns>
        public string Extract(string logRecord)
        {
            Match match = this._extractMessageDataRegex.Match(logRecord);
            if (match?.Success == false)
            {
                return null;
            }

            return match.Groups[1].ToString();
        }
    }
}
