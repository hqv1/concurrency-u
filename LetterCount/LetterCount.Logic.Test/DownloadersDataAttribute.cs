using System.Collections.Generic;
using System.Reflection;
using LetterCount.Logic.Downloaders;
using Xunit.Sdk;

namespace LetterCount.Logic.Test
{
    public class DownloadersDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var objs = new List<object[]>
            {
                new[] {(object) new[] {typeof(TaskRunDownloader)}},
                new[] {(object) new[] {typeof(AsyncDownloader)}}
            };
            return objs;
        }
    }
}