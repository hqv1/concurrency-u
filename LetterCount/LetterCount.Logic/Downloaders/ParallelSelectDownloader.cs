using System.Collections.Generic;
using System.Linq;
using LetterCount.Logic.Interfaces;
using LetterCount.Logic.Test;

namespace LetterCount.Logic.Downloaders
{
    public class ParallelSelectDownloader : IDownloader
    {
        private readonly IWebClientFactory _webClientFactory;
        private readonly ThreadDisplayer _threadDisplayer;

        public ParallelSelectDownloader(IWebClientFactory webClientFactory, IWriteLineOutputter writeLineOutputter)
        {
            _webClientFactory = webClientFactory;
            _threadDisplayer = new ThreadDisplayer(writeLineOutputter);
        }

        public string Name => "Linq.AsParallel";

        public int Run(IEnumerable<string> urls)
        {
            var results = urls.AsParallel().Select(url =>
            {
                _threadDisplayer.OutputThreadStartedToConsole(url);
                string content;
                using (var client = _webClientFactory.Create())
                {
                    content = client.DownloadString(url);
                }
                var result = ThreadDisplayer.GetWordCount(content);
                _threadDisplayer.OutputThreadCompletedToConsole(url);
                return result;
            });

            return results.Sum();
        }
    }
}