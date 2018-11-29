using System;

namespace Scheduler.Core
{
    public interface ISchedule
    {
        string Name { get; set; }

        TimeOfDay TimeOfDay { get; set; }

        bool OccursOnDate(DateTime date);
    }
}