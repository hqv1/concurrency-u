using System.Collections.Generic;
using LetterCount.Logic.Interfaces;
using LetterCount.Logic.Test;

namespace LetterCount.Logic.Downloaders
{
    /// <summary>
    /// Download a list of urls synchronously
    /// </summary>
    public class SynchronousDownloader : IDownloader
    {
        private readonly IWebClientFactory _webClientFactory;
        private readonly ThreadDisplayer _threadDisplayer;

        public string Name => "Synchronous";

        public SynchronousDownloader(IWebClientFactory webClientFactory, IWriteLineOutputter writeLineOutputter)
        {
            _webClientFactory = webClientFactory;
            _threadDisplayer = new ThreadDisplayer(writeLineOutputter);
        }

        public int Run(IEnumerable<string> urls)
        {
            var result = 0;
            foreach (var url in urls)
            {
                _threadDisplayer.OutputThreadStartedToConsole(url);
                string content;   
                using (var client = _webClientFactory.Create())
                {
                    content = client.DownloadString(url);
                }
                result += ThreadDisplayer.GetWordCount(content);
                _threadDisplayer.OutputThreadCompletedToConsole(url);
            }
            return result;
        }
    }
}