using System;
using System.Collections.Generic;
using System.Linq;
using LetterCount.Logic.Downloaders;
using LetterCount.Logic.Interfaces;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable InconsistentNaming

namespace LetterCount.Logic.Test
{
    public class LetterCounter_LargeListOfUrl_PerformanceTest
    {
        private LetterCounter _letterCounter;
        private readonly IWriteLineOutputter _writeLineOutputter;

        private readonly string[] _urls;
        private IDownloader[] _downloaders;
        private readonly IWebClientFactory _webClientFactory;
        private IEnumerable<WordCounterResult> _wordCounterResults;

        public LetterCounter_LargeListOfUrl_PerformanceTest(ITestOutputHelper testOutputHelper)
        {
            var urls = new List<string>();
            for (var i = 0; i < 200; ++i)
            {
                urls.Add("http://www.cnn.com");
            }
            _urls = urls.ToArray();

            var urlSettings = new[]
            {
                new WebClientStub.UrlSetting("cnn", 500)
            };
            _webClientFactory = new WebClientFactory(urlSettings);      
            _writeLineOutputter = new WriteLineOutputter(testOutputHelper);
        }

        [Theory]
        [Trait("Category", "LongRunning")]
        [DownloadersDataAttribute]
        public void Should_Run_Peformances(IEnumerable<Type> downloaderNames)
        {
            GivenDownloaders(downloaderNames);
            GivenWordCounter(false);
            WhenWordCounterRuns();
            DisplayResult();
        }
        
        [Fact]
        public void Should_RunTaskRun_Threads()
        {
            _downloaders = new IDownloader[]
            {
                new TaskRunDownloader(_webClientFactory, _writeLineOutputter),
            };
            GivenWordCounter(true);
            WhenWordCounterRuns();
        }
        
        [Fact]
        public void Should_RunAsync_Threads()
        {
            _downloaders = new IDownloader[]
            {
                new AsyncDownloader(_webClientFactory, _writeLineOutputter),
            };
            GivenWordCounter(true);
            WhenWordCounterRuns();
        }

        public void GivenDownloaders(IEnumerable<Type> downloaderTypes)
        {
            _downloaders = downloaderTypes
                .Select(downloaderType => 
                    Activator.CreateInstance(downloaderType, _webClientFactory, _writeLineOutputter) as IDownloader)
                .ToArray();
        }

        private void GivenWordCounter(bool displayThreads)
        {
            _letterCounter = new LetterCounter(_downloaders, null, _writeLineOutputter, displayThreads);
        }

        private void WhenWordCounterRuns()
        {
            _wordCounterResults = _letterCounter.Run(_urls);
        }

        private void DisplayResult()
        {
            foreach (var result in _wordCounterResults)
            {
                _writeLineOutputter.WriteLine($"{result.Name} has {result.Count} words and took {result.Time:g}");
            }
        }
    }
}