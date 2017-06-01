using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LetterCount.Logic.Interfaces;
using LetterCount.Logic.Test;

namespace LetterCount.Logic.Downloaders
{
    /// <summary>
    /// Downloads a list of urls using Parallel.ForEach. Uses threads and synchronously calls.
    /// </summary>
    public class ParallelLoopDownloader: IDownloader
    {
        private readonly IWebClientFactory _webClientFactory;
        private readonly ThreadDisplayer _threadDisplayer;

        public ParallelLoopDownloader(IWebClientFactory webClientFactory, IWriteLineOutputter writeLineOutputter)
        {
            _webClientFactory = webClientFactory;
            _threadDisplayer = new ThreadDisplayer(writeLineOutputter);
        }

        public string Name => "Parallel.ForEach";

        public int Run(IEnumerable<string> urls)
        {
            var result = 0;
            Parallel.ForEach(urls, url =>
            {
                _threadDisplayer.OutputThreadStartedToConsole(url);
                string content;
                using (var client = _webClientFactory.Create())
                {
                    content = client.DownloadString(url);
                }
                var count = ThreadDisplayer.GetWordCount(content);
                Interlocked.Add(ref result, count);
                _threadDisplayer.OutputThreadCompletedToConsole(url);
            });
            return result;
        }
    }
}