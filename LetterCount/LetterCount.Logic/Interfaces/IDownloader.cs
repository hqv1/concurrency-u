using System.Collections.Generic;

namespace LetterCount.Logic.Interfaces
{
    /// <summary>
    /// Download a set of URLs and return the total number of words
    /// </summary>
    public interface IDownloader
    {        
        string Name { get; }       
        int Run(IEnumerable<string> urls);
    }
}