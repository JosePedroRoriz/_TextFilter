using FluentAssertions;
using NUnit.Framework;
using FilterServices;

namespace FilterServicesTests
{
    [TestFixture]
    public class VowelInMiddleOfWordFilterTests
    {
        private const string OriginalText = "Alice was beginning to get very tired of sitting by her sister on the bank,";

        [Test]
        public void VowelInMiddleOfWordFilterShouldReturnEmptyStringOnWhiteSpaceText()
        {
            var Sut = new VowelInMiddleOfWordFilter();
            var result = Sut.ApplyFilter("       ");
            result.Should().Be(string.Empty);
        }

        [Test]
        public void VowelInMiddleOfWordFilterShouldReturnEmptyStringOnNullText()
        {
            var Sut = new VowelInMiddleOfWordFilter();
            var result = Sut.ApplyFilter(null);
            result.Should().Be(string.Empty);
        }

        [Test]
        public void VowelInMiddleOfWordFilterShouldReturnEmptyStringOnEmptyString()
        {
            var Sut = new VowelInMiddleOfWordFilter();
            var result = Sut.ApplyFilter(string.Empty);
            result.Should().Be(string.Empty);
        }

        [TestCase(OriginalText, "beginning to tired of sitting by sister on the ,")]
        [TestCase("ALICE WAS BEGINNING TO GET VERY TIRED OF SITTING BY HER SISTER ON THE BANK,", "BEGINNING TO TIRED OF SITTING BY SISTER ON THE ,")]
        [TestCase("ALICE was BEGINNING TO GET VERY TIRED of SITTING BY HER SISTER ON the BANK,", "BEGINNING TO TIRED of SITTING BY SISTER ON the ,")]
        public void VowelInMiddleOfWordFilterShouldFilterText(string textToFilter, string expectedResult)
        {
            var Sut = new VowelInMiddleOfWordFilter();
            var result = Sut.ApplyFilter(textToFilter);
            result.Should().Be(expectedResult);
        }

        [TestCase("'I hope you've eaten because dinner won't be ready for hours'", "'I eaten dinner won't be '")]
        [TestCase("I hope 'you've eaten' because dinner won't be ready for hours'", "I ' eaten' dinner won't be '")]
        [TestCase("I hope yoU've eaten because dinner won't be ready for hours", "I eaten dinner won't be")]
        [TestCase("I hope 'yoU've eaten' because dinner won't be ready for hours'", "I ' eaten' dinner won't be '")]
        [TestCase("'You're looking very smart today.'", "' looking today.'")]
        [TestCase("You're 'looking very smart' today.'", "'looking ' today.'")]
        [TestCase("YoU're looking very smart today.", "looking today.")]
        [TestCase("YoU're 'looking very smart' today.'", "'looking ' today.'")]
        public void VowelInMiddleOfWordFilterShouldFilterTextAndIgnoreApostrophesNotPartOfWords(string textToFilter, string expectedResult)
        {
            var Sut = new VowelInMiddleOfWordFilter();
            var result = Sut.ApplyFilter(textToFilter);
            result.Should().Be(expectedResult);
        }

        [TestCase("I hope you've eaten because dinner won't be ready for hours", "I eaten dinner won't be")]
        [TestCase("I hope yoU've eaten because dinner won't be ready for hours", "I eaten dinner won't be")]
        [TestCase("You're looking very smart today.", "looking today.")]
        [TestCase("YoU're looking very smart today.", "looking today.")]
        public void VowelInMiddleOfWordFilterShouldFilterTextWithApostrophes(string textToFilter, string expectedResult)
        {
            var Sut = new VowelInMiddleOfWordFilter();
            var result = Sut.ApplyFilter(textToFilter);
            result.Should().Be(expectedResult);
        }
    }
}
