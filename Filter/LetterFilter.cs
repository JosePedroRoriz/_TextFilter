using System.Text.RegularExpressions;
using FilterServices;

namespace Filter
{
    public class LetterFilter : FilterBase, IFilter
    {
        private readonly char _letterToFilterWordBy;

        private readonly Regex _captureAllWordsWithDesiredLetter;

        public LetterFilter(char letterToFilterWordBy)
        {
            _letterToFilterWordBy = letterToFilterWordBy;

            // \b[\w']*
            // we start to look into the start of each individual word in the string, while paying attention to
            // include words with apostrophes and digits
            // [{ char.ToLower(letter_to_filter) }{ char.ToUpper(letter_to_filter) }]]
            // here we start looking for words that have the character we want
            // to filter out we also have to pay attention to upper and lower case
            // we are not done yet becuase now for each word we "stoped matching" once we reach the letter we are trying to filter by
            // [\w']*\b
            // with this bit we match untill the end of the word that has the desired letter to filter by, paying once
            // again attention to apostrophes and digits

            _captureAllWordsWithDesiredLetter = new Regex($@"\b[\w']*[{ char.ToLower(_letterToFilterWordBy) }{ char.ToUpper(_letterToFilterWordBy) }][\w']*\b");
        }
       
        public string ApplyFilter(string text)
        {
            //check if the text to filter is not only white spaces
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            var wordsToRemove = _captureAllWordsWithDesiredLetter.Matches(text);

            // now we need to remove each of these matches from the original text 
            return RemoveWordsFromText(text, wordsToRemove);
        }
    }
}
