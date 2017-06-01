using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LetterCount.Logic.Interfaces;
using LetterCount.Logic.Test;

namespace LetterCount.Logic.Downloaders
{
    public class TaskRunDownloader : IDownloader
    {
        private readonly IWebClientFactory _webClientFactory;
        private readonly ThreadDisplayer _threadDisplayer;

        public TaskRunDownloader(IWebClientFactory webClientFactory, IWriteLineOutputter writeLineOutputter)
        {
            _webClientFactory = webClientFactory;
            _threadDisplayer = new ThreadDisplayer(writeLineOutputter);
        }

        public string Name => "Task.Run";

        public int Run(IEnumerable<string> urls)
        {
            var result = 0;
            var tasks = urls.Select(url => Task.Run(() =>
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
            })).ToArray();

            Task.WhenAll(tasks).Wait();
            return result;
        }      
    }
}