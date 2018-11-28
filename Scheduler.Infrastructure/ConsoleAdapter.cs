using System;
using Scheduler.Core;

namespace Scheduler.Infrastructure
{
    public class ConsoleAdapter : IConsoleAdapter
    {
        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }

        public void WriteLine(string format, object arg0)
        {
            Console.WriteLine(format, arg0);
        }
    }
}