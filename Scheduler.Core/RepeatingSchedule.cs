using System;

namespace Scheduler.Core
{
    /// <summary>
    /// The RepeatingSchedule class is the base class for schedules that may generate multiple appointments.
    /// This abstract class inherits functionality from the Schedule type, adding one extra property to hold a scheduling range.
    /// The scheduling range is a period of dates between which appointments may be generated.
    /// If the date being checked is outside of this range, even if it fulfills any other rules of the schedule,
    /// no appointment will be generated. This allows for temporary schedules that start on a given date and cease at a later date.
    /// The DateIsInPeriod method checks that a date is within the scheduling period. 
    /// </summary>
    public abstract class RepeatingSchedule : Schedule
    {
        public Period SchedulingRange { get; set; }
 
        protected bool DateIsInPeriod(DateTime date)
        {
            return date >= SchedulingRange.Start && date <= SchedulingRange.End;
        }
    }}