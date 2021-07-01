using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Text;
using Filter;

namespace TextFilter
{
    public class TextFilterProcessor
    {
        private readonly IEnumerable<IFilter> _filters;
        private readonly IFileSystem _fileSystem;

        public TextFilterProcessor(IEnumerable<IFilter> filters, IFileSystem fileSystem)
        {
            _filters = filters;
            _fileSystem = fileSystem;
        }

        public string ApplyTextFiltering(string filePath)
        {
            var stringBuilder = new StringBuilder();

            // check if the file exists, if it does not just return an empty string
            if (!_fileSystem.File.Exists(filePath))
            {
                return string.Empty;
            }

            using (var streamreader = new StreamReader(_fileSystem.FileStream.Create(filePath, FileMode.Open)))
            {
                while (streamreader.Peek() >= 0)
                {
                    var line = streamreader.ReadLine();

                    // apply the filters for each line
                    foreach (var filter in _filters)
                    {
                        line = filter.ApplyFilter(line);
                    }

                    // add a whitespace to avoid having words being put together
                    stringBuilder.Append(line).Append(' ');
                }
            }

            // trim any spaces that may left
            return stringBuilder.ToString().Trim();
        }
    }
}
