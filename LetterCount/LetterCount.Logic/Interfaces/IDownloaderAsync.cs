using System.Collections.Generic;
using System.Threading.Tasks;

namespace LetterCount.Logic.Interfaces
{
    /// <summary>
    /// Download a set of URLs and return the total number of words
    /// </summary>
    public interface IDownloaderAsync
    {
        string Name { get; }
        Task<int> RunAsync(IEnumerable<string> urls);
    }
}