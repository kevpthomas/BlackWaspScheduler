using System;

namespace Scheduler.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleIoCConfig.Instance.Register();
 
            Console.Clear();

            Console.WriteLine("Hello World!");
        }
    }
}
