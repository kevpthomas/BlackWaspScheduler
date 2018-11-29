using System.Collections.Generic;
using Scheduler.Core.Entities;

namespace Scheduler.Core.Interfaces
{
    public interface ICalendarGenerator
    {
        IEnumerable<Appointment> GenerateCalendar(Period period, IEnumerable<ISchedule> schedules);
    }
}