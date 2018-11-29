using System;
using Scheduler.Core.Types;

namespace Scheduler.Core.Interfaces
{
    public interface ISchedule
    {
        string Name { get; set; }

        TimeOfDay TimeOfDay { get; set; }

        bool OccursOnDate(DateTime date);
    }
}