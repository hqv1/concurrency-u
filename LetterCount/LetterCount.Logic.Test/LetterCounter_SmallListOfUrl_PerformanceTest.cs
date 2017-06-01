using System;
using System.Collections.Generic;
using LetterCount.Logic.Downloaders;
using LetterCount.Logic.Interfaces;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable InconsistentNaming

namespace LetterCount.Logic.Test
{
    public class LetterCounter_SmallListOfUrl_PerformanceTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        private LetterCounter _letterCounter;
        private readonly IWriteLineOutputter _writeLineOutputter;

        private readonly string[] _urls;
        private IDownloader[] _downloaders;
        private readonly IWebClientFactory _webClientFactory;
        private IEnumerable<WordCounterResult> _wordCounterResults;
        
        public LetterCounter_SmallListOfUrl_PerformanceTest(ITestOutputHelper testOutputHelperHelper)
        {
            _testOutputHelper = testOutputHelperHelper;
            _writeLineOutputter = new WriteLineOutputter(testOutputHelperHelper);

            _urls = new[]
            {
                "http://www.cnn.com",
                "http://www.yahoo.com",
                "http://www.fox.com",
                "http://www.abc.com",
            };

            var urlSettings = new[]
            {
                new WebClientStub.UrlSetting("cnn", 400),
                new WebClientStub.UrlSetting("yahoo", 500),
                new WebClientStub.UrlSetting("fox", 700),
                new WebClientStub.UrlSetting("abc", 400),
            };
            _webClientFactory = new WebClientFactory(urlSettings);    
            
        }

        [Fact]
        public void Should_RunGetSynchronousPerformance()
        {
            _downloaders = new IDownloader[]
            {
                new SynchronousDownloader(_webClientFactory, _writeLineOutputter),
            };
            GivenWordCounter(false);
            WhenWordCounterRuns();
            DisplayResult();
        }

        [Fact]
        public void Should_RunGetConcurrentPerformances()
        {
            GivenAsyncDownloaders();
            GivenWordCounter(displayThreads: false);
            WhenWordCounterRuns();
            DisplayResult();
        }

        [Fact]
        public void Should_RunGetConcurrentThreads()
        {           
            GivenAsyncDownloaders();
            GivenWordCounter(displayThreads:true);
            WhenWordCounterRuns();
        }

        private void GivenAsyncDownloaders()
        {
            _downloaders = new IDownloader[]
            {
                new TaskRunDownloader(_webClientFactory, _writeLineOutputter),
                new ParallelSelectDownloader(_webClientFactory, _writeLineOutputter),
                new ParallelLoopDownloader(_webClientFactory, _writeLineOutputter),
                new AsyncDownloader(_webClientFactory, _writeLineOutputter),
            };
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
                _testOutputHelper.WriteLine($"{result.Name} has {result.Count} words and took {result.Time:g}");
            }
        }       
    }
}
