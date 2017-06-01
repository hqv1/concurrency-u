using Xunit.Abstractions;

namespace LetterCount.Logic.Test
{
    public class WriteLineOutputter : IWriteLineOutputter
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WriteLineOutputter(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public void WriteLine(string message)
        {
            _testOutputHelper.WriteLine(message);
        }

        public void WriteLine(string format, params object[] args)
        {
            _testOutputHelper.WriteLine(format, args);
        }
    }
}