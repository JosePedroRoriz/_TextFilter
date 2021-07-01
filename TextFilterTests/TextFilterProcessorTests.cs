using FluentAssertions;
using Moq;
using NUnit.Framework;
using Filter;
using TextFilter;
using System.IO.Abstractions.TestingHelpers;

namespace TextFilterTests
{
    [TestFixture]
    public class TextFilterProcessorTests
    {
        private const string FilePath = @"c:\FileName.txt";

        private const string OriginalText = "Alice was beginning to get very tired of sitting by her sister on the bank, ";

        private MockFileSystem FileSystemMock;

        private Mock<IFilter> LengthFilterMock;

        private Mock<IFilter> LetterFilterMock;

        private Mock<IFilter> VowelInMiddleOfWordFilterMock;

        private IFilter[] FiltersToBeApplied;

        private TextFilterProcessor Sut;

        [SetUp]
        public void Setup()
        {
            LengthFilterMock = new Mock<IFilter>();
            LetterFilterMock = new Mock<IFilter>();
            VowelInMiddleOfWordFilterMock = new Mock<IFilter>();

            FiltersToBeApplied = new IFilter[] { LengthFilterMock.Object, LetterFilterMock.Object, VowelInMiddleOfWordFilterMock.Object };

            FileSystemMock = new MockFileSystem();
            FileSystemMock.AddFile(FilePath, new MockFileData(OriginalText));

            Sut = new TextFilterProcessor(FiltersToBeApplied, FileSystemMock);
        }

        [TestCase(null)]
        [TestCase("FileName")]
        [TestCase("FileName.txt")]
        public void TextFilterProcessorShouldReturnEmptyOnInvalidFilePath(string invalidFilePath)
        {
            var result = Sut.ApplyTextFiltering(invalidFilePath);
            result.Should().Be(string.Empty);
        }

        [Test]
        // we are assuming the default values, which are: length < 3, chacterter to filter = 't'
        public void TextFilterProcessorShouldFilter()
        {
            var expected = "beginning,";

            LengthFilterMock.Setup(x => x.ApplyFilter(It.IsAny<string>())).Returns("Alice was beginning get very tired sitting her sister the bank,");
            LetterFilterMock.Setup(x => x.ApplyFilter(It.IsAny<string>())).Returns("Alice was beginning very her bank,");
            VowelInMiddleOfWordFilterMock.Setup(x => x.ApplyFilter(It.IsAny<string>())).Returns("beginning,");

            var result = Sut.ApplyTextFiltering(FilePath);
            result.Should().Be(expected);
        }
    }
}
