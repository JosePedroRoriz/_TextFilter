using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Filter;
using FilterServices;
using System.IO.Abstractions;

namespace TextFilter
{
    class Program
    {
        private const char LetterToFilterBy = 't';
        private const int LengthToFilterBy = 3;

        static void Main(string[] args)
        {
            const string FileToFilter = "SampleTextFile.txt";

            // create the host and setup DI
            var host = CreateHost();

            // get the file path to start filtering
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), FileToFilter);

            var textFilterProcessor = ActivatorUtilities.CreateInstance<TextFilterProcessor>(host.Services);

            Console.WriteLine($"Starting filtering for the file {FileToFilter}");
            var filterResult = textFilterProcessor.ApplyTextFiltering(filePath);

            Console.WriteLine("Finished Processing!");
            Console.WriteLine("Result:");
            Console.WriteLine($"{filterResult}");
        }

        private static IHost CreateHost()
        {
            return Host.CreateDefaultBuilder()
                 .ConfigureServices((context, services) =>
                 {
                     services.AddSingleton<IFilter, VowelInMiddleOfWordFilter>();
                     services.AddSingleton<IFilter, LengthFilter>(x => new LengthFilter(LengthToFilterBy));
                     services.AddSingleton<IFilter, LetterFilter>(x => new LetterFilter(LetterToFilterBy));

                     services.AddTransient<IFileSystem, FileSystem>();
                 })
                 .Build();
        }
    }
}
