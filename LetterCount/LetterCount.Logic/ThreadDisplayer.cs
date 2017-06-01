using System;
using System.Threading;
using LetterCount.Logic.Test;

namespace LetterCount.Logic
{
    /// <summary>
    /// Constant variables and methods
    /// </summary>
    internal class ThreadDisplayer
    {
        private readonly IWriteLineOutputter _writeLineOutputter;

        public ThreadDisplayer(IWriteLineOutputter writeLineOutputter)
        {
            _writeLineOutputter = writeLineOutputter;
        }

        public static bool DisplayThread { get; set; }        

        /// <summary>
        /// It's supposed to get the number of words in a string. But it actually just returns the
        /// length of the content. It's not important
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static int GetWordCount(string content)
        {
            return content.Length;
        }

        /// <summary>
        /// Output that the thread has started to the console
        /// </summary>
        /// <param name="url"></param>
        public void OutputThreadStartedToConsole(string url)
        {
            if(!DisplayThread) return;
            _writeLineOutputter.WriteLine(
                $"For {url.PadRight(21)} thread {Thread.CurrentThread.ManagedThreadId.ToString().PadLeft(2)} started");
        }

        /// <summary>
        /// Output that the thread has completed to the console
        /// </summary>
        /// <param name="url"></param>
        public void OutputThreadCompletedToConsole(string url)
        {
            if (!DisplayThread) return;
            _writeLineOutputter.WriteLine(
                $"For {url.PadRight(21)} thread {Thread.CurrentThread.ManagedThreadId.ToString().PadLeft(2)} ended.");
        }
    }
}