using System;
using System.Collections.Generic;
using System.Linq;

namespace Scheduler.Core.Entities
{
    /// <summary>
    /// Schedule for a pattern of appointments that repeats weekly.
    /// You can to specify a time for the appointments and on which of the seven days of the week they should occur.
    /// This allows options such as creating an appointment on every weekday but excluding weekends.
    /// </summary>
    public class WeeklySchedule : RepeatingSchedule
    {
        private List<DayOfWeek> _days;
 
        public void SetDays(IEnumerable<DayOfWeek> days)
        {
            _days = days.Distinct().ToList();
        }
 
        public override bool OccursOnDate(DateTime date)
        {
            return DateIsInPeriod(date) && _days.Contains(date.DayOfWeek);
        }
    }
}