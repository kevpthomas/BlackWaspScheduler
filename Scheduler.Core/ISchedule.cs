using System;

namespace Scheduler.Core
{
    public interface ISchedule
    {
        string Name { get; set; }

        //TODO: refactor to a non-primitive type that enforces only hours and minutes
        TimeSpan TimeOfDay { get; set; }

        bool OccursOnDate(DateTime date);
    }
}