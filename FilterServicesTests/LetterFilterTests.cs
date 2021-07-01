using FluentAssertions;
using NUnit.Framework;
using Filter;

namespace FilterServicesTests
{
    [TestFixture]
    public class LetterFilterTests
    {
        private const string OriginalText = "Alice was beginning to get very tired of sitting by her sister on the bank,";

        [Test]
        public void LetterFilterShouldReturnEmptyStringOnWhiteSpaceText()
        {
            var Sut = new LetterFilter('t');
            var result = Sut.ApplyFilter("       ");
            result.Should().Be(string.Empty);
        }

        [Test]
        public void LetterFilterShouldReturnEmptyStringOnNullText()
        {
            var Sut = new LetterFilter('t');
            var result = Sut.ApplyFilter(null);
            result.Should().Be(string.Empty);
        }

        [Test]
        public void LetterFilterShouldReturnEmptyStringOnEmptyString()
        {
            var Sut = new LetterFilter('t');
            var result = Sut.ApplyFilter(string.Empty);
            result.Should().Be(string.Empty);
        }

        [TestCase('t', OriginalText, "Alice was beginning very of by her on bank,")]
        [TestCase('A', OriginalText, "beginning to get very tired of sitting by her sister on the ,")]
        [TestCase('s', "Alice was beginning to get very tired of sitting by her sister on the bank,", "Alice beginning to get very tired of by her on the bank,")]
        [TestCase('s', "Alice was beginning to get very tired of SITTING by her SISTER on the bank,", "Alice beginning to get very tired of by her on the bank,")]
        [TestCase('s', "Alice was beginning to get very tired of Sitting by her sister on the bank,", "Alice beginning to get very tired of by her on the bank,")]
        [TestCase('S', "Alice was beginning to get very tired of sitting by her sister on the bank,", "Alice beginning to get very tired of by her on the bank,")]
        [TestCase('S', "Alice was beginning to get very tired of SITTING by her SISTER on the bank,", "Alice beginning to get very tired of by her on the bank,")]
        [TestCase('S', "Alice was beginning to get very tired of Sitting by her Sister on the bank,", "Alice beginning to get very tired of by her on the bank,")]
        public void LetterFilterShouldFilterText(char letterToFilterBy, string textToFilter, string expectedResult)
        {
            var Sut = new LetterFilter(letterToFilterBy);
            var result = Sut.ApplyFilter(textToFilter);
            result.Should().Be(expectedResult);
        }

        [TestCase('t', "'Hi, I like it's and don't do things you've seen a lot'", "'Hi, I like and do you've seen a '")]
        [TestCase('t', "'Hi, I like iT's and don't do things you've seen a loT'", "'Hi, I like and do you've seen a '")]
        [TestCase('t', "Hi, I like 'it's' and 'don't do things' you've seen a lot", "Hi, I like '' and ' do ' you've seen a")]
        [TestCase('t', "Hi, I like 'iT's' and 'don't do things' you've seen a loT", "Hi, I like '' and ' do ' you've seen a")]
        public void LetterFilterShouldFilterTextAndIgnoreApostrophesNotPartOfWords(char letterToFilterBy, string textToFilter, string expectedResult)
        {
            var Sut = new LetterFilter(letterToFilterBy);
            var result = Sut.ApplyFilter(textToFilter);
            result.Should().Be(expectedResult);
        }

        [TestCase('t', "Hi, I like it's and don't do things you've seen a lot", "Hi, I like and do you've seen a")]
        [TestCase('t', "Hi, I like iT's and don't do things you've seen a loT", "Hi, I like and do you've seen a")]
        [TestCase('T', "Hi, I like it's and don't do things you've seen a lot", "Hi, I like and do you've seen a")]
        [TestCase('T', "Hi, I like iT's and don'T do things you've seen a loT", "Hi, I like and do you've seen a")]
        [TestCase('s', "Hi, I like it's and don't do things you've seen a lot", "Hi, I like and don't do you've a lot")]
        [TestCase('s', "Hi, I like it'S and don't do thingS you've Seen a lot", "Hi, I like and don't do you've a lot")]
        [TestCase('S', "Hi, I like it's and don't do things you've seen a lot", "Hi, I like and don't do you've a lot")]
        [TestCase('S', "Hi, I like it'S and don't do thingS you've Seen a lot", "Hi, I like and don't do you've a lot")]
        public void LetterFilterShouldFilterTextWithApostrophes(char letterToFilterBy, string textToFilter, string expectedResult)
        {
            var Sut = new LetterFilter(letterToFilterBy);
            var result = Sut.ApplyFilter(textToFilter);
            result.Should().Be(expectedResult);
        }
    }
}
