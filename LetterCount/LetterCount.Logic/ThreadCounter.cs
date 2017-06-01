using System.Collections.Concurrent;
using System.Collections.Generic;

namespace LetterCount.Logic
{
    public class ThreadCounter
    {
        private readonly ConcurrentDictionary<int, int> _threadCounts = new ConcurrentDictionary<int, int>();

        public void AddThread(int threadId)
        {
            _threadCounts.AddOrUpdate(threadId, 1, (id, count) => count + 1);
        }

        public IDictionary<int,int> ThreadCounts => _threadCounts;
    }
}