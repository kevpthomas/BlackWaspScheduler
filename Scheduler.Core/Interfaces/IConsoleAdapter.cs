namespace Scheduler.Core.Interfaces
{
    public interface IConsoleAdapter
    {
        void WriteLine(string value);
        void WriteLine(string format, object arg0);
    }
}