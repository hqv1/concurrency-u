using LetterCount.Logic.Interfaces;

namespace LetterCount.Logic
{
    public class WebClientFactory : IWebClientFactory
    {
        private readonly WebClientStub.UrlSetting[] _urlSettings;

        public WebClientFactory(WebClientStub.UrlSetting[] urlSettings)
        {
            _urlSettings = urlSettings;
        }

        public WebClientStub Create()
        {
            return new WebClientStub(_urlSettings);
        }
    }
}