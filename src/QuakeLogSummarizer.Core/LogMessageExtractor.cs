using System.Text.RegularExpressions;

namespace QuakeLogSummarizer.Core
{
    /// <summary>
    /// Logged messages are joined with a time prefix to be recorded to the log file.
    /// This prefix format specifier is: '%3i:%i%i '. This class removes the time prefix from a log record.
    /// </summary>
    /// <remarks>
    /// Reference: 'G_LogPrintf' function at https://github.com/id-Software/Quake-III-Arena/blob/master/code/game/g_main.c
    /// </remarks>
    public sealed class LogMessageExtractor
    {
        // Extracts a message from the record using the following regex:
        //     - '^(?=.{3}:)' - positive lookahead to ensure the input starts with 3 characters before the first colon
        //     - '( *\d+:)' - the first 3 characters should be zero or more whitespaces followed by at least one digit (the minutes portion of time)
        //     - '\d{2} ' - two digits and a whitespace after the first colon (the seconds portion of time and a separator after the logged time)
        //     - '([(\[a-zA-Z\]+:)(\-+)].*)' - captures everything after the time separator (should be only letters (at least one) followed by a colon; or a hyphen sequence)
        // NOTE: This regex was based on all arguments used on 'G_LogPrintf' functions (from Quake III source code 'https://github.com/id-Software/Quake-III-Arena')
        private readonly Regex _extractMessageDataRegex = new Regex(@"^(?=.{3}:)( *\d+:)\d{2} ([(\[a-zA-Z\]+:)(\-+)].*)");

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

            return match.Groups[2].ToString();
        }
    }
}
