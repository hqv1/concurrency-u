namespace LetterCount.Logic.Test
{
    public interface IWriteLineOutputter
    {
        void WriteLine(string message);
        void WriteLine(string format, params object[] args);
    }
}