using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LetterCount.Logic.Interfaces;
using LetterCount.Logic.Test;

namespace LetterCount.Logic.Downloaders
{
    /// <summary>
    /// This implementation of Parallel Loop with Async is buggy. We are running multiple threads
    /// and then awaiting. But because there's no WaitAll anywhere, it'll end the application
    /// before finishing the download.
    /// </summary>
    public class ParallelLoopAsyncDownloader : IDownloader
    {
        private readonly ThreadDisplayer _threadDisplayer;
        private readonly IWebClientFactory _webClientFactory;

        public ParallelLoopAsyncDownloader(IWebClientFactory webClientFactory, IWriteLineOutputter writeLineOutputter)
        {
            _webClientFactory = webClientFactory;
            _threadDisplayer = new ThreadDisplayer(writeLineOutputter);
        }

        public string Name => "Parallel.ForEach with Async";

        public int Run(IEnumerable<string> urls)
        {
            var result = 0;
            Parallel.ForEach(urls, async url =>
            {
                _threadDisplayer.OutputThreadStartedToConsole(url);
                string content;
                using (var client = _webClientFactory.Create())
                {
                    content = await client.DownloadStringTaskAsync(url);
                }
                var count = ThreadDisplayer.GetWordCount(content);
                Interlocked.Add(ref result, count);
                _threadDisplayer.OutputThreadCompletedToConsole(url);
            });
            return result;
        }
    }
}