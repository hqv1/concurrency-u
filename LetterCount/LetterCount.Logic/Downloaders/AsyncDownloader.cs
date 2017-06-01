using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetterCount.Logic.Interfaces;
using LetterCount.Logic.Test;

namespace LetterCount.Logic.Downloaders
{
    /// <summary>
    /// Downloads a list of URLS asynchronously using the async/await pattern.
    /// 
    /// Fastest for large number of downloads. But watch out for DDOS your external resource. 
    /// </summary>
    public class AsyncDownloader : IDownloader, IDownloaderAsync
    {
        private readonly ThreadDisplayer _threadDisplayer;
        private readonly IWebClientFactory _webClientFactory;
        
        public AsyncDownloader(IWebClientFactory webClientFactory, IWriteLineOutputter writeLineOutputter)
        {
            _threadDisplayer = new ThreadDisplayer(writeLineOutputter);
            _webClientFactory = webClientFactory;            
        }
      
        public string Name => "Async";
        
        public int Run(IEnumerable<string> urls)
        {
            var notStartedTasks = urls.Select(Run); // Create tasks, don't start them (lazy load)
            var taskAwaitable = Task.WhenAll(notStartedTasks).ConfigureAwait(false); // Get another awaitable tasks
            var result = taskAwaitable.GetAwaiter().GetResult().Sum(); // Start and Wait for the tasks here. Get the results and sum them up.

            return result;
        }

        /// <summary>
        /// Similar to Run()
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        public async Task<int> RunAsync(IEnumerable<string> urls)
        {
            var notStartedTasks = urls.Select(Run); // Create tasks, don't start them (lazy load)
            var tasks = notStartedTasks.ToArray(); // Start all the tasks. If you comment this out, tasks will start where we do our Wait() or Result()
            var results = await Task.WhenAll(tasks).ConfigureAwait(false);
            return results.Sum();
        }

        private async Task<int> Run(string url)
        {
            _threadDisplayer.OutputThreadStartedToConsole(url);
            string content;
            using (var client = _webClientFactory.Create())
            {
                content = await client.DownloadStringTaskAsync(url).ConfigureAwait(false);                
            }
            _threadDisplayer.OutputThreadCompletedToConsole(url);
            var count = ThreadDisplayer.GetWordCount(content);
            return count;
        }        
    }
    
}