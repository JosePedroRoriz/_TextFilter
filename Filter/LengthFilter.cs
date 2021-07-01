using System.Linq;
using System.Text.RegularExpressions;
using Filter;

namespace FilterServices
{
    public class LengthFilter : FilterBase, IFilter
    {
        // for this expression we just capture all the available words
        // we are also paying attention if those words have digits and an apostrophe such as you've, it's, we count them as 1 word
        // if the text is a quote such as 'this is a quote' we do not match the ' we are only after the words
        // that are being used for the quote
        private readonly Regex _captureAllWords = new Regex($@"\b[\w']+\b");

        private readonly int _lengthToFilterBy;

        public LengthFilter(int lengthToFilterBy)
        {
            _lengthToFilterBy = lengthToFilterBy;
        }

        public string ApplyFilter(string text)
        {
            // check if the text to filter is not only white spaces
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            // we are looking for all the words in the text that have a length that is smaller than
            // the length we are filtering by
            var wordsToRemove = _captureAllWords.Matches(text).Where(x => x.Value.Length < _lengthToFilterBy);

            // now we need to remove each of these matches from the original text 
            return RemoveWordsFromText(text, wordsToRemove);
        }
    }
}
