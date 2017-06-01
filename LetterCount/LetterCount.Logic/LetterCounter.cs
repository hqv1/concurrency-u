using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LetterCount.Logic.Interfaces;
using LetterCount.Logic.Test;

namespace LetterCount.Logic
{
    public class LetterCounter
    {
        private readonly IWriteLineOutputter _writeLineOutputter;
        private readonly bool _displayThreads;
        private readonly IEnumerable<IDownloader> _downloaders;
        private readonly IEnumerable<IDownloaderAsync> _downloaderAsyncs;

        public LetterCounter(IEnumerable<IDownloader> downloaders, 
            IEnumerable<IDownloaderAsync> downloaderAsyncs,
            IWriteLineOutputter writeLineOutputter,
            bool displayThreads = false)
        {
            _writeLineOutputter = writeLineOutputter;
            _displayThreads = displayThreads;
            _downloaders = downloaders ?? new List<IDownloader>();
            _downloaderAsyncs = downloaderAsyncs ?? new List<IDownloaderAsync>();

            ThreadDisplayer.DisplayThread = _displayThreads;
        }

        public IEnumerable<WordCounterResult> Run(IEnumerable<string> urls)
        {
            var results = new List<WordCounterResult>();
            urls = urls.ToList();
            foreach (var downloader in _downloaders)
            {
                var result = Run(urls, downloader.Name, downloader.Run);
                results.Add(result);
            }
            
            return results;
        }

        /// <summary>
        /// Outputs to the console useful information and calculate how long the downloads take.
        /// </summary>
        /// <param name="urls">Urls</param>
        /// <param name="runType">Type of action being run</param>
        /// <param name="func">Func that does the work</param>
        private WordCounterResult Run(IEnumerable<string> urls, string runType, Func<IEnumerable<string>, int> func)
        {           
            if (_displayThreads)
            {
                _writeLineOutputter.WriteLine($"Running {runType}");
                _writeLineOutputter.WriteLine($"Primary thread {Thread.CurrentThread.ManagedThreadId.ToString().PadLeft(2)} started");
            }
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var result = func.Invoke(urls);

            stopWatch.Stop();
            if (_displayThreads)
            {
                _writeLineOutputter.WriteLine($"Primary thread {Thread.CurrentThread.ManagedThreadId.ToString().PadLeft(2)} ended\n\n");
            }

            return new WordCounterResult(runType, result, stopWatch.Elapsed);
        }

        private void RunTask(IEnumerable<string> urls, string runType, Func<IEnumerable<string>, Task<int>> func)
        {
            _writeLineOutputter.WriteLine($"Running {runType} - Async");
            _writeLineOutputter.WriteLine($"Primary thread {Thread.CurrentThread.ManagedThreadId.ToString().PadLeft(2)} started");
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var result = func.Invoke(urls).Result;
            _writeLineOutputter.WriteLine($"Count is {result}");

            stopWatch.Stop();
            _writeLineOutputter.WriteLine($"Time took to complete is {stopWatch.Elapsed}");
            _writeLineOutputter.WriteLine($"Primary thread {Thread.CurrentThread.ManagedThreadId.ToString().PadLeft(2)} ended\n\n");
        }
    }

    public class WordCounterResult
    {
        public WordCounterResult(string name, int count, TimeSpan time)
        {
            Name = name;
            Count = count;
            Time = time;
        }

        public string Name { get; private set; }
        public int Count { get; private set; }
        public TimeSpan Time { get; private set; }
    }
}
