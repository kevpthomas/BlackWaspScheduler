using System;

namespace Scheduler.Core.Entities
{
    /// <summary>
    /// Schedule for a single appointment at a given date and time.
    /// </summary>
    public class SingleSchedule : Schedule
    {
        public DateTime Date { get; set; }
 
        public override bool OccursOnDate(DateTime date)
        {
            return Date.Date == date.Date;
        }
    }
}