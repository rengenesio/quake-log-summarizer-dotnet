using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace QuakeLogSummarizer.Core.Extensions
{
    public static class RegexExtensions
    {
        /// <summary>
        /// Contains pairs of a format specifier and an equivalent pattern matching regex.
        /// </summary>
        public static readonly IReadOnlyList<(string FormatSpecifier, string RegexPattern)> FormatReplacementList = new List<(string, string)>()
        {
            // Be aware of using non-greedy regex.
            ("%i", @"(-?\d+?)"),
            ("%s", @"(.*?)")
        };

        /// <summary>
        /// Converts a string containing format specifiers to an equivalent pattern matching regex.
        /// Useful to generate regex to extract data from a formatted string.
        /// Refer to <see cref="FormatReplacementList"/> to get a list of supported format specifiers.
        /// </summary>
        /// <param name="format">String containing format specifiers to be converted.</param>
        /// <returns>Equivalent pattern matching regex.</returns>
        /// <remarks>Input format reference: 'https://www.cplusplus.com/reference/cstdio/printf/'</remarks>
        public static Regex ToRegex(this string format)
        {
            foreach((string formatSpecifier, string regexPattern) in RegexExtensions.FormatReplacementList)
            {
                format = format.Replace(formatSpecifier, regexPattern);
            }

            return new Regex($"^{format}$", RegexOptions.Compiled);
        }
    }
}
