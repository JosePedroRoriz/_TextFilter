using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Filter;

namespace FilterServices
{
    public class VowelInMiddleOfWordFilter : FilterBase, IFilter
    {
        // for this expression we just capture all the available words
        // we are also paying attention if those words have digits and an apostrophe such as you've, it's, we count them as 1 word
        // if the text is a quote such as 'this is a quote' we do not match the ' we are only after the words
        // that are being used for the quote
        private readonly Regex _captureAllWords = new Regex($@"\b[\w']+\b");

        private readonly List<char> _vowelList;

        public VowelInMiddleOfWordFilter()
        {
            _vowelList = new List<char>
            {
                'a','e','i','o','u',
                'A','E','I','O','U'
            };
        }

        public string ApplyFilter(string text)
        {
            // check if the text to filter is not only white spaces
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            var wordsToRemove = RemoveVowelInMiddle(_captureAllWords.Matches(text));

            // now we need to remove each of these matches from the original text 
            return RemoveWordsFromText(text, wordsToRemove);
        }

        private List<Match> RemoveVowelInMiddle(IEnumerable<Match> matches)
        {
            var wordsToRemove = new List<Match>();

            foreach (var match in matches)
            {
                // we ignore words that don't have a middle char       
                if (match.Value.Length < 3)
                {
                    continue;
                }

                // check if it's even or not            
                var offset = match.Value.Length % 2 == 0 ? 1 : 0;

                var middleCharacter = match.Value.Substring((match.Length / 2) - offset, offset + 1);

                // if it's an even number a vowel might be in the middle with a consoant or another vowel
                if (offset == 1)
                {
                    // check if any of the middle letters present are in the vowel list, for this we will intersect 
                    // both lists and check if there is a match 
                    if (_vowelList.Intersect(middleCharacter.ToCharArray()).Any())
                    {
                        wordsToRemove.Add(match);
                    }
                }
                // if it's an odd number check if the vowel is in the middle and filter it out
                // we need this check to avoid filtering out words such as "I"
                else
                {
                    // check that it is a char
                    if (char.TryParse(middleCharacter, out var character))
                    {
                        // return the result of matching the character with the vowel List
                        if (_vowelList.Contains(character))
                        {
                            wordsToRemove.Add(match);
                        }
                    }
                }
            }

            return wordsToRemove;
        }
    }
}
