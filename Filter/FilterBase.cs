using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FilterServices
{
    public abstract class FilterBase
    {
        // avoid creating the same regex multiple times
        // we need a regex expression to find the whitespaces we currently have
        // \s+ will match any whitespace token where it appears once or more in the sequence
        private static readonly Regex CaptureWhiteSpaces = new Regex(@"\s+");

        protected string RemoveWordsFromText(string originalText, IEnumerable<Match> wordsToRemove)
        {
            var stringBuilder = new StringBuilder(originalText);

            // we reverse the words to remove so we match the correct index and avoid an out of range exception
            foreach (var match in wordsToRemove.Reverse())
            {
                stringBuilder.Remove(match.Index, match.Length);
            }

            var stringToClean = stringBuilder.ToString();

            // clean the extra whitespaces
            return CaptureWhiteSpaces.Replace(stringToClean, " ").Trim();
        }
    }
}
