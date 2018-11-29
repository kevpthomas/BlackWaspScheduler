using System.Collections.Generic;

namespace Scheduler.Core
{
    public interface ICalendarGenerator
    {
        IEnumerable<Appointment> GenerateCalendar(Period period, IEnumerable<ISchedule> schedules);
    }
}