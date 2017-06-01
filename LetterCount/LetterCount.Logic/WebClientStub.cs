using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LetterCount.Logic
{
    /// <summary>
    /// Stub for WebClient. Doesn't actually download anything. It'll delay and then returns a random 
    /// string. The length of the delay and the length of the returned content is based on the 
    /// urlSettings.
    /// </summary>
    public class WebClientStub : IDisposable
    {
        private readonly IEnumerable<UrlSetting> _urlSettings;

        public class UrlSetting
        {
            
            public UrlSetting(string url, int delayTime)
                :this(url, delayTime, delayTime)
            {

            }

            public UrlSetting(string url, int delayTime, int numberOfWords)
            {
                Url = url;
                DelayTime = delayTime;
                NumberOfWords = numberOfWords;
            }
           

            public string Url { get; set; }
            public int DelayTime { get; set; }
            public int NumberOfWords { get; set; }
        }

        public WebClientStub(IEnumerable<UrlSetting> urlSettings )
        {
            _urlSettings = urlSettings;
        }

        public string DownloadString(string url)
        {
            var urlSetting = _urlSettings.First(x => url.Contains(x.Url));
            Thread.Sleep(urlSetting.DelayTime);
            return new string('a', urlSetting.NumberOfWords);
        }

        public async Task<string> DownloadStringTaskAsync(string url)
        {
            var urlSetting = _urlSettings.First(x => url.Contains(x.Url));
            await Task.Delay(urlSetting.DelayTime);
            return new string('a', urlSetting.NumberOfWords);
        }

        public void Dispose()
        {
        }       
    }
}