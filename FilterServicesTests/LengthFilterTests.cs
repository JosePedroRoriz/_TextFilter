using FluentAssertions;
using NUnit.Framework;
using FilterServices;

namespace FilterServicesTests
{
    [TestFixture]
    public class LengthFilterTests
    {
        private const string OriginalText = "Alice was beginning to get very tired of sitting by her sister on the bank,";

        [Test]
        public void LengthFilterShouldReturnEmptyStringOnWhiteSpaceText()
        {
            var Sut = new LengthFilter(3);
            var result = Sut.ApplyFilter("       ");
            result.Should().Be(string.Empty);
        }

        [Test]
        public void LengthFilterShouldReturnEmptyStringOnNullText()
        {
            var Sut = new LengthFilter(3);
            var result = Sut.ApplyFilter(null);
            result.Should().Be(string.Empty);
        }

        [Test]
        public void LengthFilterShouldReturnEmptyStringOnEmptyString()
        {
            var Sut = new LengthFilter(3);
            var result = Sut.ApplyFilter(string.Empty);
            result.Should().Be(string.Empty);
        }

        [TestCase(3, OriginalText, "Alice was beginning get very tired sitting her sister the bank,")]
        [TestCase(4, OriginalText, "Alice beginning very tired sitting sister bank,")]
        [TestCase(5, OriginalText, "Alice beginning tired sitting sister ,")]
        public void LengthFilterShouldFilterText(int length, string textToFilter, string expectedResult)
        {
            var Sut = new LengthFilter(length);
            var result = Sut.ApplyFilter(textToFilter);
            result.Should().Be(expectedResult);
        }

        [TestCase(5, "Hi, I like it's and 'don't do thingsZ' you've seen a lot", ", 'don't thingsZ' you've")]
        [TestCase(6, "'Hi, I like it's and don't do thingsZ you've seen a lot'", "', thingsZ you've '")]
        [TestCase(7, "Hi, 'I like it's and don't' do thingsZ you've seen a lot", ", ' ' thingsZ")]
        public void LengthFilterShouldFilterTextAndIgnoreApostrophesNotPartOfWords(int length, string textToFilter, string expectedResult)
        {
            var Sut = new LengthFilter(length);
            var result = Sut.ApplyFilter(textToFilter);
            result.Should().Be(expectedResult);
        }

        [TestCase(5, "Hi, I like it's and don't do thingsZ you've seen a lot", ", don't thingsZ you've")]
        [TestCase(6, "Hi, I like it's and don't do thingsZ you've seen a lot", ", thingsZ you've")]
        [TestCase(7, "Hi, I like it's and don't do thingsZ you've seen a lot", ", thingsZ")]
        public void LengthShouldFilterWordsWithApostrophes(int length, string textToFilter, string expectedResult)
        {
            var Sut = new LengthFilter(length);
            var result = Sut.ApplyFilter(textToFilter);
            result.Should().Be(expectedResult);
        }
    }
}
